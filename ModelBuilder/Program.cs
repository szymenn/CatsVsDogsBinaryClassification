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
//            DataViewSchema schema;
////            var loadedModel = mlContext.Model.Load(Path.Combine(projectDirectory, "Model.zip"), out schema);
//
////            var predictionEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(loadedModel);
////            var predictValues = testSet.Schema.Take(10);
//
//            var predictionEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(trainedModel);
//
//            var testImages = LoadImagesFromDirectory(Path.Combine(projectDirectory, "Data\\test1"));
//            var testData = mlContext.Data.LoadFromEnumerable(testImages);
//
//            var preProcessedTestData = preprocessingPipeline.Fit(testData).Transform(testData);
//            var predictionData = trainedModel.Transform(preProcessedTestData);
//
//            var predictions = mlContext.Data.CreateEnumerable<ModelOutput>(predictionData, reuseRowObject: true).Take(1);
//
//            Console.WriteLine("Classifying our cat");
//            foreach (var prediction in predictions)
//            {
//                Console.WriteLine
//                    ($"Image: {Path.GetFileName(prediction.ImagePath)}, Actual Value: {prediction.Label} | Predicted Value: {prediction.PredictedLabel}");
//            }
//            
//            // IDataView predictionData = trainedModel.Transform(mlContext.Data.ShuffleRows(testSet));
//
//            // IEnumerable<ModelOutput> predictions = mlContext.Data
//            //     .CreateEnumerable<ModelOutput>(predictionData, reuseRowObject: true).Take(1);
//
//            // Console.WriteLine("Classifying multiple images");
//            // foreach (var prediction in predictions)
//            // {
//            //     Console.WriteLine
//            //         ($"Image: {Path.GetFileName(prediction.ImagePath)}, Actual Value: {prediction.Label} | Predicted Value: {prediction.PredictedLabel}");
//            // }
//            
            
            

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