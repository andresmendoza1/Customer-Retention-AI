using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerRetentionAPI.Models;

namespace CustomerRetentionAPI.Data.Repositories
{
    public interface ICustomerChurnPredictionRepository
    {
        Task<IEnumerable<CustomerChurnPrediction>> GetAllAsync();
        Task<CustomerChurnPrediction> GetByCustomerIdAsync(Guid customerId);
        Task AddAsync(CustomerChurnPrediction prediction);
        Task<IEnumerable<CustomerChurnPrediction>> GetAllPredictionsById(Guid id);
    }
}
