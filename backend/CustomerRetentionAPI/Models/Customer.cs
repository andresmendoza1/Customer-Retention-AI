using System;

namespace CustomerRetentionAPI.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
