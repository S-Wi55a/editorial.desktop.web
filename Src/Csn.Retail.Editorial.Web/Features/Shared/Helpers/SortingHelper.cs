﻿using System;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.Listings.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Shared.Helpers
{
    public interface ISortingHelper
    {
        SortingViewModel GenerateSortByViewModel(string currrentSort, string query, string keyword, string seoFragment);
    }

    [AutoBind]
    public class SortingHelper : ISortingHelper
    {
        public SortingViewModel GenerateSortByViewModel(string currrentSort, string query, string keyword, string seoFragment)
        {
            return new SortingViewModel
            {
                SortListItems = EditorialSortKeyValues.Items.Select(x => new SortingItemViewModel
                {
                    Selected = x.Key.Equals(currrentSort, StringComparison.InvariantCultureIgnoreCase),
                    Label = x.Value.DisplayName,
                    Value = x.Value.Key,
                    Url = ListingUrlHelper.GetPageAndSortPathAndQuery(query, sortOrder: x.Value.Key, keyword: keyword, seoFragment: seoFragment)
                }).ToList()
            };
        }
    }
}