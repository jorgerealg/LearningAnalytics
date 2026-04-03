using System.Collections.Generic;
using System.Threading.Tasks;
using LearningAnalytics.Core.Models;

namespace LearningAnalytics.Services.Services
{
    /// <summary>
    /// Interfaz para servicios de procesamiento de datos
    /// </summary>
    public interface IDataProcessingService
    {
        /// <summary>
        /// Lee estudiantes desde un archivo CSV
        /// </summary>
        /// <param name="filePath">Ruta del archivo CSV</param>
        /// <returns>Lista de estudiantes</returns>
        Task<List<Student>> ReadStudentsFromCsvAsync(string filePath);

        /// <summary>
        /// Escribe estudiantes a un archivo CSV
        /// </summary>
        /// <param name="students">Lista de estudiantes</param>
        /// <param name="filePath">Ruta del archivo CSV</param>
        /// <returns>Tarea asíncrona</returns>
        Task WriteStudentsToCsvAsync(List<Student> students, string filePath);

        /// <summary>
        /// Limpia y valida los datos de estudiantes
        /// </summary>
        /// <param name="students">Lista de estudiantes a limpiar</param>
        /// <returns>Lista de estudiantes limpios</returns>
        List<Student> CleanStudentData(List<Student> students);

        /// <summary>
        /// Anonimiza nombres de estudiantes usando hash
        /// </summary>
        /// <param name="students">Lista de estudiantes</param>
        /// <returns>Lista de estudiantes con nombres anonimizados</returns>
        List<Student> AnonymizeStudentNames(List<Student> students);

        /// <summary>
        /// Genera datos de ejemplo para pruebas
        /// </summary>
        /// <param name="count">Número de estudiantes a generar</param>
        /// <returns>Lista de estudiantes de ejemplo</returns>
        List<Student> GenerateSampleData(int count = 50);

        /// <summary>
        /// Valida el formato de un archivo CSV
        /// </summary>
        /// <param name="filePath">Ruta del archivo CSV</param>
        /// <returns>True si el formato es válido</returns>
        Task<bool> ValidateCsvFormatAsync(string filePath);
    }
}
