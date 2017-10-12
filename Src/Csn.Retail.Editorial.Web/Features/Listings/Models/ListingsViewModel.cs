using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Routing;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;

namespace Csn.Retail.Editorial.Web.Features.Listings.Models
{
    public class ListingsViewModel
    {
        public NavResult NavResults { get; set; }        
    }

    public class PagingItemViewModel
    {
        public long PageNo { get; set; }
        public long Skip { get; set; }
        public string Query { get; set; }
    }

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
        public bool ShowInitialSeparator { get; set; }
        public bool ShowTrailingSeparator { get; set; }
        public string DisplayText { get; set; }
    }

    public class SortingViewModel
    {
        public SortingViewModel()
        {
            SortListItems = Enumerable.Empty<SortingItemViewModel>();
        }

        public IEnumerable<SortingItemViewModel> SortListItems { get; set; }
    }

    public class SortingItemViewModel
    {
        public string Label { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }
}