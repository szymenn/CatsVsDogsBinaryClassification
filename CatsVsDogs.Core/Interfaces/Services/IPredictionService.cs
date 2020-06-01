using System.Collections.Generic;
using CatsVsDogs.Core.Dto;
using CatsVsDogs.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace CatsVsDogs.Core.Interfaces.Services
{
    public interface IPredictionService
    {
        string ClassifyImage(IFormFile imageFile);
        IEnumerable<PredictionDto> GetPredictionsHistory();
    }
}