using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CatsVsDogs.Core.Dto;
using CatsVsDogs.Core.Entities;
using CatsVsDogs.Core.Interfaces;
using CatsVsDogs.Core.Interfaces.Repositories;
using CatsVsDogs.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.ML;
using Microsoft.ML;

namespace CatsVsDogs.Core.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly PredictionEnginePool<ModelInput, ModelOutput> _predictionEngine;
        private readonly IPredictionRepository _predictionRepository;

        public PredictionService(PredictionEnginePool<ModelInput, ModelOutput> predictionEngine, IPredictionRepository predictionRepository)
        {
            _predictionEngine = predictionEngine;
            _predictionRepository = predictionRepository;
        }
        
        public string ClassifyImage(IFormFile imageFile)
        {
            var prediction = _predictionEngine.Predict(modelName: "ImageModel", GetModelInput(imageFile));
            
            _predictionRepository.Save(new PredictionHistory
            {
                PredictedValue = prediction.PredictedLabel,
                PredictionTime = DateTime.Now
            });

            File.Delete(imageFile.FileName);

            return prediction.PredictedLabel;
        }

        public IEnumerable<PredictionDto> GetPredictionsHistory()
        {
            return _predictionRepository.GetAll().Select(predictionHistory => new PredictionDto
            {
                PredictedValue = predictionHistory.PredictedValue,
                PredictionTime = predictionHistory.PredictionTime
            });
        }

        private static ModelInput GetModelInput(IFormFile imageFile)
        {
            if (imageFile.Length > 0)
            {
                using var fileStream = new FileStream(imageFile.FileName, FileMode.Create);
                imageFile.CopyTo(fileStream);
            }

            return new ModelInput {Image = File.ReadAllBytes(imageFile.FileName)};
        } 
    }
}