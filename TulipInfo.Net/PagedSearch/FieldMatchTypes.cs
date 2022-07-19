using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TulipInfo.Net
{
    public enum FieldMatchTypes
    {
        Equal,
        NotEqual,
        In,
        NotIn,
        Like,
        NotLike,
        StartWith,
        NotStartWith,
        EndWith,
        NotEndWith,
        Empty,
        NotEmpty,
        Between,
        NotBetween,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
    }
}
