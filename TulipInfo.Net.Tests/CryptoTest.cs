using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TulipInfo.Net.Tests
{
    [TestClass]
    public class CryptoTest
    {
        [TestMethod]
        public void Test()
        {
            string input = "test";
            string encryted= Crypto.Encrypt(input);
            string decrypted = Crypto.Decrypt(encryted);
            Assert.AreEqual(input, decrypted);

            string password = "Password1";
            encryted = Crypto.Encrypt(input, password);
            decrypted = Crypto.Decrypt(encryted, password);
            Assert.AreEqual(input, decrypted);

            string wrongDecrypted = Crypto.Decrypt(encryted, "wrpassword");
            Assert.AreNotEqual(input, wrongDecrypted);

            byte[] salt = Salt.GenerateBytes();
            encryted = Crypto.Encrypt(input, password, salt);
            decrypted = Crypto.Decrypt(encryted, password, salt);
            Assert.AreEqual(input, decrypted);

            wrongDecrypted = Crypto.Decrypt(encryted, "wrpassword", salt);
            Assert.AreNotEqual(input, wrongDecrypted);


            wrongDecrypted = Crypto.Decrypt(encryted,password, Salt.GenerateBytes());
            Assert.AreNotEqual(input, wrongDecrypted);
        }
    }
}
