using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace TulipInfo.Net
{
    public static class Salt
    {
        public static string Generate(int bytesLength = 16)
        {
            return Base64.UrlEncode(GenerateBytes(bytesLength));
        }

        public static byte[] GenerateBytes(int bytesLength = 16)
        {
            byte[] salt = new byte[bytesLength];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
