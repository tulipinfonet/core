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
        NotFound = 6
    }

    public class BusinessResultEmptyData
    {

    }

    public class BusinessFieldError
    {
        public BusinessFieldError()
        {

        }

        public BusinessFieldError(string fieldId, string message)
        {
            this.FieldId = fieldId;
            this.Message = message;
        }

        public string FieldId { get; set; }
        public string FieldValue { get; set; }

        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class BusinessResult: BusinessResult<BusinessResultEmptyData>
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
            : base(status,  errors)
        {
        }

        public BusinessResult(BusinessResultStatus status, string message, IEnumerable<BusinessFieldError> errors)
            : base(status, message,errors)
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


        public static readonly BusinessResult Success = new BusinessResult(BusinessResultStatus.Success);
    }

    public class BusinessResult<T> : IBusinessResult
    {
        public BusinessResult() : this(BusinessResultStatus.Unknown, "", "", null, default(T))
        {
        }

        public BusinessResult(BusinessResultStatus status) : this(status, "", "", null, default(T))
        {
        }

        public BusinessResult(T data) : this(BusinessResultStatus.Success, "", "", null, data)
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
            : this(status, "", message, null, default(T))
        {
        }

        public BusinessResult(BusinessResultStatus status, string message, IEnumerable<BusinessFieldError> errors) 
            : this(status, "", message, errors, default(T))
        {
        }

        public BusinessResult(BusinessResultStatus status, string code, string message) : this(status, code, message, null, default(T))
        {
        }

        public BusinessResult(BusinessResultStatus status, string code, string message, IEnumerable<BusinessFieldError> errors)
            : this(status, code, message, null, default(T))
        {
        }

        public BusinessResult(BusinessResultStatus status, string code, string message, IEnumerable<BusinessFieldError> errors, T data)
        {
            this.Status = status;
            this.Code = code??string.Empty;
            this.Message = message??string.Empty;
            this.FieldErrors = errors == null ? new BusinessFieldError[0] : errors;
            this.Data = data;
        }

        public Guid Id { get; protected set; } = Guid.NewGuid();
        public long Time { get; protected set; } = DateTime.UtcNow.Ticks;
        public BusinessResultStatus Status { get; protected set; } = BusinessResultStatus.Unknown;
        public string Code { get; protected set; }
        public string Message { get; protected set; }

        public IEnumerable<BusinessFieldError> FieldErrors { get; private set; }

        public T Data { get; protected set; }

        object IBusinessResult.Data => this.Data;
    }

    public interface IBusinessResult
    {
        Guid Id { get; }
        long Time { get; }
        BusinessResultStatus Status { get; }
        string Code { get; }
        string Message { get; }

        IEnumerable<BusinessFieldError> FieldErrors { get; }

        object Data { get; }
    }
}
