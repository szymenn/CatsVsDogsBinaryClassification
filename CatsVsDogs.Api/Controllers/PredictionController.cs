using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CatsVsDogs.Core.Dto;
using CatsVsDogs.Core.Interfaces.Repositories;
using CatsVsDogs.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatsVsDogs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictionController : ControllerBase
    {
        private readonly IPredictionService _predictionService;

        public PredictionController(IPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        [HttpPost]
        public IActionResult Predict([FromForm] ImageDto imageDto)
        {
            return Ok(_predictionService.ClassifyImage(imageDto.Image));
        }

        [HttpGet]
        public IActionResult GetPredictionsHistory()
        {
            return Ok(_predictionService.GetPredictionsHistory());
        }

    }
}