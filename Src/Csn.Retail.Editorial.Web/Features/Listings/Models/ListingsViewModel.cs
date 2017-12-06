using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Models;

namespace Csn.Retail.Editorial.Web.Features.Listings.Models
{
    public class ListingsViewModel
    {
        public NavResult NavResults { get; set; }
        public PolarNativeAdsData PolarNativeAdsData { get; set; }
        public PagingViewModel Paging { get; set; }
        public SortingViewModel Sorting { get; set; }
        public string Keyword { get; set; }    
        public string DisqusSource { get; set; }
        public bool ShowSponsoredLinks { get; set; }
        public Dictionary<string, string> InsightsData { get; set; }
        public SeoData SeoData { get; set; }
        public EditorialPageTypes EditorialPageType { get; set; }
    }
}