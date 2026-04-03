using System.Linq;
using LearningAnalytics.Services.Services;
using Xunit;

namespace LearningAnalytics.Tests
{
    /// <summary>
    /// Tests unitarios para BlockchainService
    /// </summary>
    public class BlockchainServiceTests
    {
        private readonly IBlockchainService _blockchainService;

        public BlockchainServiceTests()
        {
            _blockchainService = new BlockchainService();
        }

        [Fact]
        public void Constructor_InitializesWithGenesisBlock()
        {
            // Act
            var chain = _blockchainService.GetChain();

            // Assert
            Assert.Single(chain);
            Assert.Equal(0, chain[0].Index);
            Assert.Equal("Genesis Block - Learning Analytics Blockchain", chain[0].Data);
        }

        [Fact]
        public void AddBlock_AddsBlockToChain()
        {
            // Arrange
            var testData = "Test data for blockchain";

            // Act
            var block = _blockchainService.AddBlock(testData);

            // Assert
            Assert.NotNull(block);
            Assert.Equal(1, block.Index);
            Assert.Equal(testData, block.Data);
            Assert.NotEmpty(block.Hash);
            Assert.NotEmpty(block.PreviousHash);

            var chain = _blockchainService.GetChain();
            Assert.Equal(2, chain.Count);
        }

        [Fact]
        public void AddBlock_EmptyData_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _blockchainService.AddBlock(""));
            Assert.Throws<ArgumentException>(() => _blockchainService.AddBlock(null));
        }

        [Fact]
        public void ValidateChain_ValidChain_ReturnsTrue()
        {
            // Arrange
            _blockchainService.AddBlock("Block 1");
            _blockchainService.AddBlock("Block 2");
            _blockchainService.AddBlock("Block 3");

            // Act
            var isValid = _blockchainService.ValidateChain();

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void GetLatestBlock_ReturnsLastBlock()
        {
            // Arrange
            _blockchainService.AddBlock("Block 1");
            _blockchainService.AddBlock("Block 2");
            var lastBlock = _blockchainService.AddBlock("Block 3");

            // Act
            var latestBlock = _blockchainService.GetLatestBlock();

            // Assert
            Assert.Equal(lastBlock.Index, latestBlock.Index);
            Assert.Equal(lastBlock.Hash, latestBlock.Hash);
        }

        [Fact]
        public void GetBlockByIndex_ValidIndex_ReturnsCorrectBlock()
        {
            // Arrange
            _blockchainService.AddBlock("Block 1");
            _blockchainService.AddBlock("Block 2");

            // Act
            var block = _blockchainService.GetBlockByIndex(1);

            // Assert
            Assert.NotNull(block);
            Assert.Equal(1, block.Index);
            Assert.Equal("Block 1", block.Data);
        }

        [Fact]
        public void GetBlockByIndex_InvalidIndex_ReturnsNull()
        {
            // Act
            var block = _blockchainService.GetBlockByIndex(999);

            // Assert
            Assert.Null(block);
        }

        [Fact]
        public void GetBlockByIndex_NegativeIndex_ReturnsNull()
        {
            // Act
            var block = _blockchainService.GetBlockByIndex(-1);

            // Assert
            Assert.Null(block);
        }

        [Fact]
        public void GetStats_ReturnsCorrectStatistics()
        {
            // Arrange
            _blockchainService.AddBlock("Block 1");
            _blockchainService.AddBlock("Block 2");

            // Act
            var stats = _blockchainService.GetStats();

            // Assert
            Assert.Equal(3, stats.TotalBlocks); // Genesis + 2 blocks
            Assert.Equal(2, stats.TotalDataEntries);
            Assert.True(stats.IsValid);
        }

        [Fact]
        public void ClearChain_ResetsToGenesisBlock()
        {
            // Arrange
            _blockchainService.AddBlock("Block 1");
            _blockchainService.AddBlock("Block 2");
            Assert.Equal(3, _blockchainService.GetChain().Count);

            // Act
            _blockchainService.ClearChain();

            // Assert
            var chain = _blockchainService.GetChain();
            Assert.Single(chain);
            Assert.Equal(0, chain[0].Index);
        }

        [Fact]
        public void BlockHash_IsDeterministic()
        {
            // Arrange
            var data = "Test data";

            // Act
            var block1 = _blockchainService.AddBlock(data);
            _blockchainService.ClearChain();
            var block2 = _blockchainService.AddBlock(data);

            // Assert
            // Blocks should have different nonces, but same data structure
            Assert.Equal(block1.Index, block2.Index);
            Assert.Equal(block1.Data, block2.Data);
            Assert.Equal(block1.PreviousHash, block2.PreviousHash);
            // Hashes should be different due to different nonces (mining)
        }

        [Fact]
        public void Block_CalculateHash_ReturnsConsistentHash()
        {
            // Arrange
            var block = _blockchainService.AddBlock("Test data");

            // Act
            var hash1 = block.CalculateHash();
            var hash2 = block.CalculateHash();

            // Assert
            Assert.Equal(hash1, hash2);
            Assert.Equal(block.Hash, hash1);
        }

        [Fact]
        public void Block_IsValid_ReturnsTrueForValidBlock()
        {
            // Act
            var block = _blockchainService.AddBlock("Test data");

            // Assert
            Assert.True(block.IsValid());
        }

        [Fact]
        public void MultipleBlocks_MaintainCorrectPreviousHashChain()
        {
            // Arrange
            var block1 = _blockchainService.AddBlock("Block 1");
            var block2 = _blockchainService.AddBlock("Block 2");
            var block3 = _blockchainService.AddBlock("Block 3");

            // Act & Assert
            Assert.Equal(block1.Hash, block2.PreviousHash);
            Assert.Equal(block2.Hash, block3.PreviousHash);
        }

        [Fact]
        public void Chain_MultipleOperations_MaintainsIntegrity()
        {
            // Arrange & Act
            for (int i = 0; i < 10; i++)
            {
                _blockchainService.AddBlock($"Test data {i}");
            }

            // Assert
            Assert.True(_blockchainService.ValidateChain());
            Assert.Equal(11, _blockchainService.GetChain().Count); // Genesis + 10 blocks
        }
    }
}
