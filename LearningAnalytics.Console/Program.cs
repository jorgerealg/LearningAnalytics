using LearningAnalytics.Console.UI;
using LearningAnalytics.Core.Models;
using LearningAnalytics.Infrastructure.Services;
using LearningAnalytics.Services.Services;
using System.Text.Json;

namespace LearningAnalytics.Console
{
    class Program
    {
        private static ICryptoService _cryptoService = null!;
        private static IBlockchainService _blockchainService = null!;
        private static IDataProcessingService _dataProcessingService = null!;
        private static IAnalyticsService _analyticsService = null!;
        private static IMachineLearningService _machineLearningService = null!;

        static async Task Main(string[] args)
        {
            try
            {
                DisplayManager.ShowHeader();

                // Inicializar servicios
                InitializeServices();

                // Mostrar menú principal
                await ShowMainMenu();
            }
            catch (Exception ex)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine($"Error fatal: {ex.Message}");
                System.Console.ResetColor();
            }
        }

        private static void InitializeServices()
        {
            try
            {
                System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.WriteLine("Inicializando servicios...");
                System.Console.ResetColor();

                _cryptoService = new CryptoService();
                _blockchainService = new BlockchainService();
                _dataProcessingService = new DataProcessingService();
                _analyticsService = new AnalyticsService();
                _machineLearningService = new MachineLearningService();

                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine("[OK] Servicios inicializados correctamente");
                System.Console.ResetColor();
            }
            catch (Exception ex)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine($"Error inicializando servicios: {ex.Message}");
                System.Console.ResetColor();
                throw;
            }
        }

        private static async Task ShowMainMenu()
        {
            while (true)
            {
                try
                {
                    // Mostrar menú simple sin DisplayManager
                    System.Console.Clear();
                    System.Console.ForegroundColor = ConsoleColor.Cyan;
                    System.Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════════════╗");
                    System.Console.WriteLine("║                    LEARNING ANALYTICS SYSTEM                                           ║");
                    System.Console.WriteLine("║           Análisis Educativo con Criptografía y Blockchain                             ║");
                    System.Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════════════╝");
                    System.Console.ResetColor();
                    System.Console.WriteLine();

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

                    System.Console.ForegroundColor = ConsoleColor.White;
                    System.Console.Write("Seleccione una opción: ");
                    System.Console.ResetColor();
                    var choice = System.Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            await RunCompleteAnalysis();
                            break;
                        case "2":
                            await LoadAndAnalyzeCsv();
                            break;
                        case "3":
                            await GenerateSampleData();
                            break;
                        case "4":
                            TestCryptography();
                            break;
                        case "5":
                            TestBlockchain();
                            break;
                        case "6":
                            await TestMachineLearning();
                            break;
                        case "7":
                            ShowBlockchainInfo();
                            break;
                        case "8":
                            ExportData();
                            break;
                        case "9":
                            System.Console.ForegroundColor = ConsoleColor.Green;
                            System.Console.WriteLine("¡Hasta luego!");
                            System.Console.ResetColor();
                            return;
                        default:
                            System.Console.ForegroundColor = ConsoleColor.Red;
                            System.Console.WriteLine("Opción no válida. Intente nuevamente.");
                            System.Console.ResetColor();
                            break;
                    }

                    if (choice != "9")
                    {
                        System.Console.ForegroundColor = ConsoleColor.White;
                        System.Console.Write("\nPresione Enter para continuar...");
                        System.Console.ResetColor();
                        System.Console.ReadLine();
                    }
                }
                catch (Exception ex)
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine($"Error en menú: {ex.Message}");
                    System.Console.ResetColor();
                }
            }
        }

        private static async Task RunCompleteAnalysis()
        {
            try
            {
                System.Console.Clear();
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine("ANÁLISIS COMPLETO DE LEARNING ANALYTICS");
                System.Console.ResetColor();
                System.Console.WriteLine();

                // Paso 1: Generar o cargar datos
                System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.WriteLine("Paso 1: Generando datos de ejemplo...");
                System.Console.ResetColor();
                var students = _dataProcessingService.GenerateSampleData(50);
                var cleanedStudents = _dataProcessingService.CleanStudentData(students);
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine($"[OK] Se generaron {cleanedStudents.Count} estudiantes");
                System.Console.ResetColor();

                // Paso 2: Encriptar datos sensibles
                System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.WriteLine("Paso 2: Encriptando datos sensibles...");
                System.Console.ResetColor();
                var encryptedStudents = await EncryptStudentData(cleanedStudents);
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine($"[OK] Se encriptaron {encryptedStudents.Count} registros");
                System.Console.ResetColor();

                // Paso 3: Almacenar en blockchain
                System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.WriteLine("Paso 3: Almacenando datos en blockchain...");
                System.Console.ResetColor();
                var blocks = await StoreDataInBlockchain(cleanedStudents);
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine($"[OK] Se almacenaron {blocks.Count} bloques");
                System.Console.ResetColor();

                // Paso 4: Validar blockchain
                System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.WriteLine("Paso 4: Validando integridad de blockchain...");
                System.Console.ResetColor();
                var isValid = _blockchainService.ValidateChain();
                if (isValid)
                {
                    System.Console.ForegroundColor = ConsoleColor.Green;
                    System.Console.WriteLine("[OK] Blockchain válida");
                }
                else
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine("Blockchain inválida");
                }
                System.Console.ResetColor();

                // Paso 5: Ejecutar análisis
                System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.WriteLine("Paso 5: Ejecutando análisis educativo...");
                System.Console.ResetColor();
                var analysisResult = _analyticsService.AnalyzeStudents(cleanedStudents);

                // Paso 6: Entrenar modelo ML
                System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.WriteLine("Paso 6: Entrenando modelo de Machine Learning...");
                System.Console.ResetColor();
                var mlResult = _machineLearningService.TrainPassingModel(cleanedStudents);

                // Paso 7: Mostrar resultados
                System.Console.ForegroundColor = ConsoleColor.Cyan;
                System.Console.WriteLine("RESULTADOS DEL ANÁLISIS");
                System.Console.ResetColor();
                System.Console.WriteLine();
                
                System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.WriteLine("ESTADÍSTICAS GENERALES:");
                System.Console.ResetColor();
                System.Console.WriteLine($"   • Total estudiantes: {analysisResult.TotalStudents}");
                System.Console.WriteLine($"   • Promedio notas: {analysisResult.AverageGrade:F2}");
                System.Console.WriteLine($"   • Promedio participación: {analysisResult.AverageParticipation:F2}");
                System.Console.WriteLine($"   • Tasa aprobación: {analysisResult.PassRate:F1}%");
                System.Console.WriteLine($"   • Tasa riesgo: {analysisResult.AtRiskRate:F1}%");
                System.Console.WriteLine($"   • Correlación participación-nota: {analysisResult.ParticipationGradeCorrelation:F3}");
                System.Console.WriteLine();

                System.Console.ForegroundColor = ConsoleColor.White;
                         System.Console.WriteLine("ESTADÍSTICAS GENERALES:");
                if (mlResult.Success)
                {
                    System.Console.ForegroundColor = ConsoleColor.Magenta;
                    System.Console.WriteLine("MACHINE LEARNING:");
                    System.Console.ResetColor();
                    System.Console.WriteLine($"   • Estado: [OK] Entrenado exitosamente");
                    System.Console.WriteLine($"   • Precisión: {mlResult.TrainingAccuracy:F3}");
                    System.Console.WriteLine($"   • Muestras: {mlResult.TrainingSamples}");
                }
            }
            catch (Exception ex)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine($"Error en análisis completo: {ex.Message}");
                System.Console.ResetColor();
            }
        }

        private static async Task LoadAndAnalyzeCsv()
        {
            DisplayManager.ShowSectionHeader("CARGAR Y ANALIZAR CSV");

            var filePath = DisplayManager.GetUserInput("Ingrese la ruta del archivo CSV (o Enter para usar ejemplo)");
            
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = "students.csv";
                await CreateSampleCsvFile(filePath);
            }

            if (!File.Exists(filePath))
            {
                DisplayManager.ShowError($"Archivo no encontrado: {filePath}");
                return;
            }

            try
            {
                var students = await _dataProcessingService.ReadStudentsFromCsvAsync(filePath);
                var cleanedStudents = _dataProcessingService.CleanStudentData(students);
                
                DisplayManager.ShowSuccess($"[OK] Se cargaron {cleanedStudents.Count} estudiantes");

                var analysisResult = _analyticsService.AnalyzeStudents(cleanedStudents);
                DisplayManager.ShowAnalysisResults(analysisResult);

                // Preguntar si desea guardar en blockchain
                var saveToBlockchain = DisplayManager.GetUserInput("¿Desea guardar estos datos en blockchain? (S/N)").ToUpper();
                if (saveToBlockchain == "S")
                {
                    var blocks = await StoreDataInBlockchain(cleanedStudents);
                    DisplayManager.ShowSuccess($"[OK] Se almacenaron {blocks.Count} bloques en blockchain");
                }
            }
            catch (Exception ex)
            {
                DisplayManager.ShowError($"Error cargando CSV: {ex.Message}");
            }
        }

        private static async Task GenerateSampleData()
        {
            DisplayManager.ShowSectionHeader("GENERAR DATOS DE EJEMPLO");

            var countStr = DisplayManager.GetUserInput("Número de estudiantes a generar (default: 30)");
            var count = int.TryParse(countStr, out var n) && n > 0 ? n : 30;

            var students = _dataProcessingService.GenerateSampleData(count);
            
            DisplayManager.ShowSuccess($"[OK] Se generaron {students.Count} estudiantes");

            // Mostrar primeros 10 estudiantes
            DisplayManager.ShowStudentTable(students.Take(10).ToList());

            // Preguntar si desea guardar en CSV
            var saveCsv = DisplayManager.GetUserInput("¿Desea guardar en archivo CSV? (S/N)").ToUpper();
            if (saveCsv == "S")
            {
                var filePath = "sample_students.csv";
                await _dataProcessingService.WriteStudentsToCsvAsync(students, filePath);
                DisplayManager.ShowSuccess($"[OK] Datos guardados en {filePath}");
            }
        }

        private static void TestCryptography()
        {
            DisplayManager.ShowSectionHeader("PRUEBA DE CRIPTOGRAFÍA");

            var originalText = DisplayManager.GetUserInput("Texto a encriptar (o Enter para usar ejemplo)");
            if (string.IsNullOrEmpty(originalText))
                originalText = "Juan Pérez - Estudiante Confidencial";

            try
            {
                // Encriptar
                var encrypted = _cryptoService.Encrypt(originalText);
                DisplayManager.ShowInfo($"Texto encriptado: {encrypted}");

                // Desencriptar
                var decrypted = _cryptoService.Decrypt(encrypted);
                DisplayManager.ShowInfo($"Texto desencriptado: {decrypted}");

                // Verificar
                if (originalText == decrypted)
                    DisplayManager.ShowSuccess("[OK] Encriptación/desencriptación exitosa");
                else
                    DisplayManager.ShowError("Error en encriptación/desencriptación");

                // Generar hash
                var hash = _cryptoService.GenerateHash(originalText);
                DisplayManager.ShowInfo($"Hash SHA256: {hash}");

                // Generar clave
                var key = _cryptoService.GenerateKey();
                DisplayManager.ShowInfo($"Nueva clave AES: {key}");
            }
            catch (Exception ex)
            {
                DisplayManager.ShowError($"Error en criptografía: {ex.Message}");
            }
        }

        private static void TestBlockchain()
        {
            DisplayManager.ShowSectionHeader("PRUEBA DE BLOCKCHAIN");

            try
            {
                // Agregar bloques de prueba
                var block1 = _blockchainService.AddBlock("Estudiante 1: Juan Pérez - Nota: 8.5");
                var block2 = _blockchainService.AddBlock("Estudiante 2: María García - Nota: 9.2");
                var block3 = _blockchainService.AddBlock("Estudiante 3: Carlos López - Nota: 7.8");

                DisplayManager.ShowSuccess("[OK] Bloques agregados exitosamente");
                DisplayManager.ShowInfo($"Bloque 1: {block1.Hash.Substring(0, 16)}...");
                DisplayManager.ShowInfo($"Bloque 2: {block2.Hash.Substring(0, 16)}...");
                DisplayManager.ShowInfo($"Bloque 3: {block3.Hash.Substring(0, 16)}...");

                // Validar cadena
                var isValid = _blockchainService.ValidateChain();
                DisplayManager.ShowInfo($"Integridad de blockchain: {(isValid ? "VÁLIDA" : "INVÁLIDA")}");

                // Mostrar estadísticas
                var stats = _blockchainService.GetStats();
                DisplayManager.ShowBlockchainStats(stats);
            }
            catch (Exception ex)
            {
                DisplayManager.ShowError($"Error en blockchain: {ex.Message}");
            }
        }

        private static async Task TestMachineLearning()
        {
            DisplayManager.ShowSectionHeader("PRUEBA DE MACHINE LEARNING");

            try
            {
                // Generar datos para entrenamiento
                var students = _dataProcessingService.GenerateSampleData(100);

                // Entrenar modelo
                DisplayManager.ShowInfo("Entrenando modelo...");
                var trainingResult = _machineLearningService.TrainPassingModel(students);

                if (trainingResult.Success)
                {
                    DisplayManager.ShowSuccess($"[OK] Modelo entrenado exitosamente");
                    DisplayManager.ShowInfo($"Precisión: {trainingResult.TrainingAccuracy:F3}");
                    DisplayManager.ShowInfo($"Muestras: {trainingResult.TrainingSamples}");

                    // Evaluar modelo
                    var testStudents = _dataProcessingService.GenerateSampleData(20);
                    var metrics = _machineLearningService.EvaluateModel(testStudents);
                    DisplayManager.ShowMLMetrics(metrics);

                    // Hacer predicciones
                    DisplayManager.ShowInfo("Realizando predicciones...");
                    var predictions = _machineLearningService.PredictForStudents(testStudents.Take(5).ToList());
                    DisplayManager.ShowPredictions(predictions);

                    // Predicción interactiva
                    var participationStr = DisplayManager.GetUserInput("Ingrese nivel de participación (0-10) para predicción");
                    if (double.TryParse(participationStr, out var participation))
                    {
                        var willPass = _machineLearningService.PredictPassing(participation);
                        var predictedGrade = _machineLearningService.PredictGrade(participation);
                        
                        DisplayManager.ShowInfo($"Predicción para participación {participation:F1}:");
                        DisplayManager.ShowInfo($"   ¿Aprobará?: {(willPass ? "SÍ" : "NO")}");
                        DisplayManager.ShowInfo($"   Nota predicha: {predictedGrade:F2}");
                    }
                }
                else
                {
                    DisplayManager.ShowError($"Error entrenando modelo: {trainingResult.Message}");
                }
            }
            catch (Exception ex)
            {
                DisplayManager.ShowError($"Error en Machine Learning: {ex.Message}");
            }
        }

        private static void ShowBlockchainInfo()
        {
            DisplayManager.ShowSectionHeader("INFORMACIÓN DE BLOCKCHAIN");

            try
            {
                var chain = _blockchainService.GetChain();
                var stats = _blockchainService.GetStats();

                DisplayManager.ShowBlockchainStats(stats);
                DisplayManager.ShowBlockchainChain(chain);
            }
            catch (Exception ex)
            {
                DisplayManager.ShowError($"Error obteniendo información: {ex.Message}");
            }
        }

        private static void ExportData()
        {
            DisplayManager.ShowSectionHeader("EXPORTAR DATOS");

            try
            {
                var chain = _blockchainService.GetChain();
                var json = ((BlockchainService)_blockchainService).ExportToJson();

                var filePath = "blockchain_export.json";
                File.WriteAllText(filePath, json);

                DisplayManager.ShowSuccess($"[OK] Datos exportados a {filePath}");
                DisplayManager.ShowInfo($"Total bloques: {chain.Count}");
                DisplayManager.ShowInfo($"Tamaño archivo: {new FileInfo(filePath).Length} bytes");
            }
            catch (Exception ex)
            {
                DisplayManager.ShowError($"Error exportando datos: {ex.Message}");
            }
        }

        private static async Task<List<EncryptedStudent>> EncryptStudentData(List<Student> students)
        {
            var encryptedStudents = new List<EncryptedStudent>();

            foreach (var student in students)
            {
                var encryptedName = _cryptoService.EncryptWithIV(student.Name, out var iv);

                encryptedStudents.Add(new EncryptedStudent
                {
                    Id = student.Id,
                    EncryptedName = encryptedName,
                    IV = iv,
                    Grade = student.Grade,
                    Participation = student.Participation,
                    CreatedAt = student.CreatedAt
                });
            }

            return encryptedStudents;
        }

        private static async Task<List<Block>> StoreDataInBlockchain(List<Student> students)
        {
            var blocks = new List<Block>();

            foreach (var student in students)
            {
                var studentJson = JsonSerializer.Serialize(new
                {
                    student.Id,
                    Name = student.Name, // En producción, esto debería estar encriptado
                    student.Grade,
                    student.Participation,
                    student.CreatedAt
                });

                var block = _blockchainService.AddBlock(studentJson);
                blocks.Add(block);
            }

            return blocks;
        }

        private static async Task CreateSampleCsvFile(string filePath)
        {
            var sampleData = @"Name,Grade,Participation
                                Juan Pérez,8.5,7.2
                                María García,9.2,8.5
                                Carlos López,7.8,6.9
                                Ana Martínez,6.5,5.8
                                Luis Rodríguez,8.9,7.6
                                Sofía Hernández,9.5,9.0
                                Pedro Díaz,5.8,4.2
                                Laura Sánchez,7.2,6.5
                                Miguel Torres,8.1,7.0
                                Carmen Jiménez,6.9,6.2";

            await File.WriteAllTextAsync(filePath, sampleData);
        }
    }
}
