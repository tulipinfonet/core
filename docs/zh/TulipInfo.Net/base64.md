# Base64

## 定义

命名空间:TulipInfo.Net
程序集:TulipInfo.Net.dll

字符串的Base64编码与解码帮助类。由于已经包含了URL的编码与解码，因此与系统函数的Convert.FromBase64,Convert.ToBase64 不能相互调用。

## 方法

- static string UrlEncode(string input)
- static string UrlEncode(byte[] input)
- static string UrlDecode(string input)
- static byte[] UrlDecodeToBytes(string input)
