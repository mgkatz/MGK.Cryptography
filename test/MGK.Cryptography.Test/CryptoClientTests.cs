using MGK.Cryptography.Models;
using MGK.Cryptography.Test.Models;
using MGK.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MGK.Cryptography.Test
{
    [TestFixture]
    public class CryptoClientTests
    {
        // An object for the service provider.
        private IServiceProvider _serviceProvider;

        [SetUp]
        public void Setup()
        {
            // Create a collection of services to be used by the service provider.
            var collection = new ServiceCollection();

            // Register the context responsible for the strategies.
            collection.AddScoped<ICryptoClient, CryptoClient>();

            // Creates a service provider containing the items from the service collection provided.
            _serviceProvider = collection.BuildServiceProvider();
        }

        [Test]
        public void Encrypt_WhenValidText_ShouldReturnEncryptedItem()
        {
            var cryptoClient = _serviceProvider.GetService<ICryptoClient>();
            var encryptedItem = cryptoClient.Encrypt("qwerty");

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
            var cryptoClient = _serviceProvider.GetService<ICryptoClient>();
            Assert.Throws<ArgumentException>(() => cryptoClient.Encrypt(textToEncrypt));
        }

        [Test]
        public void Decrypt_WhenValidEncryptedItem_ShouldReturnDecryptedText()
        {
            var cryptoClient = _serviceProvider.GetService<ICryptoClient>();
            const string originalText = "qwerty";
            var encryptedItem = cryptoClient.Encrypt(originalText);
            var decryptedResponse = cryptoClient.Decrypt(encryptedItem);

            Assert.That(decryptedResponse.DecryptedValue, Is.EqualTo(expected: originalText));
        }

        [Test]
        public void Decrypt_WhenInvalidEncryptedItem_ShouldThrowException()
        {
            var cryptoClient = _serviceProvider.GetService<ICryptoClient>();
            ICryptoItem cryptoItem = null;
            Assert.Throws<ArgumentNullException>(() => cryptoClient.Decrypt(cryptoItem));

            cryptoItem = new CryptoItemTest
            {
                Value = "qwerty".ToByteArray(),
                Key = Array.Empty<byte>(),
                InitializationVector = Array.Empty<byte>()
            };
            Assert.Throws<Exception>(() => cryptoClient.Decrypt(cryptoItem));
        }
    }
}