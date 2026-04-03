using System.Collections.Generic;
using LearningAnalytics.Core.Models;

namespace LearningAnalytics.Services.Services
{
    /// <summary>
    /// Interfaz para servicios de Machine Learning
    /// </summary>
    public interface IMachineLearningService
    {
        /// <summary>
        /// Entrena un modelo de clasificación binaria (aprueba/reprueba)
        /// </summary>
        /// <param name="students">Datos de entrenamiento</param>
        /// <returns>Resultado del entrenamiento</returns>
        MLTrainingResult TrainPassingModel(List<Student> students);

        /// <summary>
        /// Predice si un estudiante aprobará basado en su participación
        /// </summary>
        /// <param name="participation">Nivel de participación del estudiante</param>
        /// <returns>True si se predice que aprobará</returns>
        bool PredictPassing(double participation);

        /// <summary>
        /// Predice la nota de un estudiante basado en su participación
        /// </summary>
        /// <param name="participation">Nivel de participación del estudiante</param>
        /// <returns>Nota predicha</returns>
        double PredictGrade(double participation);

        /// <summary>
        /// Evalúa la precisión del modelo
        /// </summary>
        /// <param name="testStudents">Datos de prueba</param>
        /// <return>Métricas de evaluación</returns>
        MLEvaluationMetrics EvaluateModel(List<Student> testStudents);

        /// <summary>
        /// Genera predicciones para múltiples estudiantes
        /// </summary>
        /// <param name="students">Estudiantes para predecir</param>
        /// <return>Lista de predicciones</returns>
        List<StudentPrediction> PredictForStudents(List<Student> students);

        /// <summary>
        /// Guarda el modelo entrenado en un archivo
        /// </summary>
        /// <param name="filePath">Ruta del archivo del modelo</param>
        void SaveModel(string filePath);

        /// <summary>
        /// Carga un modelo entrenado desde un archivo
        /// </summary>
        /// <param name="filePath">Ruta del archivo del modelo</param>
        void LoadModel(string filePath);
    }

    /// <summary>
    /// Resultado del entrenamiento del modelo
    /// </summary>
    public class MLTrainingResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public double TrainingAccuracy { get; set; }
        public int TrainingSamples { get; set; }
        public string ModelInfo { get; set; } = string.Empty;
    }

    /// <summary>
    /// Métricas de evaluación del modelo
    /// </summary>
    public class MLEvaluationMetrics
    {
        public double Accuracy { get; set; }
        public double Precision { get; set; }
        public double Recall { get; set; }
        public double F1Score { get; set; }
        public int TruePositives { get; set; }
        public int FalsePositives { get; set; }
        public int TrueNegatives { get; set; }
        public int FalseNegatives { get; set; }
    }

    /// <summary>
    /// Predicción para un estudiante
    /// </summary>
    public class StudentPrediction
    {
        public Student Student { get; set; } = null!;
        public bool PredictedPassing { get; set; }
        public double PredictedGrade { get; set; }
        public double Confidence { get; set; }
        public bool IsCorrect { get; set; }
    }
}
