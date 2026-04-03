using System;
using System.IO;
using System.Text.Json;

namespace TestBlockchainExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Probando exportación a JSON con datos reales de blockchain...");
                
                // Crear instancia del servicio real
                var blockchainService = new LearningAnalytics.Services.Services.BlockchainService();
                
                // Agregar bloques de prueba
                Console.WriteLine("Agregando bloques de prueba...");
                blockchainService.AddBlock("Estudiante 1: Juan Pérez - Nota: 8.5 - Participación: 7.2");
                blockchainService.AddBlock("Estudiante 2: María García - Nota: 9.2 - Participación: 8.5");
                blockchainService.AddBlock("Estudiante 3: Carlos López - Nota: 7.8 - Participación: 6.9");
                
                // Obtener la cadena
                var chain = blockchainService.GetChain();
                Console.WriteLine($"Total bloques: {chain.Count}");
                
                // Exportar a JSON usando el método real
                Console.WriteLine("Exportando a JSON...");
                var json = blockchainService.ExportToJson();
                
                // Guardar en archivo
                var filePath = "blockchain_real_export.json";
                File.WriteAllText(filePath, json);
                
                Console.WriteLine($"[OK] Datos exportados a {filePath}");
                Console.WriteLine($"Tamaño archivo: {new FileInfo(filePath).Length} bytes");
                
                // Validar JSON
                try
                {
                    using var document = JsonDocument.Parse(json);
                    Console.WriteLine("[OK] JSON generado es válido");
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Error: JSON inválido - {ex.Message}");
                }
                
                // Validar cadena
                var isValid = blockchainService.ValidateChain();
                Console.WriteLine($"Blockchain válida: {isValid}");
                
                // Mostrar primeros 500 caracteres del JSON
                Console.WriteLine("\nVista previa del JSON:");
                var preview = json.Length > 500 ? json.Substring(0, 500) + "..." : json;
                Console.WriteLine(preview);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }
    }
}
