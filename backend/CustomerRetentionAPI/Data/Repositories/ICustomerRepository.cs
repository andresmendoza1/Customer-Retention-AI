using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerRetentionAPI.Models;

namespace CustomerRetentionAPI.Data.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(Guid id);
        Task<Customer> AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
