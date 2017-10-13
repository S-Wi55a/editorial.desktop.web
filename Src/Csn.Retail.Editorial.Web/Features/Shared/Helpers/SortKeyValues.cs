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
        private const string TitleKey = "Title";
        private const string TitleName = "Title (A to Z)";
        private const string TitleDescKey = "~Title";
        private const string TitleDescName = "Title (Z to A)";

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
                        {OldestKey, new SortKeyItem(OldestKey, OldestName)},
                        {TitleKey, new SortKeyItem(TitleKey, TitleName)},
                        {TitleDescKey, new SortKeyItem(TitleDescKey, TitleDescName)}
                    };

                    return _items;
                }
            }
        }

        public static ISortKeyItem Latest => GetConfigItem(LatestKey);

        public static ISortKeyItem Oldest => GetConfigItem(OldestKey);

        public static ISortKeyItem Title => GetConfigItem(TitleKey);

        public static ISortKeyItem TitleDesc => GetConfigItem(TitleDescKey);

        private static ISortKeyItem GetConfigItem(string key)
        {
            return Items.ContainsKey(key) ? Items[key] : new SortKeyItem(key, string.Empty);
        }
    }
}