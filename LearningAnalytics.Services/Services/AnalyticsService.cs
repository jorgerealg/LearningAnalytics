using System;
using System.Collections.Generic;
using System.Linq;
using LearningAnalytics.Core.Models;

namespace LearningAnalytics.Services.Services
{
    /// <summary>
    /// Implementación de servicio de análisis educativo
    /// </summary>
    public class AnalyticsService : IAnalyticsService
    {
        public AnalyticsResult AnalyzeStudents(List<Student> students)
        {
            if (students == null || students.Count == 0)
                return new AnalyticsResult();

            var result = new AnalyticsResult
            {
                TotalStudents = students.Count,
                AverageGrade = CalculateAverageGrade(students),
                AverageParticipation = CalculateAverageParticipation(students),
                PassingStudents = students.Count(s => s.IsPassing),
                FailingStudents = students.Count(s => !s.IsPassing),
                AtRiskStudents = students.Count(s => s.IsAtRisk),
                ParticipationGradeCorrelation = CalculateParticipationGradeCorrelation(students),
                AtRiskStudentList = IdentifyAtRiskStudents(students),
                Recommendations = GenerateRecommendations(students)
            };

            return result;
        }

        public double CalculateAverageGrade(List<Student> students)
        {
            if (students == null || students.Count == 0)
                return 0;

            return students.Average(s => s.Grade);
        }

        public double CalculateAverageParticipation(List<Student> students)
        {
            if (students == null || students.Count == 0)
                return 0;

            return students.Average(s => s.Participation);
        }

        public List<Student> IdentifyAtRiskStudents(List<Student> students)
        {
            if (students == null)
                return new List<Student>();

            return students.Where(s => s.IsAtRisk).OrderBy(s => s.Grade).ToList();
        }

        public double CalculateParticipationGradeCorrelation(List<Student> students)
        {
            if (students == null || students.Count < 2)
                return 0;

            var grades = students.Select(s => s.Grade).ToList();
            var participations = students.Select(s => s.Participation).ToList();

            return CalculatePearsonCorrelation(grades, participations);
        }

        public List<string> GenerateRecommendations(List<Student> students)
        {
            var recommendations = new List<string>();

            if (students == null || students.Count == 0)
                return recommendations;

            var avgGrade = CalculateAverageGrade(students);
            var avgParticipation = CalculateAverageParticipation(students);
            var atRiskStudents = IdentifyAtRiskStudents(students);
            var correlation = CalculateParticipationGradeCorrelation(students);

            // Recomendaciones generales
            if (avgGrade < 5.0)
            {
                recommendations.Add("ADVERTENCIA: El promedio general de notas es bajo. Se recomienda revisar los métodos de enseñanza.");
            }

            if (avgParticipation < 5.0)
            {
                recommendations.Add("La participación en clase es baja. Considerar estrategias para incrementar la interacción.");
            }

            if (correlation > 0.7)
            {
                recommendations.Add("Existe una fuerte correlación entre participación y notas. Fomentar la participación puede mejorar los resultados.");
            }
            else if (correlation < 0.3)
            {
                recommendations.Add("La correlación entre participación y notas es débil. Investigar otros factores que afectan el rendimiento.");
            }

            // Recomendaciones para estudiantes en riesgo
            if (atRiskStudents.Count > 0)
            {
                var percentageAtRisk = (double)atRiskStudents.Count / students.Count * 100;
                recommendations.Add($"ALERTA: {atRiskStudents.Count} estudiantes ({percentageAtRisk:F1}%) están en riesgo académico.");

                // Top 5 estudiantes en mayor riesgo
                var topRiskStudents = atRiskStudents.Take(5);
                foreach (var student in topRiskStudents)
                {
                    if (student.Grade < 3.0)
                    {
                        recommendations.Add($"CRÍTICO: {student.Name} necesita intervención inmediata (Nota: {student.Grade:F2})");
                    }
                    else if (student.Participation < 3.0)
                    {
                        recommendations.Add($"{student.Name} necesita motivación y apoyo (Nota: {student.Grade:F2}, Participación: {student.Participation:F2})");
                    }
                    else
                    {
                        recommendations.Add($"{student.Name} requiere seguimiento cercano (Nota: {student.Grade:F2})");
                    }
                }
            }

            // Recomendaciones de mejora continua
            if (avgGrade >= 7.0 && avgParticipation >= 7.0)
            {
                recommendations.Add("Excelente rendimiento general. Mantener las estrategias actuales.");
            }

            // Recomendaciones basadas en distribución
            var gradeDistribution = GetGradeDistribution(students);
            if (gradeDistribution["A"] + gradeDistribution["B"] < students.Count * 0.5)
            {
                recommendations.Add("Menos del 50% de los estudiantes obtienen buenas notas. Considerar tutorías adicionales.");
            }

            return recommendations;
        }

        public DescriptiveStatistics GetDescriptiveStatistics(List<Student> students)
        {
            if (students == null || students.Count == 0)
                return new DescriptiveStatistics();

            var grades = students.Select(s => s.Grade).ToList();
            var participations = students.Select(s => s.Participation).ToList();

            return new DescriptiveStatistics
            {
                GradeMean = grades.Average(),
                GradeMedian = CalculateMedian(grades),
                GradeStdDev = CalculateStandardDeviation(grades),
                GradeMin = grades.Min(),
                GradeMax = grades.Max(),
                ParticipationMean = participations.Average(),
                ParticipationMedian = CalculateMedian(participations),
                ParticipationStdDev = CalculateStandardDeviation(participations),
                ParticipationMin = participations.Min(),
                ParticipationMax = participations.Max()
            };
        }

