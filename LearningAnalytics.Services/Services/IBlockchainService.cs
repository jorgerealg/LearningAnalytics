using System.Collections.Generic;
using LearningAnalytics.Core.Models;

namespace LearningAnalytics.Services.Services
{
    /// <summary>
    /// Interfaz para servicios de blockchain
    /// </summary>
    public interface IBlockchainService
    {
        /// <summary>
        /// Obtiene toda la cadena de bloques
        /// </summary>
        /// <returns>Lista de bloques</returns>
        List<Block> GetChain();

        /// <summary>
        /// Agrega un nuevo bloque a la cadena
        /// </summary>
        /// <param name="data">Datos a almacenar en el bloque</param>
        /// <returns>Bloque agregado</returns>
        Block AddBlock(string data);

        /// <summary>
        /// Valida la integridad de toda la cadena
        /// </summary>
        /// <returns>True si la cadena es válida</returns>
        bool ValidateChain();

        /// <summary>
        /// Obtiene el último bloque de la cadena
        /// </summary>
        /// <returns>Último bloque</returns>
        Block GetLatestBlock();

        /// <summary>
        /// Obtiene un bloque por su índice
        /// </summary>
        /// <param name="index">Índice del bloque</param>
        /// <returns>Bloque encontrado o null</returns>
        Block? GetBlockByIndex(int index);

        /// <summary>
        /// Obtiene estadísticas de la cadena
        /// </summary>
        /// <returns>Estadísticas</returns>
        BlockchainStats GetStats();

        /// <summary>
        /// Limpia toda la cadena (solo para pruebas)
        /// </summary>
        void ClearChain();
    }

    /// <summary>
    /// Estadísticas de la blockchain
    /// </summary>
    public class BlockchainStats
    {
        public int TotalBlocks { get; set; }
        public int TotalDataEntries { get; set; }
        public DateTime FirstBlockTimestamp { get; set; }
        public DateTime LastBlockTimestamp { get; set; }
        public bool IsValid { get; set; }
    }
}
