﻿
// This file was auto-generated by ML.NET Model Builder. 

using MLModel_ConsoleApp1;

// Create single instance of sample data from first line of dataset for model input
MLModel.ModelInput sampleData = new MLModel.ModelInput()
{
    Data = @"2024-11-01 01:00",
    Temperatura = 19F,
};



Console.WriteLine("Using model to make single prediction -- Comparing actual Consumo with predicted Consumo from sample data...\n\n");


Console.WriteLine($"Data: {@"2024-11-01 01:00"}");
Console.WriteLine($"Temperatura: {19F}");
Console.WriteLine($"Consumo: {1487F}");


// Make a single prediction on the sample data and print results
var predictionResult = MLModel.Predict(sampleData);
Console.WriteLine($"\n\nPredicted Consumo: {predictionResult.Score}\n\n");

Console.WriteLine("=============== End of process, hit any key to finish ===============");
Console.ReadKey();