        public StudentSegmentation SegmentStudents(List<Student> students)
        {
            var segmentation = new StudentSegmentation();

            if (students == null)
                return segmentation;

            foreach (var student in students)
            {
                if (student.Grade >= 9.0)
                    segmentation.Excellent.Add(student);
                else if (student.Grade >= 7.5)
                    segmentation.Good.Add(student);
                else if (student.Grade >= 6.0)
                    segmentation.Average.Add(student);
                else if (student.Grade >= 4.0)
                    segmentation.Poor.Add(student);
                else
                    segmentation.Critical.Add(student);
            }

            return segmentation;
        }

        private double CalculatePearsonCorrelation(List<double> x, List<double> y)
        {
            if (x.Count != y.Count || x.Count == 0)
                return 0;

            var n = x.Count;
            var sumX = x.Sum();
            var sumY = y.Sum();
            var sumXy = x.Zip(y, (a, b) => a * b).Sum();
            var sumX2 = x.Sum(a => a * a);
            var sumY2 = y.Sum(b => b * b);

            var numerator = n * sumXy - sumX * sumY;
            var denominator = Math.Sqrt((n * sumX2 - sumX * sumX) * (n * sumY2 - sumY * sumY));

            return denominator == 0 ? 0 : numerator / denominator;
        }

        private double CalculateMedian(List<double> values)
        {
            var sorted = values.OrderBy(x => x).ToList();
            int count = sorted.Count;

            if (count == 0)
                return 0;

            if (count % 2 == 0)
                return (sorted[count / 2 - 1] + sorted[count / 2]) / 2.0;
            else
                return sorted[count / 2];
        }

        private double CalculateStandardDeviation(List<double> values)
        {
            if (values.Count == 0)
                return 0;

            var mean = values.Average();
            var sumOfSquares = values.Sum(x => Math.Pow(x - mean, 2));
            return Math.Sqrt(sumOfSquares / values.Count);
        }

        private Dictionary<string, int> GetGradeDistribution(List<Student> students)
        {
            var distribution = new Dictionary<string, int>
            {
                ["A"] = 0,    // 9.0 - 10.0
                ["B"] = 0,    // 8.0 - 8.9
                ["C"] = 0,    // 7.0 - 7.9
                ["D"] = 0,    // 6.0 - 6.9
                ["F"] = 0     // < 6.0
            };

            foreach (var student in students)
            {
                if (student.Grade >= 9.0)
                    distribution["A"]++;
                else if (student.Grade >= 8.0)
                    distribution["B"]++;
                else if (student.Grade >= 7.0)
                    distribution["C"]++;
                else if (student.Grade >= 6.0)
                    distribution["D"]++;
                else
                    distribution["F"]++;
            }

            return distribution;
        }

        /// <summary>
        /// Genera un reporte detallado en formato texto
        /// </summary>
        public string GenerateDetailedReport(List<Student> students)
        {
            var result = AnalyzeStudents(students);
            var stats = GetDescriptiveStatistics(students);
            var segmentation = SegmentStudents(students);

            var report = new System.Text.StringBuilder();
            report.AppendLine("=== REPORTE DE ANÁLISIS EDUCATIVO ===");
            report.AppendLine($"Fecha: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            report.AppendLine($"Total de estudiantes: {result.TotalStudents}");
            report.AppendLine();

            report.AppendLine("--- ESTADÍSTICAS GENERALES ---");
            report.AppendLine($"Promedio de notas: {result.AverageGrade:F2}");
            report.AppendLine($"Promedio de participación: {result.AverageParticipation:F2}");
            report.AppendLine($"Tasa de aprobación: {result.PassRate:F1}%");
            report.AppendLine($"Tasa de riesgo: {result.AtRiskRate:F1}%");
            report.AppendLine($"Correlación participación-nota: {result.ParticipationGradeCorrelation:F3}");
            report.AppendLine();

            report.AppendLine("--- ESTADÍSTICAS DESCRIPTIVAS ---");
            report.AppendLine($"Notas - Media: {stats.GradeMean:F2}, Mediana: {stats.GradeMedian:F2}, Desv. Est.: {stats.GradeStdDev:F2}");
            report.AppendLine($"Notas - Mínimo: {stats.GradeMin:F2}, Máximo: {stats.GradeMax:F2}");
            report.AppendLine($"Participación - Media: {stats.ParticipationMean:F2}, Mediana: {stats.ParticipationMedian:F2}, Desv. Est.: {stats.ParticipationStdDev:F2}");
            report.AppendLine($"Participación - Mínimo: {stats.ParticipationMin:F2}, Máximo: {stats.ParticipationMax:F2}");
            report.AppendLine();

            report.AppendLine("--- SEGMENTACIÓN POR RENDIMIENTO ---");
            report.AppendLine($"Excelente (≥9.0): {segmentation.Excellent.Count} estudiantes");
            report.AppendLine($"Bueno (7.5-8.9): {segmentation.Good.Count} estudiantes");
            report.AppendLine($"Promedio (6.0-7.4): {segmentation.Average.Count} estudiantes");
            report.AppendLine($"Bajo (4.0-5.9): {segmentation.Poor.Count} estudiantes");
            report.AppendLine($"Crítico (<4.0): {segmentation.Critical.Count} estudiantes");
            report.AppendLine();

            report.AppendLine("--- RECOMENDACIONES ---");
            foreach (var recommendation in result.Recommendations)
            {
                report.AppendLine($"• {recommendation}");
            }

            return report.ToString();
        }
    }
}
