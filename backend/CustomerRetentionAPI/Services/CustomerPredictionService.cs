using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CustomerRetentionAPI.Models;
using Microsoft.Extensions.Logging;

namespace CustomerRetentionAPI.Services
{
    public class CustomerPredictionService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly ILogger<CustomerPredictionService> _logger;

        public CustomerPredictionService(ILogger<CustomerPredictionService> logger)
        {
            _logger = logger;
        }

       public async Task<CustomerChurnPrediction> PredictCustomerChurn(CustomerChurnData customerChurnData)
        {
            try 
            {
                var url = "http://127.0.0.1:8000/predict";
                
                string json = JsonConvert.SerializeObject(customerChurnData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"🔹 Churn Prediction API Response: {result}");

                var churnPrediction = JsonConvert.DeserializeObject<ChurnPrediction>(result);
                
                if (churnPrediction != null)
                {
                    return new CustomerChurnPrediction
                    {
                        CustomerId = customerChurnData.CustomerId,
                        Prediction = Convert.ToInt32(churnPrediction.Prediction),
                        PredictionProbability = Convert.ToDecimal(churnPrediction.PredictionProbability)
                    };
                }else
                {
                    return new CustomerChurnPrediction
                    {
                        CustomerId = customerChurnData.CustomerId,
                        Prediction = 0,
                        PredictionProbability = 0
                    };
                }
            
            }
            catch (HttpRequestException e)
            {
                _logger.LogError($"Error calling churn prediction API: {e.Message}");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"Unexpected error in churn prediction: {e.Message}");
                throw;
            }
        }
    }
}
