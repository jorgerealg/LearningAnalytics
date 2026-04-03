using System;
using System.IO;
using System.Text.Json;

namespace TestJsonOnly
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Probando exportación a JSON simple...");
                
                // Crear datos de prueba
                var testData = new[]
                {
                    new { Index = 0, Timestamp = DateTime.Now.ToString("O"), Data = "Genesis Block - Learning Analytics Blockchain", PreviousHash = "0", Hash = "genesis_hash", Nonce = 0 },
                    new { Index = 1, Timestamp = DateTime.Now.ToString("O"), Data = "Estudiante 1: Juan Pérez - Nota: 8.5 - Participación: 7.2", PreviousHash = "genesis_hash", Hash = "block1_hash", Nonce = 123 },
                    new { Index = 2, Timestamp = DateTime.Now.ToString("O"), Data = "Estudiante 2: María García - Nota: 9.2 - Participación: 8.5", PreviousHash = "block1_hash", Hash = "block2_hash", Nonce = 456 }
                };
                
                // Exportar a JSON
                Console.WriteLine("Exportando a JSON...");
                var json = JsonSerializer.Serialize(testData, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                
                // Guardar en archivo
                var filePath = "simple_export.json";
                File.WriteAllText(filePath, json);
                
                Console.WriteLine($"[OK] Datos exportados a {filePath}");
                Console.WriteLine($"Tamaño archivo: {new FileInfo(filePath).Length} bytes");
                
                // Mostrar contenido
                Console.WriteLine("\nContenido del archivo:");
                Console.WriteLine(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }
    }
}
