using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CustomerRetentionAPI.Models;

namespace CustomerRetentionAPI.Data.Repositories
{
    public class CustomerChurnPredictionRepository : ICustomerChurnPredictionRepository
    {
        private readonly CRMContext _context;

        public CustomerChurnPredictionRepository(CRMContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerChurnPrediction>> GetAllAsync()
        {
            return await _context.CustomerChurnPredictions.GroupBy(c => c.CustomerId).Select(g => g.OrderByDescending(c => c.CreatedAt).FirstOrDefault()).ToListAsync();
        }

        public async Task<CustomerChurnPrediction> GetByCustomerIdAsync(Guid customerId)
        {
            return await _context.CustomerChurnPredictions.Where(c => c.CustomerId == customerId).OrderByDescending(c => c.CreatedAt).FirstOrDefaultAsync();
        }

        public async Task AddAsync(CustomerChurnPrediction prediction)
        {
            prediction.CustomerChurnPredictionId = Guid.NewGuid();
            prediction.CreatedAt = DateTime.UtcNow;
            await _context.CustomerChurnPredictions.AddAsync(prediction);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CustomerChurnPrediction>> GetAllPredictionsById(Guid id)
        {
            var predictions = await _context.CustomerChurnPredictions.Where(p => p.CustomerId == id).ToListAsync();
            return predictions;
        }
    }
}
