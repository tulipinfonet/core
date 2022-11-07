# Hash

## 定义

命名空间:TulipInfo.Net
程序集:TulipInfo.Net.dll

Hash，一般翻译做散列、杂凑，或音译为哈希，是把任意长度的输入（又叫做预映射pre-image）通过散列算法变换成固定长度的输出，该输出就是散列值。这种转换是一种压缩映射，也就是，散列值的空间通常远小于输入的空间，不同的输入可能会散列成相同的输出，所以不可能从散列值来确定唯一的输入值。简单的说就是一种将任意长度的消息压缩到某一固定长度的消息摘要的函数。

## 方法
- static string MD5(string input)
- static string MD5(byte[] input)
- static string SHA256(string input)
- static uint CRC32(string text)
