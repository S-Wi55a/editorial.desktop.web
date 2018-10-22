using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Culture;

namespace Csn.Retail.Editorial.Web.Features.Shared.Models
{
    public static class EditorialSortKeyValues
    {
        private const string LatestKey = "Latest";
        private const string OldestKey = "Oldest";

        public static IDictionary<string, ISortKeyItem> Items => new Dictionary<string, ISortKeyItem>
        {
            { LatestKey, new SortKeyItem(LatestKey, LanguageResourceValueProvider.GetValue(LanguageConstants.LatestArticles)) },
            { OldestKey, new SortKeyItem(OldestKey, LanguageResourceValueProvider.GetValue(LanguageConstants.OldestArticles)) }
        };

        public static string ListingPageDefaultSort => Items[LatestKey].Key;

        public static bool IsValidSort(string sort)
        {
            return !string.IsNullOrEmpty(sort) && Items.ContainsKey(sort);
        }
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