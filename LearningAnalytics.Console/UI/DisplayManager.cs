using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LearningAnalytics.Core.Models;
using LearningAnalytics.Services.Services;

namespace LearningAnalytics.Console.UI
{
    /// <summary>
    /// Clase estática para manejar la visualización en consola
    /// </summary>
    public static class DisplayManager
    {
        private const int ConsoleWidth = 80;

        public static void ShowHeader()
        {
            System.Console.Clear();
            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════════════╗");
            System.Console.WriteLine("║                    LEARNING ANALYTICS SYSTEM                         ║");
            System.Console.WriteLine("║           Análisis Educativo con Criptografía y Blockchain                   ║");
            System.Console.WriteLine("║                     .NET 8 - Academic Project                              ║");
            System.Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════════════╝");
            System.Console.ResetColor();
            System.Console.WriteLine();
        }

        public static void ShowMainMenu()
        {
            ShowHeader();
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine("MENÚ PRINCIPAL");
            System.Console.ResetColor();
            System.Console.WriteLine();

            System.Console.WriteLine("1. Ejecutar análisis completo");
            System.Console.WriteLine("2. Cargar y analizar CSV");
            System.Console.WriteLine("3. Generar datos de ejemplo");
            System.Console.WriteLine("4. Probar criptografía");
            System.Console.WriteLine("5. Probar blockchain");
            System.Console.WriteLine("6. Probar Machine Learning");
            System.Console.WriteLine("7. Ver información de blockchain");
            System.Console.WriteLine("8. Exportar datos");
            System.Console.WriteLine("9. Salir");
            System.Console.WriteLine();
        }

        public static void ShowSectionHeader(string title)
        {
            System.Console.WriteLine();
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine($"═{new string('=', ConsoleWidth - 2)}═");
            System.Console.WriteLine($" {title.PadRight(ConsoleWidth - 4)} ");
            System.Console.WriteLine($"═{new string('=', ConsoleWidth - 2)}═");
            System.Console.ResetColor();
            System.Console.WriteLine();
        }

        public static string GetUserInput(string prompt)
        {
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.Write($"{prompt}: ");
            System.Console.ResetColor();
            return System.Console.ReadLine() ?? string.Empty;
        }

        public static void ShowInfo(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine($"   {message}");
            System.Console.ResetColor();
        }

        public static void ShowSuccess(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine($"{message}");
            System.Console.ResetColor();
        }

        public static void ShowError(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"{message}");
            System.Console.ResetColor();
        }

        public static void ShowWarning(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine($"{message}");
            System.Console.ResetColor();
        }

        public static void ShowAnalysisResults(AnalyticsResult result, MLTrainingResult? mlResult = null)
        {
            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine("RESULTADOS DEL ANÁLISIS EDUCATIVO");
            System.Console.ResetColor();
            System.Console.WriteLine();

            // Estadísticas generales
            System.Console.WriteLine("ESTADÍSTICAS GENERALES:");
            System.Console.WriteLine($"   • Total estudiantes: {result.TotalStudents}");
            System.Console.WriteLine($"   • Promedio notas: {result.AverageGrade:F2}");
            System.Console.WriteLine($"   • Promedio participación: {result.AverageParticipation:F2}");
            System.Console.WriteLine($"   • Tasa aprobación: {result.PassRate:F1}%");
            System.Console.WriteLine($"   • Tasa riesgo: {result.AtRiskRate:F1}%");
            System.Console.WriteLine($"   • Correlación participación-nota: {result.ParticipationGradeCorrelation:F3}");
            System.Console.WriteLine();

            // Distribución
            System.Console.WriteLine("DISTRIBUCIÓN:");
            System.Console.WriteLine($"   • Aprobados: {result.PassingStudents} estudiantes");
            System.Console.WriteLine($"   • Suspendos: {result.FailingStudents} estudiantes");
            System.Console.WriteLine($"   • En riesgo: {result.AtRiskStudents} estudiantes");
            System.Console.WriteLine();

            // Recomendaciones
            if (result.Recommendations.Any())
            {
                System.Console.ForegroundColor = ConsoleColor.Yellow;
                System.Console.WriteLine("RECOMENDACIONES:");
                System.Console.ResetColor();
                foreach (var recommendation in result.Recommendations.Take(10))
                {
                    System.Console.WriteLine($"   • {recommendation}");
                }
                System.Console.WriteLine();
            }

            // Resultados ML
            if (mlResult != null)
            {
                System.Console.ForegroundColor = ConsoleColor.Magenta;
                System.Console.WriteLine("MACHINE LEARNING:");
                System.Console.ResetColor();
                if (mlResult.Success)
                {
                    System.Console.WriteLine($"   • Estado: Entrenado exitosamente");
                    System.Console.WriteLine($"   • Precisión: {mlResult.TrainingAccuracy:F3}");
                    System.Console.WriteLine($"   • Muestras: {mlResult.TrainingSamples}");
                    System.Console.WriteLine($"   • Info: {mlResult.ModelInfo}");
                }
                else
                {
                    System.Console.WriteLine($"   • Estado: Error");
                    System.Console.WriteLine($"   • Mensaje: {mlResult.Message}");
                }
                System.Console.WriteLine();
            }
        }

