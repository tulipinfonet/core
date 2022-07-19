using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TulipInfo.Net
{
    //reference:
    //https://gist.github.com/therightstuff/aa65356e95f8d0aae888e9f61aa29414
    //https://www.cnblogs.com/dudu/p/dotnet-core-rsa-openssl.html

    public static class Rsa
    {
        private const string PRIVATE_KEY_BEGIN = "-----BEGIN RSA PRIVATE KEY-----";
        private const string PRIVATE_KEY_END = "-----END RSA PRIVATE KEY-----";
        private const string PUBLIC_KEY_BEGIN = "-----BEGIN PUBLIC KEY-----";
        private const string PUBLIC_KEY_END = "-----END PUBLIC KEY-----";

        private static readonly string _pemPrivateKey = @"-----BEGIN RSA PRIVATE KEY-----
MIIEowIBAAKCAQEAvqXvdB8Q5XBZFdJ6yLXa4psZDFxegwvwwbXGP5i1k/ABsyQp
h5ziQD5RI1AcTBCdEEaaskfW5q5VnSagtMoB3yPjdnND3lI2k+TkdQeHZIkw1Ulv
1tv4G79tXzvcPo3yh8cou2sHdjEqcf6mTxE1G9FcPDWdDGXseTOnDPyKhhho8odV
mxg6C0/gxBP/pgZXaaiOGn5eJ9XmzeCX0VDfb7LO+v/cGNYLTwG2KBViBkNCtuPb
OMrcbGBfQU3lfXSdbW4g0izx2qpQXSBVeKNWnC5zGxHXRnSdYBMlpiltKacYASN6
dd4eH9bqmRdIdwpc+2ybIsI7AYh74lrS1zWY1wIDAQABAoIBAQCvOADo37h+txAZ
X1Zb68/dnyCZXLe8h5fh4TfwsWCJM/fL9nt42TaURvH5m6I2Qrqn/8wj+KTJZQbo
pVzkbBjmRazD69I/nZ3ttVxHNwE35GMOVC1G8uqIThqugWy9zWZPZjrIbejwtuVE
2f9uAemmxHeaGshZYb/B8TgiVmTWMjgzlWwemUhfEmmaSjSazk2tb4HlIp4wNDrJ
CvOw66WcIMSxIsGjtgdTiJcN5+l8k569jQtQwuJzhCvMHkq0hV8N693ksqjQYWCl
iK8YQy8bqpHWV1OLWIL29ccX9MZYkR2eNYi5zI4raN4s8Pw7kd3iGIOV2P0/vcRm
djOhHnMxAoGBAOtYv8LbgucGsyWtfjWw3Q6gGNiiuHNBmMP1OY/pSy/n2UvinFXS
7UialMbmz6RYEWVSgUek+Emvp0BS07/wS+9TjEezHbdlbzoNZ4h2sc1i4PqtSJ2Y
6ZwmDJ+VfcddZxjvAz+PdEfnOn+K8lcJEht7lP+NPP0BPgTpTKbFLXspAoGBAM9g
/7+3CexhHRmOzlZ2tfmye3FTl/NhE0FMLEh4mTk5X4DitilOq5M4NwO4vYjKE0zw
70DObVvjHLjXaCkm7wYxZhE0PWzpU9GYXT7SqdwXAmooPWzbAbkmgW39NI+i1Ie9
68mgqJhGcfPL6Lr1BtcqLJzCg7Yze+gBRLOLAPP/AoGAbc6g2lf1Qbm8iI3kX0TA
P3yvWxTBHvWyQ2v9iYn+TMHOfzuiWeDqWX/Wft9ebn2w0CeorjboqejNDpQWvG1v
4KCIyUNnUBSBywJKFj/bQcsq0YtcYDvic7rFFQh5ATYxTk7moxdZ19qpTq0T2Uwg
KLaGlJ3foOSdfUklPIKvb9ECgYAic/MJXtd/NgMno9oT81T4TvbDNSghfxc72fPh
dme+YhHDz+aDplZK9yPsslyKxUe/mKenFKSGh4zWCLN9YsKDPOHAKfWqinkqcS3M
qiMeNZRpHpZV52y53fPS2iZJVrwIcT0jGlXoNovn/RCFPwAL7y3KPWa0dok3Wj6X
tZhK8QKBgA/JUNNlQt2m0iNFcaYrZWgpVa13qy9OWIxIpL+JBSPZ7VFuoJLP9Ukb
rD4FIhFD/s/gsWP/cQYs3MEP7WIKnucI7fue5pbkdk0LBSO92V7MPI9tkEN5bSrM
H+WdS49GFIlERxf0vh8Dr4BxKDgxDEceGw12rEgn+3NpABvIFSBI
-----END RSA PRIVATE KEY-----";
        private static readonly string _pemPublicKey = @"-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAvqXvdB8Q5XBZFdJ6yLXa
4psZDFxegwvwwbXGP5i1k/ABsyQph5ziQD5RI1AcTBCdEEaaskfW5q5VnSagtMoB
3yPjdnND3lI2k+TkdQeHZIkw1Ulv1tv4G79tXzvcPo3yh8cou2sHdjEqcf6mTxE1
G9FcPDWdDGXseTOnDPyKhhho8odVmxg6C0/gxBP/pgZXaaiOGn5eJ9XmzeCX0VDf
b7LO+v/cGNYLTwG2KBViBkNCtuPbOMrcbGBfQU3lfXSdbW4g0izx2qpQXSBVeKNW
nC5zGxHXRnSdYBMlpiltKacYASN6dd4eH9bqmRdIdwpc+2ybIsI7AYh74lrS1zWY
1wIDAQAB
-----END PUBLIC KEY-----";

        /// <summary>
        /// Encrypt with the default public key
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Encrypt(string input)
        {
            using(var rsa = CreateFromPemPublicKey(_pemPublicKey))
            {
                return rsa.Encrypt(input);
            }
        }

        /// <summary>
        /// Encrypt the input string
        /// </summary>
        /// <param name="rsa"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Encrypt(this RSA rsa, string input)
        {
            byte[] encryptd = rsa.Encrypt(Encoding.UTF8.GetBytes(input), RSAEncryptionPadding.Pkcs1);

            string base64Str = Base64.UrlEncode(encryptd);

            return base64Str;
        }

        /// <summary>
        /// Decrypt with the default private key
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Decrypt(string input)
        {
            using(var rsa = CreateFromPemPrivateKey(_pemPrivateKey))
            {
                return rsa.Decrypt(input);
            }
        }

        /// <summary>
        /// Decrypt the input string
        /// </summary>
        /// <param name="rsa"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Decrypt(this RSA rsa, string input)
        {
            byte[] decrypted = rsa.Decrypt(Base64.UrlDecodeToBytes(input), RSAEncryptionPadding.Pkcs1);
            string rawText = Encoding.UTF8.GetString(decrypted);
            return rawText;
        }

        #region Export
        //reference: https://gist.github.com/therightstuff/aa65356e95f8d0aae888e9f61aa29414

        public static string ExportDefaultPEMPrivateKey()
        {
            var rsa = CreateFromPemPrivateKey(_pemPrivateKey);
            return ExportPemPrivateKey(rsa);
        }

        public static string ExportDefaultPEMPublicKey()
        {
            var rsa = CreateFromPemPublicKey(_pemPublicKey);
            return ExportPemPublicKey(rsa);
        }

        /// <summary>
        /// Export private (including public) key from MS RSACryptoServiceProvider into OpenSSH PEM string
        /// slightly modified from https://stackoverflow.com/a/23739932/2860309
        /// </summary>
        /// <param name="csp"></param>
        /// <returns></returns>
        public static string ExportPemPrivateKey(this RSA csp)
        {
            StringWriter outputStream = new StringWriter();
            var parameters = csp.ExportParameters(true);
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write((byte)0x30); // SEQUENCE
                using (var innerStream = new MemoryStream())
                {
                    var innerWriter = new BinaryWriter(innerStream);
                    EncodeIntegerBigEndian(innerWriter, new byte[] { 0x00 }); // Version
                    EncodeIntegerBigEndian(innerWriter, parameters.Modulus);
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent);
                    EncodeIntegerBigEndian(innerWriter, parameters.D);
                    EncodeIntegerBigEndian(innerWriter, parameters.P);
                    EncodeIntegerBigEndian(innerWriter, parameters.Q);
                    EncodeIntegerBigEndian(innerWriter, parameters.DP);
                    EncodeIntegerBigEndian(innerWriter, parameters.DQ);
                    EncodeIntegerBigEndian(innerWriter, parameters.InverseQ);
                    var length = (int)innerStream.Length;
                    EncodeLength(writer, length);
                    writer.Write(innerStream.GetBuffer(), 0, length);
                }

                var base64 = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length).ToCharArray();
                // WriteLine terminates with \r\n, we want only \n
                outputStream.Write($"{PRIVATE_KEY_BEGIN}\n");
                // Output as Base64 with lines chopped at 64 characters
                for (var i = 0; i < base64.Length; i += 64)
                {
                    outputStream.Write(base64, i, Math.Min(64, base64.Length - i));
                    outputStream.Write("\n");
                }
                outputStream.Write(PRIVATE_KEY_END);
            }

            return outputStream.ToString();
        }

        /// <summary>
        /// Export public key from MS RSACryptoServiceProvider into OpenSSH PEM string
        /// slightly modified from https://stackoverflow.com/a/28407693
        /// </summary>
        /// <param name="csp"></param>
        /// <returns></returns>
        public static string ExportPemPublicKey(this RSA csp)
        {
            StringWriter outputStream = new StringWriter();
            var parameters = csp.ExportParameters(false);
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write((byte)0x30); // SEQUENCE
                using (var innerStream = new MemoryStream())
                {
                    var innerWriter = new BinaryWriter(innerStream);
                    innerWriter.Write((byte)0x30); // SEQUENCE
                    EncodeLength(innerWriter, 13);
                    innerWriter.Write((byte)0x06); // OBJECT IDENTIFIER
                    var rsaEncryptionOid = new byte[] { 0x2a, 0x86, 0x48, 0x86, 0xf7, 0x0d, 0x01, 0x01, 0x01 };
                    EncodeLength(innerWriter, rsaEncryptionOid.Length);
                    innerWriter.Write(rsaEncryptionOid);
                    innerWriter.Write((byte)0x05); // NULL
                    EncodeLength(innerWriter, 0);
                    innerWriter.Write((byte)0x03); // BIT STRING
                    using (var bitStringStream = new MemoryStream())
                    {
                        var bitStringWriter = new BinaryWriter(bitStringStream);
                        bitStringWriter.Write((byte)0x00); // # of unused bits
                        bitStringWriter.Write((byte)0x30); // SEQUENCE
                        using (var paramsStream = new MemoryStream())
                        {
                            var paramsWriter = new BinaryWriter(paramsStream);
                            EncodeIntegerBigEndian(paramsWriter, parameters.Modulus); // Modulus
                            EncodeIntegerBigEndian(paramsWriter, parameters.Exponent); // Exponent
                            var paramsLength = (int)paramsStream.Length;
                            EncodeLength(bitStringWriter, paramsLength);
                            bitStringWriter.Write(paramsStream.GetBuffer(), 0, paramsLength);
                        }
                        var bitStringLength = (int)bitStringStream.Length;
                        EncodeLength(innerWriter, bitStringLength);
                        innerWriter.Write(bitStringStream.GetBuffer(), 0, bitStringLength);
                    }
                    var length = (int)innerStream.Length;
                    EncodeLength(writer, length);
                    writer.Write(innerStream.GetBuffer(), 0, length);
                }

                var base64 = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length).ToCharArray();
                // WriteLine terminates with \r\n, we want only \n
                outputStream.Write($"{PUBLIC_KEY_BEGIN}\n");
                for (var i = 0; i < base64.Length; i += 64)
                {
                    outputStream.Write(base64, i, Math.Min(64, base64.Length - i));
                    outputStream.Write("\n");
                }
                outputStream.Write(PUBLIC_KEY_END);
            }

            return outputStream.ToString();
        }

        /// <summary>
        /// https://stackoverflow.com/a/23739932/2860309
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="length"></param>
        private static void EncodeLength(BinaryWriter stream, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "Length must be non-negative");
            if (length < 0x80)
            {
                // Short form
                stream.Write((byte)length);
            }
            else
            {
                // Long form
                var temp = length;
                var bytesRequired = 0;
                while (temp > 0)
                {
                    temp >>= 8;
                    bytesRequired++;
                }
                stream.Write((byte)(bytesRequired | 0x80));
                for (var i = bytesRequired - 1; i >= 0; i--)
                {
                    stream.Write((byte)(length >> (8 * i) & 0xff));
                }
            }
        }

        /// <summary>
        /// https://stackoverflow.com/a/23739932/2860309
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        /// <param name="forceUnsigned"></param>
        private static void EncodeIntegerBigEndian(BinaryWriter stream, byte[] value, bool forceUnsigned = true)
        {
            stream.Write((byte)0x02); // INTEGER
            var prefixZeros = 0;
            for (var i = 0; i < value.Length; i++)
            {
                if (value[i] != 0) break;
                prefixZeros++;
            }
            if (value.Length - prefixZeros == 0)
            {
                EncodeLength(stream, 1);
                stream.Write((byte)0);
            }
            else
            {
                if (forceUnsigned && value[prefixZeros] > 0x7f)
                {
                    // Add a prefix zero to force unsigned if the MSB is 1
                    EncodeLength(stream, value.Length - prefixZeros + 1);
                    stream.Write((byte)0);
                }
                else
                {
                    EncodeLength(stream, value.Length - prefixZeros);
                }
                for (var i = prefixZeros; i < value.Length; i++)
                {
                    stream.Write(value[i]);
                }
            }
        }
        #endregion

        #region Create
        //reference: https://www.cnblogs.com/dudu/p/dotnet-core-rsa-openssl.html

        /// <summary>
        /// Create a new RSA instance
        /// </summary>
        /// <returns></returns>
        public static RSA CreateRSA()
        {
            return RSA.Create();
        }


        public static RSA CreateFromPemPrivateKey(string pemPrivateKeyString)
        {
            var privateKeyBits = System.Convert.FromBase64String(TrimPemPriateKey(pemPrivateKeyString));
            var rsa = RSA.Create();
            var RSAparams = new RSAParameters();

            using (var binr = new BinaryReader(new MemoryStream(privateKeyBits)))
            {
                byte bt = 0;
                ushort twobytes = 0;
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    throw new Exception("Unexpected value read binr.ReadUInt16()");

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)
                    throw new Exception("Unexpected version");

                bt = binr.ReadByte();
                if (bt != 0x00)
                    throw new Exception("Unexpected value read binr.ReadByte()");

                RSAparams.Modulus = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Exponent = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.D = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.P = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Q = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DP = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DQ = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
            }

            rsa.ImportParameters(RSAparams);
            return rsa;
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte();
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }

        public static RSA CreateFromPemPublicKey(string pemPublicKeyString)
        {
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] x509key;
            byte[] seq = new byte[15];
            int x509size;

            x509key = Convert.FromBase64String(TrimPemPublicKey(pemPublicKeyString));
            x509size = x509key.Length;

            using (var mem = new MemoryStream(x509key))
            {
                using (var binr = new BinaryReader(mem))
                {
                    byte bt = 0;
                    ushort twobytes = 0;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130)
                        binr.ReadByte();
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();
                    else
                        return null;

                    seq = binr.ReadBytes(15);
                    if (!CompareBytearrays(seq, SeqOID))
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8103)
                        binr.ReadByte();
                    else if (twobytes == 0x8203)
                        binr.ReadInt16();
                    else
                        return null;

                    bt = binr.ReadByte();
                    if (bt != 0x00)
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130)
                        binr.ReadByte();
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();
                    else
                        return null;

                    twobytes = binr.ReadUInt16();
                    byte lowbyte = 0x00;
                    byte highbyte = 0x00;

                    if (twobytes == 0x8102)
                        lowbyte = binr.ReadByte();
                    else if (twobytes == 0x8202)
                    {
                        highbyte = binr.ReadByte();
                        lowbyte = binr.ReadByte();
                    }
                    else
                        return null;
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                    int modsize = BitConverter.ToInt32(modint, 0);

                    int firstbyte = binr.PeekChar();
                    if (firstbyte == 0x00)
                    {
                        binr.ReadByte();
                        modsize -= 1;
                    }

                    byte[] modulus = binr.ReadBytes(modsize);

                    if (binr.ReadByte() != 0x02)
                        return null;
                    int expbytes = (int)binr.ReadByte();
                    byte[] exponent = binr.ReadBytes(expbytes);

                    var rsa = RSA.Create();
                    var rsaKeyInfo = new RSAParameters
                    {
                        Modulus = modulus,
                        Exponent = exponent
                    };
                    rsa.ImportParameters(rsaKeyInfo);
                    return rsa;
                }

            }
        }

        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }

        private static string TrimPemPriateKey(string privateKey)
        {
            return privateKey.Replace(PRIVATE_KEY_BEGIN, "").Replace(PRIVATE_KEY_END, "").Replace("\n", "");
        }

        private static string TrimPemPublicKey(string publicKey)
        {
            return publicKey.Replace(PUBLIC_KEY_BEGIN, "").Replace(PUBLIC_KEY_END, "").Replace("\n", "");
        }
        #endregion
    }

}
