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
                Console.WriteLine("Probando exportación de blockchain a JSON...");
                
                // Crear bloques de prueba
                var blocks = new[]
                {
                    new { Index = 0, Timestamp = DateTime.UtcNow, Data = "Genesis Block - Learning Analytics Blockchain", PreviousHash = "0", Hash = "", Nonce = 0 },
                    new { Index = 1, Timestamp = DateTime.UtcNow, Data = "Estudiante 1: Juan Pérez - Nota: 8.5 - Participación: 7.2", PreviousHash = "", Hash = "", Nonce = 0 },
                    new { Index = 2, Timestamp = DateTime.UtcNow, Data = "Estudiante 2: María García - Nota: 9.2 - Participación: 8.5", PreviousHash = "", Hash = "", Nonce = 0 }
                };
                
                // Simular cálculo de hashes (similar al Block.CalculateHash)
                var simulatedBlocks = new object[blocks.Length];
                for (int i = 0; i < blocks.Length; i++)
                {
                    var block = blocks[i];
                    string hash;
                    
                    if (i == 0)
                    {
                        // Genesis block
                        hash = CalculateHash(block.Index, block.Timestamp, block.Data, block.PreviousHash, block.Nonce);
                        simulatedBlocks[i] = new { block.Index, Timestamp = block.Timestamp.ToString("O"), block.Data, block.PreviousHash, Hash = hash, block.Nonce };
                    }
                    else
                    {
                        // Bloques regulares - usar hash anterior
                        var prevBlock = (dynamic)simulatedBlocks[i - 1];
                        string prevHash = prevBlock.Hash;
                        hash = CalculateHash(block.Index, block.Timestamp, block.Data, prevHash, block.Nonce);
                        simulatedBlocks[i] = new { block.Index, Timestamp = block.Timestamp.ToString("O"), block.Data, PreviousHash = prevHash, Hash = hash, block.Nonce };
                    }
                }
                
                // Exportar a JSON
                Console.WriteLine("Exportando a JSON...");
                var json = JsonSerializer.Serialize(simulatedBlocks, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                
                // Guardar en archivo
                var filePath = "blockchain_export.json";
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
        
        static string CalculateHash(int index, DateTime timestamp, string data, string previousHash, int nonce)
        {
            // Simulación del cálculo SHA256
            var rawData = $"{index}{timestamp:O}{data}{previousHash}{nonce}";
            var hash = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(rawData));
            return Convert.ToHexString(hash).ToLower();
        }
    }
}
