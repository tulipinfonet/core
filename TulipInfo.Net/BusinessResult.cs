using System;
using System.Collections.Generic;
using System.Text;

namespace TulipInfo.Net
{
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

    public class BusinessResultEmptyData
    {

    }

    public class BusinessFieldError
    {
        public BusinessFieldError()
        {

        }

        public BusinessFieldError(string name, string message) : this(name, "", message)
        {
        }

        public BusinessFieldError(string name, string code, string message)
        {
            this.Name = name;
            this.Code = code;
            this.Message = message;
        }

        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class BusinessResult : BusinessResult<BusinessResultEmptyData>
    {
        public BusinessResult() : base()
        {
        }

        public BusinessResult(BusinessResultStatus status) : base(status)
        {
        }

        public BusinessResult(IEnumerable<BusinessFieldError> errors)
            : base(errors)
        {
        }

        public BusinessResult(BusinessResultStatus status, IEnumerable<BusinessFieldError> errors)
            : base(status, errors)
        {
        }

        public BusinessResult(BusinessResultStatus status, string message, IEnumerable<BusinessFieldError> errors)
            : base(status, message, errors)
        {
        }

        public BusinessResult(BusinessResultStatus status, string message) : base(status, "", message)
        {
        }

        public BusinessResult(BusinessResultStatus status, string code, string message) : base(status, code, message)
        {
        }

        public BusinessResult(BusinessResultStatus status, string code, string message, IEnumerable<BusinessFieldError> errors)
            : base(status, code, message)
        {
        }
        public BusinessResult(IBusinessResult br) : this(br.Status, br.Code, br.Message, br.FieldErrors)
        {
        }

        public static readonly BusinessResult Success = new BusinessResult(BusinessResultStatus.Success);
    }

    public class BusinessResult<T> : IBusinessResult
    {
        public BusinessResult() : this(BusinessResultStatus.Unknown, "", "", Enumerable.Empty<BusinessFieldError>(), default(T))
        {
        }

        public BusinessResult(BusinessResultStatus status) : this(status, "", "", Enumerable.Empty<BusinessFieldError>(), default(T))
        {
        }

        public BusinessResult(T data) : this(BusinessResultStatus.Success, "", "", Enumerable.Empty<BusinessFieldError>(), data)
        {
        }

        public BusinessResult(IEnumerable<BusinessFieldError> errors)
            : this(BusinessResultStatus.Failure, "", "", errors, default(T))
        {
        }

        public BusinessResult(BusinessResultStatus status, IEnumerable<BusinessFieldError> errors)
            : this(status, "", "", errors, default(T))
        {
        }

        public BusinessResult(BusinessResultStatus status, string message)
            : this(status, "", message, Enumerable.Empty<BusinessFieldError>(), default(T))
        {
        }

        public BusinessResult(BusinessResultStatus status, string message, IEnumerable<BusinessFieldError> errors)
            : this(status, "", message, errors, default(T))
        {
        }

        public BusinessResult(BusinessResultStatus status, string code, string message) : this(status, code, message, Enumerable.Empty<BusinessFieldError>(), default(T))
        {
        }

        public BusinessResult(BusinessResultStatus status, string code, string message, IEnumerable<BusinessFieldError> errors)
            : this(status, code, message, Enumerable.Empty<BusinessFieldError>(), default(T))
        {
        }

        public BusinessResult(IBusinessResult br)
            : this(br.Status, br.Code, br.Message, br.FieldErrors, default(T))
        {
        }

        public BusinessResult(BusinessResult<T> br)
            : this(br.Status, br.Code, br.Message, br.FieldErrors, br.Data)
        {
        }

        public BusinessResult(BusinessResultStatus status, string code, string message, IEnumerable<BusinessFieldError> errors, T? data)
        {
            this.Status = status;
            this.Code = code ?? string.Empty;
            this.Message = message ?? string.Empty;
            this.FieldErrors = errors == null ? new BusinessFieldError[0] : errors;
            this.Data = data;
        }

        public Guid Id { get; protected set; } = Guid.NewGuid();
        public long Time { get; protected set; } = DateTime.UtcNow.Ticks;
        public BusinessResultStatus Status { get; protected set; } = BusinessResultStatus.Unknown;
        public string Code { get; protected set; }
        public string Message { get; protected set; }

        public IEnumerable<BusinessFieldError> FieldErrors { get; private set; }

        public T? Data { get; protected set; }

        object? IBusinessResult.Data => this.Data;
    }

    public interface IBusinessResult
    {
        Guid Id { get; }
        long Time { get; }
        BusinessResultStatus Status { get; }
        string Code { get; }
        string Message { get; }

        IEnumerable<BusinessFieldError> FieldErrors { get; }

        object? Data { get; }
    }
}
