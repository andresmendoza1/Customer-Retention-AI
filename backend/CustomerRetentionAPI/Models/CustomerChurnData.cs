using System;

namespace CustomerRetentionAPI.Models
{
    public class CustomerChurnData
    {
        public Guid CustomerChurnDataId { get; set; } // Unique Identifier
        public Guid CustomerId { get; set; }
        public int Tenure { get; set; }
        public int OrderCount { get; set; }
        public int DaysSinceLastOrder { get; set; }
        public decimal CashbackAmount { get; set; }
        public int SatisfactionScore { get; set; }
        public bool Complain { get; set; }
        public int HourSpendOnApp { get; set; }
        public int NumberOfDeviceRegistered { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
