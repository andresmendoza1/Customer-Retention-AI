 using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CustomerRetentionAPI.Models;

namespace CustomerRetentionAPI.Data.Repositories
{
    public class CustomerChurnDataRepository : ICustomerChurnDataRepository
    {
        private readonly CRMContext _context;

        public CustomerChurnDataRepository(CRMContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerChurnData>> GetAllAsync()
        {
            return await _context.CustomerChurnData.ToListAsync();
        }

        public async Task<CustomerChurnData> GetByCustomerIdAsync(Guid customerId)
        {
            return await _context.CustomerChurnData.Where(c => c.CustomerId == customerId).OrderByDescending(c => c.CreatedAt).FirstOrDefaultAsync();
        }

        public async Task<CustomerChurnData> AddAsync(CustomerChurnData customerChurnData)
        {
            await _context.CustomerChurnData.AddAsync(customerChurnData);
            await _context.SaveChangesAsync();
            return customerChurnData;
        }

        public async Task UpdateAsync(CustomerChurnData customerChurnData)
        {
            _context.Entry(customerChurnData).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid customerId)
        {
            var customerChurnData = await _context.CustomerChurnData.FindAsync(customerId);
            if (customerChurnData != null)
            {
                _context.CustomerChurnData.Remove(customerChurnData);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid customerId)
        {
            return await _context.CustomerChurnData.AnyAsync(c => c.CustomerId == customerId);
        }
    }
}
