using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TulipInfo.Net
{
    public static class Crypto
    {
        internal static readonly string Password = "T#l1p0";
        internal static readonly byte[] Salt = new byte[] { 3, 25, 43, 43, 234, 89, 123, 240 };

        #region Encrypt
        public static string Encrypt(string input)
        {
            return Encrypt(input, Password, Salt);
        }

        public static string Encrypt(string input,string password)
        {
            return Encrypt(input, password, Salt);
        }

        public static string Encrypt(string input, string password, string salt)
        {
            byte[] saltBytes = System.Text.Encoding.UTF8.GetBytes(salt);
            return Encrypt(input,password, saltBytes);
        }

        public static string Encrypt(string input, string password, byte[] salt)
        {
            if(string.IsNullOrEmpty(input)|| string.IsNullOrEmpty(password) || salt==null || salt.Length==0)
            {
                return string.Empty;
            }

            byte[] saltUsed = new byte[8];
            for (var i = 0; i < salt.Length && i < 8; i++)
            {
                saltUsed[i] = salt[i];
            }
            byte[] encrypted;
            using (TripleDESCryptoServiceProvider tdsAlg = new TripleDESCryptoServiceProvider())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, saltUsed);
                tdsAlg.Key = pdb.GetBytes(24);
                tdsAlg.IV = pdb.GetBytes(8);

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = tdsAlg.CreateEncryptor(tdsAlg.Key, tdsAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(input);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Base64.UrlEncode(encrypted);
        }
        #endregion

        #region Decrypt
        public static string Decrypt(string input)
        {
            return Decrypt(input,Password, Salt);
        }

        public static string Decrypt(string input,string password)
        {
            return Decrypt(input, password, Salt);
        }

        public static string Decrypt(string input, string password, string salt)
        {
            byte[] saltBytes = System.Text.Encoding.UTF8.GetBytes(salt);
            return Decrypt(input, password, saltBytes);
        }

        public static string Decrypt(string input, string password, byte[] salt)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(password) || salt == null || salt.Length == 0)
            {
                return string.Empty;
            }

            byte[] saltUsed = new byte[8];
            for (var i = 0; i < salt.Length && i < 8; i++)
            {
                saltUsed[i] = salt[i];
            }

            byte[] cipherText = Base64.UrlDecodeToBytes(input);
            string plaintext = null;

            using (TripleDESCryptoServiceProvider tdsAlg = new TripleDESCryptoServiceProvider())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, saltUsed);
                tdsAlg.Key = pdb.GetBytes(24);
                tdsAlg.IV = pdb.GetBytes(8);

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = tdsAlg.CreateDecryptor(tdsAlg.Key, tdsAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            try
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                            }
                            catch
                            {
                                //ignore
                            }
                        }
                    }
                }

            }

            return plaintext;
        }
        #endregion
    }
}
