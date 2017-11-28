using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.WebMetrics.Core.Model;

namespace Csn.Retail.Editorial.Web.Features.Tracking.Mapping
{
    public static class SearchFilterMapper
    {
        public static IEnumerable<SearchFilterItem> Map(RyvussNavResultDto ryvussNavResult)
        {
            if (ryvussNavResult?.INav == null) return null;

            return GetFacetted(ryvussNavResult.INav.BreadCrumbs).Union(GetKeyword(ryvussNavResult.INav.BreadCrumbs));
        }

        private static IEnumerable<SearchFilterItem> GetFacetted(IEnumerable<BreadCrumbDto> breadCrumbs)
        {
            //would be nice to use this with the other form of linq, but can't use select many which we need for the flatten
            return breadCrumbs?.Where(x => x.IsFacetBreadCrumb)
                .SelectMany(x =>
                {
                    var list = new List<SearchFilterItem> {new SearchFilterItem(x.AspectDisplay, x.Facet)};

                    if (x.Children != null)
                    {
                        list.AddRange(GetFacetted(x.Children));
                    }

                    return list;
                });
        }

        private static IEnumerable<SearchFilterItem> GetKeyword(IEnumerable<BreadCrumbDto> breadCrumbs)
        {
            return breadCrumbs.Where(bc => bc.IsKeywordBreadCrumb).Select(bc => new SearchFilterItem("Keywords", bc.Term));
        }
    }
}