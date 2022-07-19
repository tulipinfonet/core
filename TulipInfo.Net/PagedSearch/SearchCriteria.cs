using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TulipInfo.Net
{
    public class SearchCriteria
    {
        public string SearchKey { get; set; } = String.Empty;
        public int PageIndex { get; set; }
        public int PageSize { get; set; } = 20;
        public IEnumerable<FieldFilter> Filters { get; set; } = Enumerable.Empty<FieldFilter>();
        public IEnumerable<FieldSort> Sorts { get; set; } = Enumerable.Empty<FieldSort>();
    }
}
