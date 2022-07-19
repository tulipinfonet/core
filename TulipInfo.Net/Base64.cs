using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TulipInfo.Net
{
    public static class Base64
    {
        const char Base64PadCharacter = '=';
        const char Base64Character62 = '+';
        const char Base64UrlCharacter62 = '-';
        const char Base64Character63 = '/';
        const char Base64UrlCharacter63 = '\u005F';
        const string DoubleBase64PadCharacter = "==";

        public static string UrlEncode(string input)
        {
            if (input == null)
                throw new ArgumentNullException("Base64UrlEncode");

            return UrlEncode(Encoding.UTF8.GetBytes(input));
        }

        public static string UrlEncode(byte[] input)
        {
            if (input == null)
                throw new ArgumentNullException("Base64UrlEncode");

            string base64String = Convert.ToBase64String(input);
            base64String = base64String.Split(new char[] { Base64PadCharacter })[0];
            base64String = base64String.Replace(Base64Character62, Base64UrlCharacter62);
            return base64String.Replace(Base64Character63, Base64UrlCharacter63);
        }

        public static string UrlDecode(string input)
        {
            return Encoding.UTF8.GetString(UrlDecodeToBytes(input));
        }

        public static byte[] UrlDecodeToBytes(string input)
        {
            if (input == null)
                throw new ArgumentNullException("Base64UrlDecodeBytes");

            object[] objArray;
            CultureInfo invariantCulture = CultureInfo.InvariantCulture;

            input = input.Replace(Base64UrlCharacter62, Base64Character62);
            input = input.Replace(Base64UrlCharacter63, Base64Character63);
            switch (input.Length % 4)
            {
                case 0:
                    {
                        break;
                    }
                case 1:
                    {
                        objArray = new object[] { input };
                        throw new FormatException(string.Format(invariantCulture, "Unable to decode: '{0}' as Base64url encoded string.", objArray));
                    }
                case 2:
                    {
                        input = string.Concat(input, DoubleBase64PadCharacter);
                        break;
                    }
                case 3:
                    {
                        input = string.Concat(input, Base64PadCharacter.ToString());
                        break;
                    }
                default:
                    {
                        objArray = new object[] { input };
                        throw new FormatException(string.Format(invariantCulture, "Unable to decode: '{0}' as Base64url encoded string.", objArray));
                    }
            }
            return Convert.FromBase64String(input);
        }
    }
}
