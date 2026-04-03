namespace LearningAnalytics.Core.Models
{
    /// <summary>
    /// Representa un estudiante con datos sensibles encriptados
    /// </summary>
    public class EncryptedStudent
    {
        /// <summary>
        /// Identificador único del estudiante (no encriptado)
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nombre del estudiante encriptado en base64
        /// </summary>
        public string EncryptedName { get; set; } = string.Empty;

        /// <summary>
        /// Nota final del estudiante (no encriptada, dato no sensible)
        /// </summary>
        public double Grade { get; set; }

        /// <summary>
        /// Nivel de participación en clase (no encriptado, dato no sensible)
        /// </summary>
        public double Participation { get; set; }

        /// <summary>
        /// Vector de inicialización usado para la encriptación
        /// </summary>
        public string IV { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de registro del estudiante
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Determina si el estudiante aprueba (nota >= 5)
        /// </summary>
        public bool IsPassing => Grade >= 5.0;

        /// <summary>
        /// Determina si el estudiante está en riesgo
        /// </summary>
        public bool IsAtRisk => Grade < 5.0 || Participation < 3.0;

        public override string ToString()
        {
            return $"EncryptedStudent ID: {Id} | Grade: {Grade:F2} | Participation: {Participation:F2} | Status: {(IsPassing ? "Passing" : "Failing")}";
        }
    }
}
