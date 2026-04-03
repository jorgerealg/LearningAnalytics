using System.Collections.Generic;

namespace LearningAnalytics.Core.Models
{
    /// <summary>
    /// Resultados del análisis de datos educativos
    /// </summary>
    public class AnalyticsResult
    {
        /// <summary>
        /// Total de estudiantes analizados
        /// </summary>
        public int TotalStudents { get; set; }

        /// <summary>
        /// Promedio de notas general
        /// </summary>
        public double AverageGrade { get; set; }

        /// <summary>
        /// Promedio de participación general
        /// </summary>
        public double AverageParticipation { get; set; }

        /// <summary>
        /// Cantidad de estudiantes que aprueban
        /// </summary>
        public int PassingStudents { get; set; }

        /// <summary>
        /// Cantidad de estudiantes que reprueban
        /// </summary>
        public int FailingStudents { get; set; }

        /// <summary>
        /// Cantidad de estudiantes en riesgo
        /// </summary>
        public int AtRiskStudents { get; set; }

        /// <summary>
        /// Correlación entre participación y notas
        /// </summary>
        public double ParticipationGradeCorrelation { get; set; }

        /// <summary>
        /// Lista de estudiantes en riesgo
        /// </summary>
        public List<Student> AtRiskStudentList { get; set; } = new();

        /// <summary>
        /// Lista de recomendaciones generadas
        /// </summary>
        public List<string> Recommendations { get; set; } = new();

        /// <summary>
        /// Tasa de aprobación en porcentaje
        /// </summary>
        public double PassRate => TotalStudents > 0 ? (double)PassingStudents / TotalStudents * 100 : 0;

        /// <summary>
        /// Tasa de riesgo en porcentaje
        /// </summary>
        public double AtRiskRate => TotalStudents > 0 ? (double)AtRiskStudents / TotalStudents * 100 : 0;

        public override string ToString()
        {
            return $"Analytics Result: {TotalStudents} students | Avg Grade: {AverageGrade:F2} | Pass Rate: {PassRate:F1}% | At Risk: {AtRiskRate:F1}%";
        }
    }
}
