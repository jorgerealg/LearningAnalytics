using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using LearningAnalytics.Core.Models;

namespace LearningAnalytics.Services.Services
{
    /// <summary>
    /// Implementación de servicio de Machine Learning usando ML.NET
    /// </summary>
    public class MachineLearningService : IMachineLearningService
    {
        private readonly MLContext _mlContext;
        private ITransformer? _passingModel;
        private ITransformer? _gradeModel;
        private PredictionEngine<StudentData, PassingPrediction>? _passingPredictionEngine;
        private PredictionEngine<StudentData, GradePrediction>? _gradePredictionEngine;
        private bool _isModelTrained;

        public MachineLearningService()
        {
            _mlContext = new MLContext(seed: 0);
        }

        public MLTrainingResult TrainPassingModel(List<Student> students)
        {
            try
            {
                if (students == null || students.Count < 10)
                {
                    return new MLTrainingResult
                    {
                        Success = false,
                        Message = "Se necesitan al menos 10 estudiantes para entrenar el modelo"
                    };
                }

                // Convertir estudiantes a datos de entrenamiento
                var trainingData = students.Select(s => new StudentData
                {
                    Participation = (float)s.Participation,
                    Grade = (float)s.Grade,
                    IsPassing = s.IsPassing
                }).ToList();

                var dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

                // Dividir datos para entrenamiento y validación
                var trainTestData = _mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

                // Pipeline para clasificación binaria
                var pipeline = _mlContext.Transforms.Concatenate("Features", nameof(StudentData.Participation))
                    .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                        labelColumnName: nameof(StudentData.IsPassing),
                        featureColumnName: "Features"));

                // Entrenar modelo
                _passingModel = pipeline.Fit(trainTestData.TrainSet);

                // Evaluar modelo
                var predictions = _passingModel.Transform(trainTestData.TestSet);
                var metrics = _mlContext.BinaryClassification.Evaluate(predictions, labelColumnName: nameof(StudentData.IsPassing));

                // Crear motor de predicción
                _passingPredictionEngine = _mlContext.Model.CreatePredictionEngine<StudentData, PassingPrediction>(_passingModel);

                // Entrenar modelo de regresión para predecir notas
                TrainGradeModel(trainingData);

                _isModelTrained = true;

