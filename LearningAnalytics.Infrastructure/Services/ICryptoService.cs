using System;

namespace LearningAnalytics.Infrastructure.Services
{
    /// <summary>
    /// Interfaz para servicios de criptografía
    /// </summary>
    public interface ICryptoService
    {
        /// <summary>
        /// Encripta un texto usando AES-256
        /// </summary>
        /// <param name="plainText">Texto a encriptar</param>
        /// <returns>Texto encriptado en base64</returns>
        string Encrypt(string plainText);

        /// <summary>
        /// Desencripta un texto usando AES-256
        /// </summary>
        /// <param name="cipherText">Texto encriptado en base64</param>
        /// <returns>Texto original</returns>
        string Decrypt(string cipherText);

        /// <summary>
        /// Encripta un texto y devuelve el IV utilizado
        /// </summary>
        /// <param name="plainText">Texto a encriptar</param>
        /// <param name="iv">Vector de inicialización utilizado</param>
        /// <returns>Texto encriptado en base64</returns>
        string EncryptWithIV(string plainText, out string iv);

        /// <summary>
        /// Desencripta un texto usando un IV específico
        /// </summary>
        /// <param name="cipherText">Texto encriptado en base64</param>
        /// <param name="iv">Vector de inicialización utilizado</param>
        /// <returns>Texto original</returns>
        string DecryptWithIV(string cipherText, string iv);

        /// <summary>
        /// Genera un hash SHA256 de un texto
        /// </summary>
        /// <param name="input">Texto a hashear</param>
        /// <returns>Hash en formato hexadecimal</returns>
        string GenerateHash(string input);

        /// <summary>
        /// Genera una clave aleatoria para AES
        /// </summary>
        /// <returns>Clave en base64</returns>
        string GenerateKey();
    }
}
