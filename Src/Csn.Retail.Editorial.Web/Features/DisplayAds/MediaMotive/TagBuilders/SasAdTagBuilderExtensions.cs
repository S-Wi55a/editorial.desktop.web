using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.MediaMotive.TagBuilders
{
    public class SasAdTagBuilderExtensions
    {
        private static readonly Regex DisallowedCharsForValueRegex;

        static SasAdTagBuilderExtensions()
        {
            DisallowedCharsForValueRegex = new Regex("[^a-zA-Z0-9,]*");
        }

        public static string Clean(string value)
        {
            return value == null ? null : DisallowedCharsForValueRegex.Replace(value, string.Empty).ToLower();
        }

        public static string JoinMakeModelMarketingGroup(string make, string model, string marketingGroup)
        {
            return Clean(make + model + marketingGroup);
        }

        private static readonly Dictionary<string, string> ArticleTypeToSasATypeDictionary =
            new Dictionary<string, string>
            {
                {"advice", "advice"},
                {"car advice", "advice"},
                {"engine review", "engine-reviews"},
                {"feature", "features"},
                {"finance", "news"},
                {"industry news", "news"},
                {"insurance", "news"},
                {"motorsport", "news"},
                {"motoracing", "motoracing"},
                {"news", "news"},
                {"product", "products"},
                {"recipe", "news"},
                {"review", "reviews"},
                {"riding advice", "riding-advice"},
                {"tips", "tips"},
                {"tow test", "tow-tests"},
                {"video", "videos"}
            };

        public static string GetArticleTypeValues(DetailsPageContext detailsPageContext)
        {
            if (detailsPageContext != null && detailsPageContext.ArticleTypes.Any(x =>
                    string.Equals("sponsored", x, StringComparison.OrdinalIgnoreCase)))
            {
                return "sponsored";
            }

            var articleType = detailsPageContext?.ArticleType;

            if (articleType == null)
                return null;

            string aType;
            return ArticleTypeToSasATypeDictionary.TryGetValue(articleType.ToLower(), out aType) &&
                   !string.IsNullOrEmpty(aType)
                ? aType
                : "news";
        }
    }
}