        public static void ShowStudentTable(List<Student> students)
        {
            if (!students.Any())
            {
                ShowWarning("No hay estudiantes para mostrar");
                return;
            }

            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine("LISTA DE ESTUDIANTES");
            System.Console.ResetColor();
            System.Console.WriteLine();

            // Encabezado de tabla
            System.Console.WriteLine("┌─────────────────────────────────────┬────────────┬──────────────┬───────────┐");
            System.Console.WriteLine("│ Nombre                             │ Nota       │ Participación │ Estado    │");
            System.Console.WriteLine("├─────────────────────────────────────┼────────────┼──────────────┼───────────┤");

            // Datos
            foreach (var student in students.Take(20))
            {
                var name = (student.Name.Length > 35) ? student.Name.Substring(0, 32) + "..." : student.Name;
                var status = student.IsPassing ? "Aprobado" : "Suspenso";
                var statusColor = student.IsPassing ? ConsoleColor.Green : ConsoleColor.Red;

                System.Console.Write($"│ {name.PadRight(35)} │ ");
                System.Console.Write($"{student.Grade,8:F2} │ ");
                System.Console.Write($"{student.Participation,10:F2} │ ");
                
                System.Console.ForegroundColor = statusColor;
                System.Console.Write($"{status,9}");
                System.Console.ResetColor();
                
                System.Console.WriteLine(" │");
            }

            System.Console.WriteLine("└─────────────────────────────────────┴────────────┴──────────────┴───────────┘");

            if (students.Count > 20)
            {
                ShowInfo($"Mostrando primeros 20 de {students.Count} estudiantes");
            }
        }

        public static void ShowPredictions(List<StudentPrediction> predictions)
        {
            if (!predictions.Any())
            {
                ShowWarning("No hay predicciones para mostrar");
                return;
            }

            System.Console.ForegroundColor = ConsoleColor.Magenta;
            System.Console.WriteLine("PREDICCIONES DE MACHINE LEARNING");
            System.Console.ResetColor();
            System.Console.WriteLine();

            System.Console.WriteLine("┌─────────────────────────────────────┬─────────────┬─────────────┬─────────────┬───────────┐");
            System.Console.WriteLine("│ Nombre                             │ Real        │ Predicho    │ Confianza   │ Correcto  │");
            System.Console.WriteLine("├─────────────────────────────────────┼─────────────┼─────────────┼─────────────┼───────────┤");

            foreach (var prediction in predictions)
            {
                var name = (prediction.Student.Name.Length > 35) ? prediction.Student.Name.Substring(0, 32) + "..." : prediction.Student.Name;
                var realStatus = prediction.Student.IsPassing ? "Aprueba" : "Reprueba";
                var predictedStatus = prediction.PredictedPassing ? "Aprueba" : "Reprueba";
                var correctColor = prediction.IsCorrect ? ConsoleColor.Green : ConsoleColor.Red;
                var correctText = prediction.IsCorrect ? "OK" : "ERROR";

                System.Console.Write($"│ {name.PadRight(35)} │ ");
                System.Console.Write($"{realStatus,11} │ ");
                System.Console.Write($"{predictedStatus,11} │ ");
                System.Console.Write($"{prediction.Confidence,9:F2} │ ");
                
                System.Console.ForegroundColor = correctColor;
                System.Console.Write($"{correctText,9}");
                System.Console.ResetColor();
                
                System.Console.WriteLine(" │");
            }

            System.Console.WriteLine("└─────────────────────────────────────┴─────────────┴─────────────┴─────────────┴───────────┘");
        }

