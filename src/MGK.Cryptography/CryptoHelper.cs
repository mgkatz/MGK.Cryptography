using MGK.Acceptance;
using System.IO;
using System.Security.Cryptography;

namespace MGK.Cryptography
{
	/// <summary>
	/// A helper to manage encryption and decryption of texts
	/// </summary>
	public sealed class CryptoHelper
	{
		private static CryptoHelper _instance = null;
		private static readonly object _padlock = new object();

		private readonly AesCryptoServiceProvider _cryptoServiceProvider;

		/// <summary>
		/// Private constructor for the helper.
		/// </summary>
		private CryptoHelper()
		{
			// Create a new AesCryptoServiceProvider object to generate a key and initialization vector (IV).
			_cryptoServiceProvider = new AesCryptoServiceProvider();
		}

		/// <summary>
		/// Gets an instance of the CryptoHelper.
		/// </summary>
		public static CryptoHelper Instance
		{
			get
			{
				lock (_padlock)
				{
					if (_instance == null)
						_instance = new CryptoHelper();

					return _instance;
				}
			}
		}

		/// <summary>
		/// Decrypts an encrypted object to get the original text.
		/// </summary>
		/// <param name="cryptoItem">The encrypted object.</param>
		/// <returns>The decrypted text.</returns>
		public string Decrypt(ICryptoItem cryptoItem)
		{
			Ensure.Parameter.IsNotNull(cryptoItem, nameof(cryptoItem));

			string decryptedText;

			using (MemoryStream ms = new MemoryStream(cryptoItem.Value))
			{
				using (CryptoStream cs = GetCryptoStream(ms, CryptoStreamMode.Read, cryptoItem))
				{
					using (StreamReader sr = new StreamReader(cs))
					{
						decryptedText = sr.ReadToEnd();
					}
				}
			}

			return decryptedText;
		}

		/// <summary>
		/// Encrypts a text.
		/// </summary>
		/// <param name="textToEncrypt">The text to encrypt.</param>
		/// <returns>An encrypted object.</returns>
		public ICryptoItem Encrypt(string textToEncrypt)
		{
			Ensure.Parameter.IsNotNullNorEmptyNorWhiteSpace(textToEncrypt, nameof(textToEncrypt));

			byte[] encryptedText;

			using (MemoryStream ms = new MemoryStream())
			{
				using (CryptoStream cs = GetCryptoStream(ms, CryptoStreamMode.Write))
				{
					using (StreamWriter sw = new StreamWriter(cs))
					{
						sw.Write(textToEncrypt);
					}

					encryptedText = ms.ToArray();
				}
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
			var cryptoProvider = new AesCryptoServiceProvider();
			byte[] key = cryptoItem?.Key ?? _cryptoServiceProvider.Key;
			byte[] iv = cryptoItem?.InitializationVector ?? _cryptoServiceProvider.IV;

			var transform = mode == CryptoStreamMode.Read
				? cryptoProvider.CreateDecryptor(key, iv)
				: cryptoProvider.CreateEncryptor(key, iv);

			return new CryptoStream(memoryStream, transform, mode);
		}
	}
}
