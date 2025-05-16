using System;
using Newtonsoft.Json;

namespace CustomerRetentionAPI.Models
{
     public class ChurnPrediction
    {
        [JsonProperty("prediction")]
        public bool Prediction { get; set; }

        [JsonProperty("probability")]
        [JsonConverter(typeof(DecimalJsonConverter))]
        public decimal PredictionProbability { get; set; }
    }

    // Custom JSON converter to ensure decimal precision
    public class DecimalJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(decimal);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Float || reader.TokenType == JsonToken.Integer)
            {
                // Parse the value and round to 3 decimal places
                return decimal.Round(Convert.ToDecimal(reader.Value), 3);
            }
            return 0m;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(decimal.Round((decimal)value, 3));
        }
    }
}
