using System;
using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.Listings.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Formatters;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Shared.Helpers
{
    public interface ISortingHelper
    {
        SortingViewModel GenerateSortByViewModel(IDictionary<string, ISortKeyItem> sortKeys, string currrentSort, string query, string keyword);
    }

    [AutoBind]
    public class SortingHelper : ISortingHelper
    {
        public SortingViewModel GenerateSortByViewModel(IDictionary<string, ISortKeyItem> sortKeys, string currrentSort, string query, string keyword)
        {
            var model = new SortingViewModel
            {
                SortListItems = sortKeys.Select(x => new SortingItemViewModel
                {
                    Selected = x.Key.Equals(currrentSort, StringComparison.InvariantCultureIgnoreCase),
                    Label = x.Value.DisplayName,
                    Value = x.Value.Key,
                    Url = $"?q={query}{UrlParamsFormatter.GetSortParam(x.Value.Key)}{UrlParamsFormatter.GetKeywordParam(keyword)}"
                }).ToList()
            };
            return model;
        }
    }
}