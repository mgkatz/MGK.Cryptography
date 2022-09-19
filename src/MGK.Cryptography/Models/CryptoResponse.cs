namespace MGK.Cryptography.Models;

/// <summary>
/// A response containing the decrypted value.
/// </summary>
/// <param name="DecryptedValue">The real value of a decrypted object.</param>
public record CryptoResponse(string DecryptedValue) : ICryptoResponse;