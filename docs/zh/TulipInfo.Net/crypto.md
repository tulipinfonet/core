# Crypto

## 定义

命名空间:TulipInfo.Net
程序集:TulipInfo.Net.dll

crypto 模块提供了加密功能，包含对 OpenSSL 的哈希、HMAC、加密、解密、签名、以及验证功能的一整套封装。

## 方法

### Encrypt

- static string Encrypt(string input)
- static string Encrypt(string input,string password)
- static string Encrypt(string input, string password, string salt)
- static string Encrypt(string input, string password, byte[] salt)

### Decrypt

- static string Decrypt(string input)
- static string Decrypt(string input,string password)
- static string Decrypt(string input, string password, string salt)
- static string Decrypt(string input, string password, byte[] salt)
