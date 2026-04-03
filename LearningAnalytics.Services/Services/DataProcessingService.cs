using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LearningAnalytics.Core.Models;

namespace LearningAnalytics.Services.Services
{
    /// <summary>
    /// Implementación de servicio de procesamiento de datos
    /// </summary>
    public class DataProcessingService : IDataProcessingService
    {
        private readonly string[] _expectedHeaders = { "Name", "Grade", "Participation" };

        public async Task<List<Student>> ReadStudentsFromCsvAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"CSV file not found: {filePath}");

            var students = new List<Student>();

            try
            {
                var lines = await File.ReadAllLinesAsync(filePath);
                
                if (lines.Length == 0)
                    throw new InvalidOperationException("CSV file is empty");

                // Validar encabezados
                var headerLine = lines[0];
                var headers = headerLine.Split(',').Select(h => h.Trim()).ToArray();
                
                if (!ValidateHeaders(headers))
                    throw new InvalidOperationException($"Invalid CSV headers. Expected: {string.Join(", ", _expectedHeaders)}");

                // Procesar líneas de datos
                for (int i = 1; i < lines.Length; i++)
                {
                    var line = lines[i].Trim();
                    if (string.IsNullOrEmpty(line))
                        continue;

                    var values = line.Split(',');
                    if (values.Length < 3)
                        continue;

                    var student = ParseStudentFromCsv(values);
                    if (student != null)
                        students.Add(student);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error reading CSV file: {ex.Message}", ex);
            }

            return students;
        }

        public async Task WriteStudentsToCsvAsync(List<Student> students, string filePath)
        {
            if (students == null || students.Count == 0)
                throw new ArgumentException("Students list cannot be null or empty", nameof(students));

            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var lines = new List<string>();
            
            // Encabezado
            lines.Add("Name,Grade,Participation,CreatedAt");

            // Datos
            foreach (var student in students)
            {
                var line = $"{EscapeCsvField(student.Name)},{student.Grade.ToString(CultureInfo.InvariantCulture)},{student.Participation.ToString(CultureInfo.InvariantCulture)},{student.CreatedAt:O}";
                lines.Add(line);
            }

            await File.WriteAllLinesAsync(filePath, lines);
        }

        public List<Student> CleanStudentData(List<Student> students)
        {
            if (students == null)
                return new List<Student>();

            var cleanedStudents = new List<Student>();

            foreach (var student in students)
            {
                // Validar y limpiar nombre
                var name = student.Name?.Trim();
                if (string.IsNullOrEmpty(name))
                    name = "Unknown Student";

                // Validar y normalizar notas
                var grade = Math.Max(0, Math.Min(10, student.Grade));
                var participation = Math.Max(0, Math.Min(10, student.Participation));

                // Crear estudiante limpio
                var cleanedStudent = new Student
                {
                    Id = student.Id == Guid.Empty ? Guid.NewGuid() : student.Id,
                    Name = name,
                    Grade = grade,
                    Participation = participation,
                    CreatedAt = student.CreatedAt == default ? DateTime.UtcNow : student.CreatedAt
                };

                cleanedStudents.Add(cleanedStudent);
            }

            return cleanedStudents;
        }

        public List<Student> AnonymizeStudentNames(List<Student> students)
        {
            if (students == null)
                return new List<Student>();

            var anonymizedStudents = new List<Student>();

            foreach (var student in students)
            {
                var anonymizedStudent = new Student
                {
                    Id = student.Id,
                    Name = GenerateHash(student.Name),
                    Grade = student.Grade,
                    Participation = student.Participation,
                    CreatedAt = student.CreatedAt
                };

                anonymizedStudents.Add(anonymizedStudent);
            }

            return anonymizedStudents;
        }

        public List<Student> GenerateSampleData(int count = 50)
        {
            var students = new List<Student>();
            var random = new Random();
            var firstNames = new[] { "Juan", "María", "Carlos", "Ana", "Luis", "Sofía", "Pedro", "Laura", "Miguel", "Carmen", "José", "Isabel", "Francisco", "Teresa", "Antonio", "Elena" };
            var lastNames = new[] { "García", "Rodríguez", "Martínez", "López", "González", "Pérez", "Sánchez", "Ramírez", "Torres", "Díaz", "Herrera", "Morales", "Vargas", "Jiménez", "Reyes", "Mendoza" };

            for (int i = 0; i < count; i++)
            {
                var firstName = firstNames[random.Next(firstNames.Length)];
                var lastName = lastNames[random.Next(lastNames.Length)];
                
                // Generar notas con distribución normal simulada
                var grade = Math.Min(10, Math.Max(0, random.NextDouble() * 4 + 4 + (random.NextDouble() - 0.5) * 2));
                var participation = Math.Min(10, Math.Max(0, random.NextDouble() * 5 + 2.5 + (random.NextDouble() - 0.5) * 2));

                var student = new Student
                {
                    Id = Guid.NewGuid(),
                    Name = $"{firstName} {lastName}",
                    Grade = Math.Round(grade, 2),
                    Participation = Math.Round(participation, 2),
                    CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 365))
                };

                students.Add(student);
            }

            return students;
        }

        public async Task<bool> ValidateCsvFormatAsync(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return false;

                var lines = await File.ReadAllLinesAsync(filePath);
                if (lines.Length == 0)
                    return false;

                var headers = lines[0].Split(',').Select(h => h.Trim()).ToArray();
                return ValidateHeaders(headers);
            }
            catch
            {
                return false;
            }
        }

        private bool ValidateHeaders(string[] headers)
        {
            if (headers.Length < 3)
                return false;

            var normalizedHeaders = headers.Select(h => h.ToLowerInvariant().Trim()).ToArray();
            
            return normalizedHeaders.Contains("name") && 
                   normalizedHeaders.Contains("grade") && 
                   normalizedHeaders.Contains("participation");
        }

        private Student? ParseStudentFromCsv(string[] values)
        {
            try
            {
                var name = values[0]?.Trim() ?? string.Empty;
                if (string.IsNullOrEmpty(name))
                    return null;

                if (!double.TryParse(values[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var grade))
                    grade = 0;

                if (!double.TryParse(values[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var participation))
                    participation = 0;

                return new Student
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Grade = grade,
                    Participation = participation,
                    CreatedAt = DateTime.UtcNow
                };
            }
            catch
            {
                return null;
            }
        }

        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
                return string.Empty;

            if (field.Contains(',') || field.Contains('"') || field.Contains('\n') || field.Contains('\r'))
            {
                return $"\"{field.Replace("\"", "\"\"")}\"";
            }

            return field;
        }

        private string GenerateHash(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return $"Student_{Convert.ToHexString(hash).Substring(0, 8)}";
        }

        /// <summary>
        /// Exporta estudiantes a formato JSON
        /// </summary>
        public string ExportToJson(List<Student> students)
        {
            return System.Text.Json.JsonSerializer.Serialize(students, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
            });
        }
    }
}
