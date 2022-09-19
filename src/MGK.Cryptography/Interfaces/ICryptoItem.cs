namespace MGK.Cryptography.Interfaces;

/// <summary>
/// Allow the implementation of an encrypted object.
/// </summary>
public interface ICryptoItem
{
    /// <summary>
    /// The encrypted value.
    /// </summary>
    byte[] Value { get; }

    /// <summary>
    /// The public encryption key.
    /// </summary>
    byte[] Key { get; }

    /// <summary>
    /// The initialization vector.
    /// </summary>
    byte[] InitializationVector { get; }
}
