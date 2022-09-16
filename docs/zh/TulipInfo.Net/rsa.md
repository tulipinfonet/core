# Rsa

## 定义

命名空间:TulipInfo.Net
程序集:TulipInfo.Net.dll

## 方法

### Export
- static string Encrypt(string input)
- static string Encrypt(this RSA rsa, string input)
- static string Decrypt(string input)
- static string Decrypt(this RSA rsa, string input)
- static string ExportDefaultPEMPrivateKey()
- static string ExportDefaultPEMPublicKey()
- static string ExportPemPrivateKey(this RSA csp)
- static string ExportPemPublicKey(this RSA csp)
- static void EncodeLength(BinaryWriter stream, int length)
- static void EncodeIntegerBigEndian(BinaryWriter stream, byte[] value, bool forceUnsigned = true)

### CreateRSA
- static RSA CreateRSA()
- static RSA CreateFromPemPrivateKey(string pemPrivateKeyString)
- static int GetIntegerSize(BinaryReader binr)
- static RSA CreateFromPemPublicKey(string pemPublicKeyString)
- static bool CompareBytearrays(byte[] a, byte[] b)
- static string TrimPemPriateKey(string privateKey)
- static string TrimPemPublicKey(string publicKey)