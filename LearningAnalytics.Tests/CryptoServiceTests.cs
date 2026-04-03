using System;
using LearningAnalytics.Infrastructure.Services;
using Xunit;

namespace LearningAnalytics.Tests
{
    /// <summary>
    /// Tests unitarios para CryptoService
    /// </summary>
    public class CryptoServiceTests
    {
        private readonly ICryptoService _cryptoService;

        public CryptoServiceTests()
        {
            _cryptoService = new CryptoService();
        }

        [Fact]
        public void Encrypt_Decrypt_ReturnsOriginalText()
        {
            // Arrange
            var originalText = "Este es un texto de prueba para encriptar";

            // Act
            var encrypted = _cryptoService.Encrypt(originalText);
            var decrypted = _cryptoService.Decrypt(encrypted);

            // Assert
            Assert.Equal(originalText, decrypted);
            Assert.NotEqual(originalText, encrypted);
            Assert.NotEmpty(encrypted);
        }

        [Fact]
        public void EncryptWithIV_DecryptWithIV_ReturnsOriginalText()
        {
            // Arrange
            var originalText = "Texto de prueba con IV separado";

            // Act
            var encrypted = _cryptoService.EncryptWithIV(originalText, out var iv);
            var decrypted = _cryptoService.DecryptWithIV(encrypted, iv);

            // Assert
            Assert.Equal(originalText, decrypted);
            Assert.NotEmpty(encrypted);
            Assert.NotEmpty(iv);
        }

        [Fact]
        public void GenerateHash_ReturnsConsistentHash()
        {
            // Arrange
            var input = "texto para hashear";

            // Act
            var hash1 = _cryptoService.GenerateHash(input);
            var hash2 = _cryptoService.GenerateHash(input);

            // Assert
            Assert.Equal(hash1, hash2);
            Assert.Equal(64, hash1.Length); // SHA256 produces 64 hex characters
        }

        [Fact]
        public void GenerateHash_DifferentInputs_ReturnDifferentHashes()
        {
            // Arrange
            var input1 = "texto1";
            var input2 = "texto2";

            // Act
            var hash1 = _cryptoService.GenerateHash(input1);
            var hash2 = _cryptoService.GenerateHash(input2);

            // Assert
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void GenerateKey_ReturnsValidBase64Key()
        {
            // Act
            var key = _cryptoService.GenerateKey();

            // Assert
            Assert.NotEmpty(key);
            
            // Verify it's valid base64
            var keyBytes = Convert.FromBase64String(key);
            Assert.True(keyBytes.Length == 32 || keyBytes.Length == 16); // AES-256 or AES-128
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Encrypt_EmptyText_ThrowsArgumentException(string text)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _cryptoService.Encrypt(text));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Decrypt_EmptyText_ThrowsArgumentException(string text)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _cryptoService.Decrypt(text));
        }

        [Fact]
        public void Decrypt_InvalidText_ThrowsException()
        {
            // Arrange
            var invalidCipherText = "texto_invalido_que_no_es_base64";

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => _cryptoService.Decrypt(invalidCipherText));
        }

        [Fact]
        public void GenerateHash_EmptyInput_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _cryptoService.GenerateHash(""));
        }

        [Fact]
        public void GenerateHash_NullInput_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _cryptoService.GenerateHash(null));
        }

        [Fact]
        public void MultipleEncryptions_SameText_ReturnDifferentResults()
        {
            // Arrange
            var originalText = "texto para encriptar múltiples veces";

            // Act
            var encrypted1 = _cryptoService.Encrypt(originalText);
            var encrypted2 = _cryptoService.Encrypt(originalText);

            // Assert
            Assert.NotEqual(encrypted1, encrypted2); // Different IV should produce different ciphertext
        }

        [Fact]
        public void Constructor_WithValidKey_InitializesCorrectly()
        {
            // Arrange
            var key = _cryptoService.GenerateKey();

            // Act
            var cryptoService = new CryptoService(key);

            // Assert
            var testText = "texto de prueba";
            var encrypted = cryptoService.Encrypt(testText);
            var decrypted = cryptoService.Decrypt(encrypted);
            
            Assert.Equal(testText, decrypted);
        }

        [Fact]
        public void Constructor_WithInvalidKey_ThrowsArgumentException()
        {
            // Arrange
            var invalidKey = "clave_invalida_no_base64";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new CryptoService(invalidKey));
        }
    }
}
