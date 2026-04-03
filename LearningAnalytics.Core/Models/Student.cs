using System;

namespace LearningAnalytics.Core.Models
{
    /// <summary>
    /// Representa un estudiante con datos académicos básicos
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Identificador único del estudiante
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nombre completo del estudiante
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Nota final del estudiante (escala 0-10)
        /// </summary>
        public double Grade { get; set; }

        /// <summary>
        /// Nivel de participación en clase (escala 0-10)
        /// </summary>
        public double Participation { get; set; }

        /// <summary>
        /// Fecha de registro del estudiante
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Determina si el estudiante aprueba (nota >= 5)
        /// </summary>
        public bool IsPassing => Grade >= 5.0;

        /// <summary>
        /// Determina si el estudiante está en riesgo (nota < 5 o participación < 3)
        /// </summary>
        public bool IsAtRisk => Grade < 5.0 || Participation < 3.0;

        public override string ToString()
        {
            return $"Student: {Name} | Grade: {Grade:F2} | Participation: {Participation:F2} | Status: {(IsPassing ? "Passing" : "Failing")}";
        }
    }
}
