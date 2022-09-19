namespace MGK.Cryptography.Interfaces;

public interface ICryptoClient
{
    /// <summary>
    /// Decrypts an encrypted object to get the original value.
    /// </summary>
    /// <param name="cryptoItem">The encrypted object.</param>
    /// <returns>A response containing the decrypted value.</returns>
    ICryptoResponse Decrypt(ICryptoItem cryptoItem);

    /// <summary>
    /// Decrypts an encrypted object to get the original value.
    /// </summary>
    /// <typeparam name="TResponse">The generic object that represents the response.</typeparam>
    /// <param name="cryptoItem">The encrypted object.</param>
    /// <returns>A response containing the decrypted value.</returns>
    TResponse Decrypt<TResponse>(ICryptoItem cryptoItem)
        where TResponse : ICryptoResponse, new();

    /// <summary>
    /// Encrypts a text.
    /// </summary>
    /// <param name="textToEncrypt">The text to encrypt.</param>
    /// <returns>An encrypted object.</returns>
    ICryptoItem Encrypt(string textToEncrypt);
}