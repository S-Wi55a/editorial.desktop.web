using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.Listings.Models
{
    public class PagingViewModel
    {
        public PagingItemViewModel First { get; set; }
        public PagingItemViewModel Last { get; set; }
        public PagingItemViewModel Previous { get; set; }
        public PagingItemViewModel Next { get; set; }
        public IEnumerable<PagingItemViewModel> Pages { get; set; }
        public long TotalCount { get; set; }
        public long TotalPageCount { get; set; }
        public long CurrentPageNo { get; set; }
        public int Limit { get; set; }
        public string DisplayText { get; set; }
    }

    public class PagingItemViewModel
    {
        public long PageNo { get; set; }
        public long Skip { get; set; }
        public string Query { get; set; }
    }
}