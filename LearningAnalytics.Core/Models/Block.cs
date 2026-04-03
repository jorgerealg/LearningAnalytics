using System;
using System.Security.Cryptography;
using System.Text;

namespace LearningAnalytics.Core.Models
{
    /// <summary>
    /// Representa un bloque en la cadena de blockchain
    /// </summary>
    public class Block
    {
        /// <summary>
        /// Índice del bloque en la cadena
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Timestamp de creación del bloque
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Datos almacenados en el bloque (en formato JSON)
        /// </summary>
        public string Data { get; set; } = string.Empty;

        /// <summary>
        /// Hash del bloque anterior
        /// </summary>
        public string PreviousHash { get; set; } = string.Empty;

        /// <summary>
        /// Hash del bloque actual
        /// </summary>
        public string Hash { get; set; } = string.Empty;

        /// <summary>
        /// Nonce para proof of work (simulado)
        /// </summary>
        public int Nonce { get; set; }

        /// <summary>
        /// Constructor para el bloque génesis
        /// </summary>
        public Block()
        {
            Index = 0;
            PreviousHash = "0";
            Hash = CalculateHash();
        }

        /// <summary>
        /// Constructor para bloques regulares
        /// </summary>
        /// <param name="index">Índice del bloque</param>
        /// <param name="data">Datos a almacenar</param>
        /// <param name="previousHash">Hash del bloque anterior</param>
        public Block(int index, string data, string previousHash)
        {
            Index = index;
            Data = data;
            PreviousHash = previousHash;
            Hash = CalculateHash();
        }

        /// <summary>
        /// Calcula el hash SHA256 del bloque
        /// </summary>
        /// <returns>Hash en formato hexadecimal</returns>
        public string CalculateHash()
        {
            using var sha256 = SHA256.Create();
            var rawData = $"{Index}{Timestamp:O}{Data}{PreviousHash}{Nonce}";
            var bytes = Encoding.UTF8.GetBytes(rawData);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToHexString(hash).ToLower();
        }

        /// <summary>
        /// Minera el bloque (simulación de proof of work)
        /// </summary>
        /// <param name="difficulty">Dificultad de minería (número de ceros al inicio)</param>
        public void MineBlock(int difficulty = 2)
        {
            var target = new string('0', difficulty);
            
            while (Hash.Substring(0, difficulty) != target)
            {
                Nonce++;
                Hash = CalculateHash();
            }
        }

        /// <summary>
        /// Valida la integridad del bloque
        /// </summary>
        /// <returns>True si el hash es válido</returns>
        public bool IsValid()
        {
            var calculatedHash = CalculateHash();
            return Hash.Equals(calculatedHash, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return $"Block #{Index} | Timestamp: {Timestamp:O} | Hash: {Hash.Substring(0, 16)}... | Data: {(Data.Length > 50 ? Data.Substring(0, 50) + "..." : Data)}";
        }
    }
}
