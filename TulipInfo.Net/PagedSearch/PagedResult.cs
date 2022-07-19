using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TulipInfo.Net
{
    public class PagedResult<ElementType>
    {
        public PagedResult(
            int pageIndex,
            int pageSize,
            int totalCount,
            IEnumerable<ElementType> elements)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
            this.Elements = elements;
        }
        public int PageIndex { get;private set; }
        public int PageSize { get;private set; }
        public int TotalCount { get;private set; }
        public IEnumerable<ElementType> Elements { get;private set; } = Enumerable.Empty<ElementType>();
        public IEnumerable<FieldFilter> Filters { get; set; } = Enumerable.Empty<FieldFilter>();
        public IEnumerable<FieldSort> Sorts { get; set; } = Enumerable.Empty<FieldSort>();
    }
}
