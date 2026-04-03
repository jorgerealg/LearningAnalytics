using System.Collections.Generic;
using LearningAnalytics.Core.Models;

namespace LearningAnalytics.Services.Services
{
    /// <summary>
    /// Interfaz para servicios de análisis educativo
    /// </summary>
    public interface IAnalyticsService
    {
        /// <summary>
        /// Realiza un análisis completo de los datos de estudiantes
        /// </summary>
        /// <param name="students">Lista de estudiantes a analizar</param>
        /// <returns>Resultados del análisis</returns>
        AnalyticsResult AnalyzeStudents(List<Student> students);

        /// <summary>
        /// Calcula el promedio de notas
        /// </summary>
        /// <param name="students">Lista de estudiantes</param>
        /// <returns>Promedio de notas</returns>
        double CalculateAverageGrade(List<Student> students);

        /// <summary>
        /// Calcula el promedio de participación
        /// </summary>
        /// <param name="students">Lista de estudiantes</param>
        /// <returns>Promedio de participación</returns>
        double CalculateAverageParticipation(List<Student> students);

        /// <summary>
        /// Identifica estudiantes en riesgo
        /// </summary>
        /// <param name="students">Lista de estudiantes</param>
        /// <returns>Lista de estudiantes en riesgo</returns>
        List<Student> IdentifyAtRiskStudents(List<Student> students);

        /// <summary>
        /// Calcula la correlación entre participación y notas
        /// </summary>
        /// <param name="students">Lista de estudiantes</param>
        /// <returns>Coeficiente de correlación de Pearson</returns>
        double CalculateParticipationGradeCorrelation(List<Student> students);

        /// <summary>
        /// Genera recomendaciones personalizadas
        /// </summary>
        /// <param name="students">Lista de estudiantes</param>
        /// <returns>Lista de recomendaciones</returns>
        List<string> GenerateRecommendations(List<Student> students);

        /// <summary>
        /// Obtiene estadísticas descriptivas detalladas
        /// </summary>
        /// <param name="students">Lista de estudiantes</param>
        /// <returns>Estadísticas descriptivas</returns>
        DescriptiveStatistics GetDescriptiveStatistics(List<Student> students);

        /// <summary>
        /// Segmenta estudiantes por rendimiento
        /// </summary>
        /// <param name="students">Lista de estudiantes</param>
        /// <returns>Segmentación de estudiantes</returns>
        StudentSegmentation SegmentStudents(List<Student> students);
    }

    /// <summary>
    /// Estadísticas descriptivas de los datos
    /// </summary>
    public class DescriptiveStatistics
    {
        public double GradeMean { get; set; }
        public double GradeMedian { get; set; }
        public double GradeStdDev { get; set; }
        public double GradeMin { get; set; }
        public double GradeMax { get; set; }
        public double ParticipationMean { get; set; }
        public double ParticipationMedian { get; set; }
        public double ParticipationStdDev { get; set; }
        public double ParticipationMin { get; set; }
        public double ParticipationMax { get; set; }
    }

    /// <summary>
    /// Segmentación de estudiantes por rendimiento
    /// </summary>
    public class StudentSegmentation
    {
        public List<Student> Excellent { get; set; } = new();
        public List<Student> Good { get; set; } = new();
        public List<Student> Average { get; set; } = new();
        public List<Student> Poor { get; set; } = new();
        public List<Student> Critical { get; set; } = new();
    }
}
