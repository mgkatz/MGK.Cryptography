namespace MGK.Cryptography.Interfaces;

/// <summary>
/// Allow the implementation of a decrypted object.
/// </summary>
public interface ICryptoResponse
{
    /// <summary>
    /// The decrypted value.
    /// </summary>
    string DecryptedValue { get; init; }
}