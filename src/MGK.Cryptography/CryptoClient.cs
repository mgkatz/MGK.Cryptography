using MGK.Acceptance;

namespace MGK.Cryptography;

/// <summary>
/// A client to manage encryption and decryption of texts
/// </summary>
public class CryptoClient : ICryptoClient
{
    private readonly Aes _cryptoServiceProvider;

    /// <summary>
    /// Private constructor for the client.
    /// </summary>
    public CryptoClient()
    {
        // Creates an instance of Aes to generate a key and initialization vector (IV).
        _cryptoServiceProvider = Aes.Create();
    }

    /// <summary>
    /// Decrypts an encrypted object to get the original value.
    /// </summary>
    /// <param name="cryptoItem">The encrypted object.</param>
    /// <returns>A response containing the decrypted value.</returns>
    public ICryptoResponse Decrypt(ICryptoItem cryptoItem)
        => new CryptoResponse(GetDecryptedValue(cryptoItem));

    /// <summary>
    /// Decrypts an encrypted object to get the original value.
    /// </summary>
    /// <typeparam name="TResponse">The generic object that represents the response.</typeparam>
    /// <param name="cryptoItem">The encrypted object.</param>
    /// <returns>A response containing the decrypted value.</returns>
    public virtual TResponse Decrypt<TResponse>(ICryptoItem cryptoItem)
        where TResponse : ICryptoResponse, new() => new()
        {
            DecryptedValue = GetDecryptedValue(cryptoItem)
        };

    /// <summary>
    /// Encrypts a text.
    /// </summary>
    /// <param name="textToEncrypt">The text to encrypt.</param>
    /// <returns>An encrypted object.</returns>
    public ICryptoItem Encrypt(string textToEncrypt)
    {
        Ensure.Parameter.IsNotNullNorEmptyNorWhiteSpace(textToEncrypt, nameof(textToEncrypt));

        byte[] encryptedText;

        using (MemoryStream ms = new())
        {
            using CryptoStream cs = GetCryptoStream(ms, CryptoStreamMode.Write);
            using (StreamWriter sw = new(cs))
            {
                sw.Write(textToEncrypt);
            }

            encryptedText = ms.ToArray();
        }

        return new CryptoItem(encryptedText, _cryptoServiceProvider.Key, _cryptoServiceProvider.IV);
    }

    /// <summary>
    /// Gets a crypto stream.
    /// </summary>
    /// <param name="memoryStream">The stream to encrypt.</param>
    /// <param name="mode">If the crypto stream is for reading or writing.</param>
    /// <param name="cryptoItem">The encrypted item to read.</param>
    /// <returns>The crypto stream.</returns>
    private CryptoStream GetCryptoStream(MemoryStream memoryStream, CryptoStreamMode mode, ICryptoItem cryptoItem = null)
    {
        var cryptoProvider = Aes.Create();
        byte[] key = cryptoItem?.Key ?? _cryptoServiceProvider.Key;
        byte[] iv = cryptoItem?.InitializationVector ?? _cryptoServiceProvider.IV;

        var transform = mode == CryptoStreamMode.Read
            ? cryptoProvider.CreateDecryptor(key, iv)
            : cryptoProvider.CreateEncryptor(key, iv);

        return new CryptoStream(memoryStream, transform, mode);
    }

    /// <summary>
    /// Decrypts an encrypted object to get the original value.
    /// </summary>
    /// <param name="cryptoItem">The encrypted object.</param>
    /// <returns>The decrypted value.</returns>
    private string GetDecryptedValue(ICryptoItem cryptoItem)
    {
        Ensure.Parameter.IsNotNull(cryptoItem, nameof(cryptoItem));
        Ensure.Value.IsNotNullNorEmpty(cryptoItem.Key, nameof(cryptoItem.Key));
        Ensure.Value.IsNotNullNorEmpty(cryptoItem.InitializationVector, nameof(cryptoItem.InitializationVector));

        string decryptedValue;

        using (MemoryStream ms = new(cryptoItem.Value))
        {
            using CryptoStream cs = GetCryptoStream(ms, CryptoStreamMode.Read, cryptoItem);
            using StreamReader sr = new(cs);
            decryptedValue = sr.ReadToEnd();
        }

        return decryptedValue;
    }
}
