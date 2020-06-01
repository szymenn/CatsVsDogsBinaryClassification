using System;

namespace CatsVsDogs.Core.Dto
{
    public class PredictionDto
    {
        public string PredictedValue { get; set; }
        public DateTime PredictionTime { get; set; }
    }
}