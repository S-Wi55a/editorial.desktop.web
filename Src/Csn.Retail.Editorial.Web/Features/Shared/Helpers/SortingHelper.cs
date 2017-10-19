﻿using System;
using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.Listings.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Shared.Helpers
{
    public interface ISortingHelper
    {
        SortingViewModel GenerateSortByViewModel(IDictionary<string, ISortKeyItem> sortKeys, string currrentSort, string query);
    }

    [AutoBind]
    public class SortingHelper : ISortingHelper
    {
        public SortingViewModel GenerateSortByViewModel(IDictionary<string, ISortKeyItem> sortKeys, string currrentSort, string query)
        {
            var model = new SortingViewModel
            {
                SortListItems = sortKeys.Select(x => new SortingItemViewModel
                {
                    Selected = x.Key.Equals(currrentSort, StringComparison.InvariantCultureIgnoreCase),
                    Label = x.Value.DisplayName,
                    Value = x.Value.Key,
                    Query = query
                }).ToList()
            };
            return model;
        }
    }
}