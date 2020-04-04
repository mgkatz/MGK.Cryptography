using MGK.Extensions;
using NUnit.Framework;
using System;

namespace MGK.Cryptography.Test
{
	public class CryptoHelperTests
	{
		private readonly CryptoHelper _cryptoHelper;

		public CryptoHelperTests()
		{
			_cryptoHelper = CryptoHelper.Instance;
		}

		[Test]
		public void Encrypt_WhenValidText_ShouldReturnEncryptedItem()
		{
			var encryptedItem = _cryptoHelper.Encrypt("qwerty");

			Assert.IsNotNull(encryptedItem);
			Assert.IsNotNull(encryptedItem.Value);
			Assert.IsNotEmpty(encryptedItem.Value);
			Assert.IsNotNull(encryptedItem.Key);
			Assert.IsNotEmpty(encryptedItem.Key);
			Assert.IsNotNull(encryptedItem.InitializationVector);
			Assert.IsNotEmpty(encryptedItem.InitializationVector);
		}

		[TestCase(null)]
		[TestCase("")]
		[TestCase("          ")]
		public void Encrypt_WhenInvalidText_ShouldThrowException(string textToEncrypt)
			=> Assert.Throws<ArgumentException>(() => _cryptoHelper.Encrypt(textToEncrypt));

		[Test]
		public void Decrypt_WhenValidEncryptedItem_ShouldReturnDecryptedText()
		{
			var originalText = "qwerty";
			var encryptedItem = _cryptoHelper.Encrypt(originalText);
			var decryptedText = _cryptoHelper.Decrypt(encryptedItem);

			Assert.AreEqual(originalText, decryptedText);
		}

		[Test]
		public void Decrypt_WhenInvalidEncryptedItem_ShouldThrowException()
			=> Assert.Throws<ArgumentNullException>(() => _cryptoHelper.Decrypt(null));
	}
}