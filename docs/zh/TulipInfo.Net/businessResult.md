# BusinessResult

## 定义

命名空间:TulipInfo.Net
程序集:TulipInfo.Net.dll

## 属性

```cs
    public enum BusinessResultStatus
    {
        Unknown = 0,
        Success = 1,
        Warning = 2,
        Failure = 3,
        AuthorityFailure = 4,
        ValidationError = 5,
        NotFound = 6,
        NotAllowed=7,
        InvalidRequest=8
    }
```

## 方法 
### BusinessResult
- BusinessResult()
- BusinessResult(BusinessResultStatus status)
- BusinessResult(IEnumerable<BusinessFieldError> errors)
- BusinessResult(BusinessResultStatus status, IEnumerable<BusinessFieldError> errors)
- BusinessResult(BusinessResultStatus status, string message, IEnumerable<BusinessFieldError> errors)
- BusinessResult(BusinessResultStatus status, string message)
- BusinessResult(BusinessResultStatus status, string code, string message)
- BusinessResult(BusinessResultStatus status, string code, string message, IEnumerable<BusinessFieldError> errors)