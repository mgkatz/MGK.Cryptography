using MGK.Acceptance;

namespace MGK.Cryptography
{
	/// <summary>
	/// An encrypted object.
	/// </summary>
	public sealed class CryptoItem : ICryptoItem
	{
		/// <summary>
		/// Encrypted object's constructor.
		/// </summary>
		/// <param name="value">The encrypted value.</param>
		/// <param name="key">The public encryption key.</param>
		/// <param name="initializationVector">The initialization vector.</param>
		public CryptoItem(byte[] value, byte[] key, byte[] initializationVector)
		{
			Ensure.Parameter.IsNotNullNorEmpty(value, nameof(value));
			Ensure.Parameter.IsNotNullNorEmpty(key, nameof(key));
			Ensure.Parameter.IsNotNullNorEmpty(initializationVector, nameof(initializationVector));

			Value = value;
			Key = key;
			InitializationVector = initializationVector;
		}

		/// <summary>
		/// The encrypted value.
		/// </summary>
		public byte[] Value { get; }

		/// <summary>
		/// The public encryption key.
		/// </summary>
		public byte[] Key { get; }

		/// <summary>
		/// The initialization vector.
		/// </summary>
		public byte[] InitializationVector { get; }

		public override bool Equals(object obj)
		{
			return obj is CryptoItem item &&
				   Value.Equals(item.Value) &&
				   Key.Equals(item.Key) &&
				   InitializationVector.Equals(item.InitializationVector);
		}

		public override int GetHashCode()
		{
			var hashCode = 1533609244;
			hashCode = hashCode * -1521134295 + Value.GetHashCode();
			hashCode = hashCode * -1521134295 + Key.GetHashCode();
			hashCode = hashCode * -1521134295 + InitializationVector.GetHashCode();
			return hashCode;
		}
	}
}
