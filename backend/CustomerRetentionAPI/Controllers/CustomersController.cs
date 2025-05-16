using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CustomerRetentionAPI.Models;
using CustomerRetentionAPI.Services;
using CustomerRetentionAPI.Data.Repositories;

namespace CustomerRetentionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerChurnDataRepository _customerChurnDataRepository;
        private readonly ICustomerChurnPredictionRepository _customerChurnPredictionRepository;
        private readonly CustomerPredictionService _predictionService;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ICustomerRepository customerRepository, ICustomerChurnDataRepository customerChurnDataRepository, ICustomerChurnPredictionRepository customerChurnPredictionRepository, CustomerPredictionService predictionService, ILogger<CustomersController> logger)
        {
            _customerRepository = customerRepository;
            _customerChurnDataRepository = customerChurnDataRepository;
            _customerChurnPredictionRepository = customerChurnPredictionRepository;
            _predictionService = predictionService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest("Customer cannot be null");
            }

            customer.Id = Guid.NewGuid(); // Automatically assign a unique ID
            await _customerRepository.AddAsync(customer);

            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _customerRepository.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] Customer customer)
        {
            if (customer == null || id != customer.Id)
            {
                return BadRequest();
            }

            var exists = await _customerRepository.ExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            await _customerRepository.UpdateAsync(customer);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var exists = await _customerRepository.ExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            await _customerRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("churn-prediction")]
        public async Task<IActionResult> RequestPredictCustomerChurn([FromBody] string customerId)
        {
            Guid customerIdGuid;

            if (string.IsNullOrEmpty(customerId))
            {
                return BadRequest("Customer ID is required.");
            }
                       
            if (!Guid.TryParse(customerId, out customerIdGuid))
            {
                return BadRequest("Invalid CustomerId format.");
            }
        

            if (customerIdGuid == Guid.Empty)
            {
                return BadRequest("A valid CustomerId is required.");
            }
            
            CustomerChurnData customerChurnData = await _customerChurnDataRepository.GetByCustomerIdAsync(customerIdGuid);
            
            if (customerChurnData == null)
            {
                return NotFound("CustomerChurnData not found for the given CustomerId.");
            }

            try
            {

                var churnPrediction = await _predictionService.PredictCustomerChurn(customerChurnData);

                // 🔹 Generar estrategia de retención SOLO si el abandono es probable
                string retentionAction = churnPrediction.Prediction == 1 ? "Enviar cupón de descuento del 20%" : string.Empty;

                return Ok(new 
                {
                    CustomerId = churnPrediction.CustomerId,
                    IsChurnLikely = churnPrediction.Prediction,
                    ChurnProbability = churnPrediction.Prediction == 1 ? churnPrediction.PredictionProbability : 0, 
                    RetentionAction = retentionAction
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error predicting churn for customer {customerChurnData.CustomerId}: {ex.Message}");
                return StatusCode(500, new 
                { 
                    Message = "An error occurred while predicting customer churn.",
                    ErrorDetails = ex.Message 
                });
            }
        }

        [HttpPost("save-prediction")]
        public async Task<IActionResult> AddPrediction([FromBody] CustomerChurnPrediction prediction)
        {
            if (prediction == null)
            {
                return BadRequest("Prediction data is required.");
            }
            await _customerChurnPredictionRepository.AddAsync(prediction);
            return CreatedAtAction(nameof(GetPredictionById), new { id = prediction.CustomerId }, prediction);
        }

        [HttpGet("predictions")]
        public async Task<IActionResult> GetAllPredictions()
        {
            var predictions = await _customerChurnPredictionRepository.GetAllAsync();
            return Ok(predictions);
        }

        [HttpGet("predictions/{id}")]
        public async Task<IActionResult> GetPredictionById(Guid id)
        {
            var prediction = await _customerChurnPredictionRepository.GetByCustomerIdAsync(id);
            if (prediction == null)
            {
                return NotFound();
            }
            return Ok(prediction);
        }

        [HttpGet("all-predictions/{id}")]
        public async Task<IActionResult> GetAllPredictionsById(Guid id)
        {
            var predictions = await _customerChurnPredictionRepository.GetAllPredictionsById(id);
            return Ok(predictions);
        }

        [HttpGet("customer-data/{id}")]
        public async Task<IActionResult> GetCustomerDataById(Guid id)
        {
            var customerData = await _customerChurnDataRepository.GetByCustomerIdAsync(id);
            if (customerData == null)
            {
                return NotFound();
            }
            return Ok(customerData);
        }
    }
}
