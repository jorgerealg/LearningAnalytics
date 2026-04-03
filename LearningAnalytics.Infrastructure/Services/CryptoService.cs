using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace LearningAnalytics.Infrastructure.Services
{
    /// <summary>
    /// Implementación de servicio de criptografía usando AES-256
    /// </summary>
    public class CryptoService : ICryptoService
    {
        private readonly byte[] _key;
        private readonly IConfiguration? _configuration;

        /// <summary>
        /// Constructor que carga la clave desde configuración o genera una nueva
        /// </summary>
        /// <param name="configuration">Configuración de la aplicación</param>
        public CryptoService(IConfiguration? configuration = null)
        {
            _configuration = configuration;
            _key = GetOrCreateKey();
        }

        /// <summary>
        /// Constructor que acepta una clave específica
        /// </summary>
        /// <param name="key">Clave en base64</param>
        public CryptoService(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty", nameof(key));

            _key = Convert.FromBase64String(key);
            if (_key.Length != 32 && _key.Length != 16)
                throw new ArgumentException("Key must be 16 or 32 bytes (AES-128 or AES-256)", nameof(key));
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentException("Plain text cannot be null or empty", nameof(plainText));

            using var aes = Aes.Create();
            aes.Key = _key;
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            using var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            var encrypted = msEncrypt.ToArray();
            var result = new byte[aes.IV.Length + encrypted.Length];
            Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
            Buffer.BlockCopy(encrypted, 0, result, aes.IV.Length, encrypted.Length);

            return Convert.ToBase64String(result);
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentException("Cipher text cannot be null or empty", nameof(cipherText));

            var fullCipher = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Key = _key;

            var iv = new byte[aes.BlockSize / 8];
            var cipher = new byte[fullCipher.Length - iv.Length];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            using var msDecrypt = new MemoryStream(cipher);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }

        public string EncryptWithIV(string plainText, out string iv)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentException("Plain text cannot be null or empty", nameof(plainText));

            using var aes = Aes.Create();
            aes.Key = _key;
            aes.GenerateIV();

            iv = Convert.ToBase64String(aes.IV);

            using var encryptor = aes.CreateEncryptor();
            using var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            var encrypted = msEncrypt.ToArray();
            return Convert.ToBase64String(encrypted);
        }

        public string DecryptWithIV(string cipherText, string iv)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentException("Cipher text cannot be null or empty", nameof(cipherText));

            if (string.IsNullOrEmpty(iv))
                throw new ArgumentException("IV cannot be null or empty", nameof(iv));

            var encrypted = Convert.FromBase64String(cipherText);
            var ivBytes = Convert.FromBase64String(iv);

            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = ivBytes;

            using var decryptor = aes.CreateDecryptor();
            using var msDecrypt = new MemoryStream(encrypted);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }

        public string GenerateHash(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Input cannot be null or empty", nameof(input));

            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToHexString(hash).ToLower();
        }

        public string GenerateKey()
        {
            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.GenerateKey();
            return Convert.ToBase64String(aes.Key);
        }

        private byte[] GetOrCreateKey()
        {
            // Intentar obtener la clave desde configuración
            var keyFromConfig = _configuration?["CryptoKey"];
            if (!string.IsNullOrEmpty(keyFromConfig))
            {
                try
                {
                    return Convert.FromBase64String(keyFromConfig);
                }
                catch (FormatException)
                {
                    // Si la clave en configuración no es válida, generar una nueva
                }
            }

            // Generar una nueva clave
            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.GenerateKey();
            return aes.Key;
        }

        /// <summary>
        /// Obtiene la clave actual en formato base64 (para guardarla en configuración)
        /// </summary>
        /// <returns>Clave en base64</returns>
        public string GetKey()
        {
            return Convert.ToBase64String(_key);
        }
    }
}
