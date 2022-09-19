namespace MGK.Cryptography.Test.Models
{
    internal class CryptoItemTest : ICryptoItem
    {
        public byte[] Value { get; init; }

        public byte[] Key { get; init; }

        public byte[] InitializationVector { get; init; }
    }
}
