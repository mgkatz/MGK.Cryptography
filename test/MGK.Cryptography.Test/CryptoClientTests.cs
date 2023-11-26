using MGK.Cryptography.Test.Models;
using MGK.Extensions;
using System;

namespace MGK.Cryptography.Test
{
    [TestFixture]
    public class CryptoClientTests
    {
        private ICryptoClient _cryptoClient;

        [SetUp]
        public void Setup()
        {
            _cryptoClient = new CryptoClient();
        }

        [Test]
        public void Encrypt_WhenValidText_ShouldReturnEncryptedItem()
        {
            var encryptedItem = _cryptoClient.Encrypt("qwerty");

            Assert.That(encryptedItem, Is.Not.Null);
            Assert.That(encryptedItem.Value, Is.Not.Null);
            Assert.That(encryptedItem.Value, Is.Not.Empty);
            Assert.That(encryptedItem.Key, Is.Not.Null);
            Assert.That(encryptedItem.Key, Is.Not.Empty);
            Assert.That(encryptedItem.InitializationVector, Is.Not.Null);
            Assert.That(encryptedItem.InitializationVector, Is.Not.Empty);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("          ")]
        public void Encrypt_WhenInvalidText_ShouldThrowException(string textToEncrypt)
        {
            Assert.Throws<ArgumentException>(() => _cryptoClient.Encrypt(textToEncrypt));
        }

        [Test]
        public void Decrypt_WhenValidEncryptedItem_ShouldReturnDecryptedText()
        {
            const string originalText = "qwerty";
            var encryptedItem = _cryptoClient.Encrypt(originalText);
            var decryptedResponse = _cryptoClient.Decrypt(encryptedItem);

            Assert.That(decryptedResponse.DecryptedValue, Is.EqualTo(expected: originalText));
        }

        [Test]
        public void Decrypt_WhenInvalidEncryptedItem_ShouldThrowException()
        {
            ICryptoItem cryptoItem = null;
            Assert.Throws<ArgumentNullException>(() => _cryptoClient.Decrypt(cryptoItem));

            cryptoItem = new CryptoItemTest
            {
                Value = "qwerty".ToByteArray(),
                Key = Array.Empty<byte>(),
                InitializationVector = Array.Empty<byte>()
            };
            Assert.Throws<Exception>(() => _cryptoClient.Decrypt(cryptoItem));
        }
    }
}