using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerRetentionAPI.Models;

namespace CustomerRetentionAPI.Data.Repositories
{
    public interface ICustomerChurnDataRepository
    {
        Task<IEnumerable<CustomerChurnData>> GetAllAsync();
        Task<CustomerChurnData> GetByCustomerIdAsync(Guid customerId);
        Task<CustomerChurnData> AddAsync(CustomerChurnData customerChurnData);
        Task UpdateAsync(CustomerChurnData customerChurnData);
        Task DeleteAsync(Guid customerId);
        Task<bool> ExistsAsync(Guid customerId);
    }
}
