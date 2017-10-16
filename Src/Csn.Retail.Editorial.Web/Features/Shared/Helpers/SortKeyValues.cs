using System;
using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Listings.Models;

namespace Csn.Retail.Editorial.Web.Features.Shared.Helpers
{
    public class EditorialSortKeyValues
    {
        private const string LatestKey = "Latest";
        private const string LatestName = "Latest Articles";
        private const string OldestKey = "Oldest";
        private const string OldestName = "Oldest Articles";

        private static IDictionary<string, ISortKeyItem> _items;
        private static readonly Object ThisLock = new Object();

        public static IDictionary<string, ISortKeyItem> Items
        {
            get
            {
                lock (ThisLock)
                {
                    if (_items != null)
                    {
                        return _items;
                    }

                    _items = new Dictionary<string, ISortKeyItem>
                    {
                        {LatestKey, new SortKeyItem(LatestKey, LatestName)},
                        {OldestKey, new SortKeyItem(OldestKey, OldestName)}
                    };

                    return _items;
                }
            }
        }
    }
}