        public static void ShowMLMetrics(MLEvaluationMetrics metrics)
        {
            System.Console.ForegroundColor = ConsoleColor.Magenta;
            System.Console.WriteLine("MÉTRICAS DE EVALUACIÓN DEL MODELO");
            System.Console.ResetColor();
            System.Console.WriteLine();

            System.Console.WriteLine($"   • Precisión (Accuracy): {metrics.Accuracy:F3}");
            System.Console.WriteLine($"   • Precisión (Precision): {metrics.Precision:F3}");
            System.Console.WriteLine($"   • Recall: {metrics.Recall:F3}");
            System.Console.WriteLine($"   • F1-Score: {metrics.F1Score:F3}");
            System.Console.WriteLine();

            System.Console.WriteLine("MATRIZ DE CONFUSIÓN:");
            System.Console.WriteLine($"   • Verdaderos Positivos: {metrics.TruePositives}");
            System.Console.WriteLine($"   • Verdaderos Negativos: {metrics.TrueNegatives}");
            System.Console.WriteLine($"   • Falsos Positivos: {metrics.FalsePositives}");
            System.Console.WriteLine($"   • Falsos Negativos: {metrics.FalseNegatives}");
            System.Console.WriteLine();
        }

        public static void ShowBlockchainStats(BlockchainStats stats)
        {
            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine("ESTADÍSTICAS DE BLOCKCHAIN");
            System.Console.ResetColor();
            System.Console.WriteLine();

            System.Console.WriteLine($"   • Total bloques: {stats.TotalBlocks}");
            System.Console.WriteLine($"   • Entradas de datos: {stats.TotalDataEntries}");
            System.Console.WriteLine($"   • Primer bloque: {stats.FirstBlockTimestamp:yyyy-MM-dd HH:mm:ss}");
            System.Console.WriteLine($"   • Último bloque: {stats.LastBlockTimestamp:yyyy-MM-dd HH:mm:ss}");
            System.Console.WriteLine($"   • Integridad: {(stats.IsValid ? "Válida" : "Inválida")}");
            System.Console.WriteLine();
        }

        public static void ShowBlockchainChain(List<Block> chain)
        {
            if (!chain.Any())
            {
                ShowWarning("No hay bloques en la cadena");
                return;
            }

            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine("CADENA DE BLOQUES");
            System.Console.ResetColor();
            System.Console.WriteLine();

            System.Console.WriteLine("┌──────┬─────────────────────┬─────────────────────────────────────────────────────────┬─────────────────┐");
            System.Console.WriteLine("│ #    │ Timestamp          │ Hash                                                │ Previous Hash   │");
            System.Console.WriteLine("├──────┼─────────────────────┼─────────────────────────────────────────────────────────┼─────────────────┤");

            foreach (var block in chain.Take(10))
            {
                var hash = block.Hash.Length > 50 ? block.Hash.Substring(0, 47) + "..." : block.Hash;
                var prevHash = block.PreviousHash.Length > 15 ? block.PreviousHash.Substring(0, 12) + "..." : block.PreviousHash;

                System.Console.WriteLine($"│ {block.Index,4} │ {block.Timestamp:yyyy-MM-dd HH:mm} │ {hash,-50} │ {prevHash,-15} │");
            }

            System.Console.WriteLine("└──────┴─────────────────────┴─────────────────────────────────────────────────────────┴─────────────────┘");

            if (chain.Count > 10)
            {
                ShowInfo($"Mostrando primeros 10 de {chain.Count} bloques");
            }
        }

        public static void ShowProgressBar(string title, int current, int total)
        {
            var progress = (double)current / total;
            var progressBarWidth = 40;
            var filledWidth = (int)(progress * progressBarWidth);
            var emptyWidth = progressBarWidth - filledWidth;

            System.Console.Write($"\r{title}: [");
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.Write(new string('█', filledWidth));
            System.Console.ResetColor();
            System.Console.Write(new string('░', emptyWidth));
            System.Console.Write($"] {current}/{total} ({progress * 100:F1}%)");

            if (current == total)
            {
                System.Console.WriteLine();
            }
        }

        public static void PressEnterToContinue()
        {
            System.Console.WriteLine();
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.Write("Presione Enter para continuar...");
            System.Console.ResetColor();
            System.Console.ReadLine();
        }
    }
}
