using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.Shared.Models
{
    public class CsnInsightsData
    {
        public Dictionary<string, string> MetaData { get; set; }
        public CsnInsightsSearchResultsData SearchResultsData { get; set; }
    }

    public class CsnInsightsSearchResultsData
    {
        public List<CsnInsightsSearchResultItem> Results { get; set; }
    }

    public class CsnInsightsSearchResultItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}