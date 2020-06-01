using System;

namespace CatsVsDogs.Core.Entities
{
    public class PredictionHistory
    {
        public Guid Id { get; set; }
        public string PredictedValue { get; set; }
        public DateTime PredictionTime { get; set; }
    }
}