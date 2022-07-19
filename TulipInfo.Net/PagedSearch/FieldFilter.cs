using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TulipInfo.Net
{
    public class FieldFilter
    {
        public string FieldName { get; set; } = string.Empty;
        public FieldMatchTypes MatchType { get; set; } = FieldMatchTypes.Equal;
        public object[]? Values { get; set; }
    }
}
