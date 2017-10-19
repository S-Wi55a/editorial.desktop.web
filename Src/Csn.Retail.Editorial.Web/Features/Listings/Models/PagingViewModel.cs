using System.Collections.Generic;
using System.Linq;

namespace Csn.Retail.Editorial.Web.Features.Listings.Models
{
    public class PagingViewModel
    {
        public PagingViewModel()
        {
            Pages = Enumerable.Empty<PagingItemViewModel>();
        }
        public PagingItemViewModel First { get; set; }
        public PagingItemViewModel Last { get; set; }
        public PagingItemViewModel Previous { get; set; }
        public PagingItemViewModel Next { get; set; }
        public IEnumerable<PagingItemViewModel> Pages { get; set; }
        public long TotalCount { get; set; }
        public long TotalPageCount { get; set; }
        public long CurrentPageNo { get; set; }
        public string DisplayText { get; set; }
    }

    public class PagingItemViewModel
    {
        public long PageNo { get; set; }
        public long Offset { get; set; }
        public string Query { get; set; }
    }
}