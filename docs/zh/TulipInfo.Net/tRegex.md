# TRegex

## 定义

命名空间:TulipInfo.Net
程序集:TulipInfo.Net.dll

## 正则
``` cs
  ChineseMobilePattern = "^1(3([0-35-9]\\d|4[1-8])|4[14-9]\\d|5([0-35689]\\d|7[1-79])|66\\d|7[2-35-8]\\d|8\\d{2}|9[13589]\\d)\\d{7}$";
  ChineseMobileSimplePattern = "^1[0-9]{10}$";
  EmailAddressPattern = @"^[\w!#$%&'*+/=?`{|}~^-]+(?:\.[\w!#$%&'*+/=?`{|}~^-]+)*@(?:[A-Z0-9-]+\.)+[A-Z]{2,6}$";
  StrongPasswordPattern = @"(?=^.{8,}$)(?=.*\d)(?=.*[^\w\s]+)(?=.*[A-Z])(?=.*[a-z]).*$";
  StrongPasswordPatternWithOutSymbol = @"(?=^.{8,}$)(?=.*\d)(?=.*[A-Z])(?=.*[a-z]).*$";
```


## 方法
- static bool IsChineseMobile(string input, bool simpleCheck = false)
- static bool IsEmail(string input)
- static bool IsMatch(string input, string pattern)
- static bool IsMatch(string input, string pattern, RegexOptions options)