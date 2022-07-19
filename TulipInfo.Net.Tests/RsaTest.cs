using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TulipInfo.Net.Tests
{
    [TestClass]
    public class RsaTest
    {
        [TestMethod]
        public void Test_Encrypt_Decrypt_Default()
        {
            string strToEncrypt = "Data to Encrypt";
            string strEncrypted = Rsa.Encrypt(strToEncrypt);
            string strDecrypted = Rsa.Decrypt(strEncrypted);
            Assert.AreEqual(strToEncrypt, strDecrypted);

        }

        [TestMethod]
        public void Test_Export_Import_Encrypt_Decrypt()
        {
            string strToEncrypt = "Data to Encrypt";

            RSA exportRsa = Rsa.CreateRSA();
            string pemPrivateKey = exportRsa.ExportPemPrivateKey();
            string pemPublicKey = exportRsa.ExportPemPublicKey();
            string strEncrypted = exportRsa.Encrypt(strToEncrypt);

            RSA importtedPrivateRsa = Rsa.CreateFromPemPrivateKey(pemPrivateKey);
            string strDecrypted = importtedPrivateRsa.Decrypt(strEncrypted);
            Assert.AreEqual(strToEncrypt, strDecrypted);


            RSA importtedPublicRsa = Rsa.CreateFromPemPublicKey(pemPublicKey);
            strEncrypted = importtedPublicRsa.Encrypt(strToEncrypt);

            importtedPrivateRsa = Rsa.CreateFromPemPrivateKey(pemPrivateKey);
            strDecrypted = importtedPrivateRsa.Decrypt(strEncrypted);
            Assert.AreEqual(strToEncrypt, strDecrypted);
        }

        [TestMethod]
        public void Test_Export_Import_PrivateKey()
        {
            RSA exportRsa = Rsa.CreateRSA();
            string pemPrivateKey = exportRsa.ExportPemPrivateKey();

            RSA importtedPrivateRsa = Rsa.CreateFromPemPrivateKey(pemPrivateKey);
            string pemPriateKeyCompare = importtedPrivateRsa.ExportPemPrivateKey();

            Assert.AreEqual(pemPrivateKey, pemPriateKeyCompare);
        }
    }
}
