using System;
using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.Shared.Models
{
    public class EditorialSortKeyValues
    {
        private const string LatestKey = "Latest";
        private const string LatestName = "Latest Articles";
        private const string OldestKey = "Oldest";
        private const string OldestName = "Oldest Articles";

        public static IDictionary<string, ISortKeyItem> Items => new Dictionary<string, ISortKeyItem>
        {
            { LatestKey, new SortKeyItem(LatestKey, LatestName) },
            { OldestKey, new SortKeyItem(OldestKey, OldestName) }
        };
    }

    public interface ISortKeyItem
    {
        string DisplayName { get; }
        string Key { get; }
    }
    public class SortKeyItem : ISortKeyItem
    {
        public SortKeyItem(string key, string displayName)
        {
            DisplayName = displayName;
            Key = key;
        }

        public string DisplayName { get; }
        public string Key { get; }
    }
}