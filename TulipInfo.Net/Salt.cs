using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace TulipInfo.Net
{
    public static class Salt
    {
        public static string Generate(int bitLength = 128)
        {
            return Base64.UrlEncode(GenerateBytes(bitLength));
        }

        public static byte[] GenerateBytes(int bitLength = 128)
        {
            byte[] salt = new byte[bitLength / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
