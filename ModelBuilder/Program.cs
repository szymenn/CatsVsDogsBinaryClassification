using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using ModelBuilder.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Vision;

namespace ModelBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            var mlContext = new MLContext();
            
            var projectDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../"));
            var trainRelativePath = Path.Combine(projectDirectory, "Data\\train");
            var workspaceRelativePath = Path.Combine(projectDirectory, "Workspace");

            var trainImages = LoadImagesFromDirectory(trainRelativePath);

            var trainData = mlContext.Data.LoadFromEnumerable(trainImages);

            var shuffledData = mlContext.Data.ShuffleRows(trainData);            
            
            var preprocessingPipeline = mlContext.Transforms.Conversion.MapValueToKey(
                    inputColumnName: "Label",
                    outputColumnName: "LabelAsKey")
                .Append(mlContext.Transforms.LoadRawImageBytes(
                    outputColumnName: "Image",
                    imageFolder: trainRelativePath,
                    inputColumnName: "ImagePath"));

            var preProcessedData = preprocessingPipeline
                .Fit(shuffledData)
                .Transform(shuffledData);
            
            var trainTestSplit = mlContext.Data.TrainTestSplit(preProcessedData, testFraction: 0.3);
            var trainSet = trainTestSplit.TrainSet;

            var validationTestSplit = mlContext.Data.TrainTestSplit(trainTestSplit.TestSet);
            var validationSet = validationTestSplit.TrainSet;
            var testSet = validationTestSplit.TestSet;

            var classifierOptions = new ImageClassificationTrainer.Options()
            {
                FeatureColumnName = "Image",
                LabelColumnName = "LabelAsKey",
                ValidationSet = validationSet,
                Arch = ImageClassificationTrainer.Architecture.ResnetV2101,
                MetricsCallback = Console.WriteLine,
                TestOnTrainSet = false,
                ReuseTrainSetBottleneckCachedValues = true,
                ReuseValidationSetBottleneckCachedValues = true,
                WorkspacePath=workspaceRelativePath,
                Epoch = 100
            };
            
            var trainingPipeline = mlContext.MulticlassClassification.Trainers.ImageClassification(classifierOptions)
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            var trainedModel = trainingPipeline.Fit(trainSet);
            mlContext.Model.Save(trainedModel, trainData.Schema, "model.zip");

            Console.ReadLine();
        }

        private static IEnumerable<ImageData> LoadImagesFromDirectory(string folder)
        {
            var files = Directory.GetFiles(folder, "*",
                searchOption: SearchOption.AllDirectories);

            foreach (var file in files)
            {
                    var label = Path.GetFileName(file).Split(".")[0];
                    
                    yield return new ImageData()
                    {
                        ImagePath = file,
                        Label = label
                    };
            }
        }
    }
}