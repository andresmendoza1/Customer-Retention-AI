using System;

namespace CustomerRetentionAPI.Models
{
    public class CustomerChurnPrediction
    {
        public Guid CustomerChurnPredictionId { get; set; } // Unique Identifier     
        public Guid CustomerId { get; set; }
        public int Prediction { get; set; }
        public decimal PredictionProbability { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