                return new MLTrainingResult
                {
                    Success = true,
                    Message = "Modelo entrenado exitosamente",
                    TrainingAccuracy = metrics.Accuracy,
                    TrainingSamples = trainingData.Count,
                    ModelInfo = $"Accuracy: {metrics.Accuracy:F3}, AUC: {metrics.AreaUnderRocCurve:F3}"
                };
            }
            catch (Exception ex)
            {
                return new MLTrainingResult
                {
                    Success = false,
                    Message = $"Error entrenando modelo: {ex.Message}"
                };
            }
        }

        public bool PredictPassing(double participation)
        {
            if (!_isModelTrained || _passingPredictionEngine == null)
                return participation >= 5.0; // Predicción simple si no hay modelo

            var inputData = new StudentData { Participation = (float)participation };
            var prediction = _passingPredictionEngine.Predict(inputData);
            return prediction.PredictedLabel;
        }

        public double PredictGrade(double participation)
        {
            if (!_isModelTrained || _gradePredictionEngine == null)
                return Math.Min(10, participation * 1.2); // Predicción simple si no hay modelo

            var inputData = new StudentData { Participation = (float)participation };
            var prediction = _gradePredictionEngine.Predict(inputData);
            return Math.Max(0, Math.Min(10, prediction.PredictedGrade));
        }

        public MLEvaluationMetrics EvaluateModel(List<Student> testStudents)
        {
            if (!_isModelTrained || _passingModel == null)
                return new MLEvaluationMetrics { Accuracy = 0 };

            try
            {
                var testData = testStudents.Select(s => new StudentData
                {
                    Participation = (float)s.Participation,
                    Grade = (float)s.Grade,
                    IsPassing = s.IsPassing
                }).ToList();

                var dataView = _mlContext.Data.LoadFromEnumerable(testData);
                var predictions = _passingModel.Transform(dataView);
                var metrics = _mlContext.BinaryClassification.Evaluate(predictions, labelColumnName: nameof(StudentData.IsPassing));

                // Calcular métricas detalladas
                var predictionResults = _mlContext.Data.CreateEnumerable<PredictionResult>(predictions, reuseRowObject: false).ToList();
                var tp = predictionResults.Count(p => p.Label && p.Prediction);
                var tn = predictionResults.Count(p => !p.Label && !p.Prediction);
                var fp = predictionResults.Count(p => !p.Label && p.Prediction);
                var fn = predictionResults.Count(p => p.Label && !p.Prediction);

                var precision = tp + fp > 0 ? (double)tp / (tp + fp) : 0;
                var recall = tp + fn > 0 ? (double)tp / (tp + fn) : 0;
                var f1Score = precision + recall > 0 ? 2 * precision * recall / (precision + recall) : 0;

                return new MLEvaluationMetrics
                {
                    Accuracy = metrics.Accuracy,
                    Precision = precision,
                    Recall = recall,
                    F1Score = f1Score,
                    TruePositives = tp,
                    TrueNegatives = tn,
                    FalsePositives = fp,
                    FalseNegatives = fn
                };
            }
            catch
            {
                return new MLEvaluationMetrics { Accuracy = 0 };
            }
        }

        public List<StudentPrediction> PredictForStudents(List<Student> students)
        {
            if (!_isModelTrained)
                return students.Select(s => new StudentPrediction
                {
                    Student = s,
                    PredictedPassing = s.IsPassing,
                    PredictedGrade = s.Grade,
                    Confidence = 0.5,
                    IsCorrect = true
                }).ToList();

            var predictions = new List<StudentPrediction>();

            foreach (var student in students)
            {
                var predictedPassing = PredictPassing(student.Participation);
                var predictedGrade = PredictGrade(student.Participation);
                var confidence = CalculateConfidence(student.Participation);

                predictions.Add(new StudentPrediction
                {
                    Student = student,
                    PredictedPassing = predictedPassing,
                    PredictedGrade = predictedGrade,
                    Confidence = confidence,
                    IsCorrect = predictedPassing == student.IsPassing
                });
            }

            return predictions;
        }

        public void SaveModel(string filePath)
        {
            if (!_isModelTrained || _passingModel == null)
                throw new InvalidOperationException("No hay modelo entrenado para guardar");

            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var dataView = _mlContext.Data.LoadFromEnumerable(new List<StudentData>());
            _mlContext.Model.Save(_passingModel, dataView.Schema, filePath);
        }

        public void LoadModel(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Archivo de modelo no encontrado: {filePath}");

            _passingModel = _mlContext.Model.Load(filePath, out var inputSchema);
            _passingPredictionEngine = _mlContext.Model.CreatePredictionEngine<StudentData, PassingPrediction>(_passingModel);
            _isModelTrained = true;
        }

        private void TrainGradeModel(List<StudentData> trainingData)
        {
            var dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

            var pipeline = _mlContext.Transforms.Concatenate("Features", nameof(StudentData.Participation))
                .Append(_mlContext.Regression.Trainers.Sdca(
                    labelColumnName: nameof(StudentData.Grade),
                    featureColumnName: "Features"));

            _gradeModel = pipeline.Fit(dataView);
            _gradePredictionEngine = _mlContext.Model.CreatePredictionEngine<StudentData, GradePrediction>(_gradeModel);
        }

        private double CalculateConfidence(double participation)
        {
            // Simulación de confianza basada en la distancia de los extremos
            if (participation <= 2.0 || participation >= 8.0)
                return 0.9;
            else if (participation <= 3.5 || participation >= 6.5)
                return 0.7;
            else
                return 0.5;
        }

        /// <summary>
        /// Clase para datos de entrenamiento
        /// </summary>
        private class StudentData
        {
            public float Participation { get; set; }
            public float Grade { get; set; }
            public bool IsPassing { get; set; }
        }

        /// <summary>
        /// Clase para predicción de aprobación
        /// </summary>
        private class PassingPrediction
        {
            [ColumnName("PredictedLabel")]
            public bool PredictedLabel { get; set; }

            public float Probability { get; set; }
        }

        /// <summary>
        /// Clase para predicción de nota
        /// </summary>
        private class GradePrediction
        {
            [ColumnName("Score")]
            public float PredictedGrade { get; set; }
        }

        /// <summary>
        /// Clase para resultados de evaluación
        /// </summary>
        private class PredictionResult
        {
            public bool Label { get; set; }
            public bool Prediction { get; set; }
        }
    }
}
