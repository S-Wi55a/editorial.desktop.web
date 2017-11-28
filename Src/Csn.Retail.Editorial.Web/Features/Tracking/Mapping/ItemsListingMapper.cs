using System.Linq;
using Bolt.Common.Extensions;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;

namespace Csn.Retail.Editorial.Web.Features.Tracking.Mapping
{
    public static class ItemsListingMapper
    {
        public static string MapMakes(SearchResultDto searchResult)
        {
            var allMakes = searchResult.Items.NullSafe().Where(x => !string.IsNullOrEmpty(x?.Make)).Select(x => x.Make).Distinct().ToList();
            return string.Join(",", allMakes);
        }

        public static string MapModels(SearchResultDto searchResult)
        {
            var allModels = searchResult.Items.NullSafe().Where(x => !string.IsNullOrEmpty(x?.Model)).Select(x => x.Model).Distinct().ToList();
            return string.Join(",", allModels);
        }
    }
}