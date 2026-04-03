using System;
using System.IO;
using LearningAnalytics.Services.Services;
using LearningAnalytics.Infrastructure.Services;

namespace TestExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Probando exportación de blockchain...");
                
                // Crear servicios
                var blockchainService = new BlockchainService();
                
                // Agregar algunos bloques de prueba
                Console.WriteLine("Agregando bloques de prueba...");
                blockchainService.AddBlock("Estudiante 1: Juan Pérez - Nota: 8.5 - Participación: 7.2");
                blockchainService.AddBlock("Estudiante 2: María García - Nota: 9.2 - Participación: 8.5");
                blockchainService.AddBlock("Estudiante 3: Carlos López - Nota: 7.8 - Participación: 6.9");
                
                // Obtener la cadena
                var chain = blockchainService.GetChain();
                Console.WriteLine($"Total bloques: {chain.Count}");
                
                // Exportar a JSON
                Console.WriteLine("Exportando a JSON...");
                var json = blockchainService.ExportToJson();
                
                // Guardar en archivo
                var filePath = "blockchain_export.json";
                File.WriteAllText(filePath, json);
                
                Console.WriteLine($"[OK] Datos exportados a {filePath}");
                Console.WriteLine($"Tamaño archivo: {new FileInfo(filePath).Length} bytes");
                
                // Mostrar contenido
                Console.WriteLine("\nContenido del archivo:");
                Console.WriteLine(json);
                
                // Validar cadena
                var isValid = blockchainService.ValidateChain();
                Console.WriteLine($"\nBlockchain válida: {isValid}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }
    }
}
