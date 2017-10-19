using System.Collections.Generic;
using System.Linq;

namespace Csn.Retail.Editorial.Web.Features.Listings.Models
{
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
        public string Url { get; set; }
        public bool Selected { get; set; }
    }
}