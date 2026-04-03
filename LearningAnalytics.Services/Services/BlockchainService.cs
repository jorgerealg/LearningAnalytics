using System;
using System.Collections.Generic;
using System.Linq;
using LearningAnalytics.Core.Models;

namespace LearningAnalytics.Services.Services
{
    /// <summary>
    /// Implementación de servicio de blockchain simulado
    /// </summary>
    public class BlockchainService : IBlockchainService
    {
        private readonly List<Block> _chain;
        private readonly object _lock = new object();

        /// <summary>
        /// Constructor que inicializa la cadena con el bloque génesis
        /// </summary>
        public BlockchainService()
        {
            _chain = new List<Block>();
            CreateGenesisBlock();
        }

        public List<Block> GetChain()
        {
            lock (_lock)
            {
                return new List<Block>(_chain);
            }
        }

        public Block AddBlock(string data)
        {
            if (string.IsNullOrEmpty(data))
                throw new ArgumentException("Data cannot be null or empty", nameof(data));

            lock (_lock)
            {
                var latestBlock = GetLatestBlock();
                var newBlock = new Block(_chain.Count, data, latestBlock.Hash);
                newBlock.MineBlock(2); // Dificultad de minería simulada

                _chain.Add(newBlock);
                return newBlock;
            }
        }

        public bool ValidateChain()
        {
            lock (_lock)
            {
                if (_chain.Count == 0)
                    return true;

                // Validar bloque génesis
                var genesisBlock = _chain[0];
                if (!genesisBlock.IsValid())
                    return false;

                // Validar el resto de la cadena
                for (int i = 1; i < _chain.Count; i++)
                {
                    var currentBlock = _chain[i];
                    var previousBlock = _chain[i - 1];

                    // Validar hash del bloque actual
                    if (!currentBlock.IsValid())
                        return false;

                    // Validar referencia al bloque anterior
                    if (currentBlock.PreviousHash != previousBlock.Hash)
                        return false;

                    // Validar índice consecutivo
                    if (currentBlock.Index != i)
                        return false;
                }

                return true;
            }
        }

        public Block GetLatestBlock()
        {
            lock (_lock)
            {
                return _chain.LastOrDefault() ?? _chain[0];
            }
        }

        public Block? GetBlockByIndex(int index)
        {
            lock (_lock)
            {
                if (index < 0 || index >= _chain.Count)
                    return null;

                return _chain[index];
            }
        }

        public BlockchainStats GetStats()
        {
            lock (_lock)
            {
                var isValid = ValidateChain();
                var totalDataEntries = _chain.Sum(b => string.IsNullOrEmpty(b.Data) ? 0 : 1);

                return new BlockchainStats
                {
                    TotalBlocks = _chain.Count,
                    TotalDataEntries = totalDataEntries,
                    FirstBlockTimestamp = _chain.Count > 0 ? _chain[0].Timestamp : DateTime.MinValue,
                    LastBlockTimestamp = _chain.Count > 0 ? _chain[^1].Timestamp : DateTime.MinValue,
                    IsValid = isValid
                };
            }
        }

        public void ClearChain()
        {
            lock (_lock)
            {
                _chain.Clear();
                CreateGenesisBlock();
            }
        }

        private void CreateGenesisBlock()
        {
            var genesisBlock = new Block();
            genesisBlock.Data = "Genesis Block - Learning Analytics Blockchain";
            genesisBlock.MineBlock(2);
            _chain.Add(genesisBlock);
        }

        /// <summary>
        /// Busca bloques que contienen datos específicos
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Bloques que contienen el término</returns>
        public List<Block> SearchBlocks(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return new List<Block>();

            lock (_lock)
            {
                return _chain.Where(b => 
                    b.Data.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }

        /// <summary>
        /// Exporta la cadena a formato JSON
        /// </summary>
        /// <returns>Representación JSON de la cadena</returns>
        public string ExportToJson()
        {
            lock (_lock)
            {
                var blocks = _chain.Select(b => new
                {
                    b.Index,
                    Timestamp = b.Timestamp.ToString("O"),
                    b.Data,
                    b.PreviousHash,
                    b.Hash,
                    b.Nonce
                });

                return System.Text.Json.JsonSerializer.Serialize(blocks, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });
            }
        }
    }
}
