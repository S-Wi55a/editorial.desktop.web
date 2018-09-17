using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.DisplayAds;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds.TagBuilders
{
    [AutoBind]
    public class DetailsTagBuilder : IMediaMotiveTagBuilder
    {
        private readonly IPageContextStore _pageContextStore;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public DetailsTagBuilder(IPageContextStore pageContextStore, ITenantProvider<TenantInfo> tenantProvider)
        {
            _pageContextStore = pageContextStore;
            _tenantProvider = tenantProvider;
        }

        public IEnumerable<MediaMotiveTag> Build(MediaMotiveTagBuildersParams parameters)
        {
            var adTags = new List<MediaMotiveTag>();
            var pageContext = _pageContextStore.Get() as DetailsPageContext;
            var items = pageContext?.Items;

            if (items != null && items.Any())
            {
                var item = items.FirstOrDefault();
                if (item != null)
                {
                    adTags.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Car, SasAdTagValues.JoinMakeModelMarketingGroup(item.Make, item.Model, item.MarketingGroup)));
                    adTags.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Make, SasAdTagValues.CleanMakeModel(item.Make)));
                    adTags.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Model, SasAdTagValues.CleanMakeModel(item.Model)));
                }
            }

            // Move these sections outside the Items check, because if an article does not have any item, they won't get included in the commonttags
            adTags.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Lifestyle, SasAdTagValues.Clean(GetLifestyle(pageContext))));
            //adTags.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.ArticleType, SasAdTagValues.GetArticleTypeValues(pageContext)));
            adTags.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Category, SasAdTagValues.Clean(GetCategory(pageContext))));
            //adTags.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Keyword, SasAdTagValues.Clean(GetKeyword(pageContext))));
            adTags.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Area, GetAdArea()));

            return adTags;
        }

        public bool IsApplicable(MediaMotiveTagBuildersParams parameters)
        {
            return  _pageContextStore.Get().PageContextType == PageContextTypes.Details;
        }

        private string GetAdArea()
        {
            if (_tenantProvider.Current().Name == "bikesales"
                || _tenantProvider.Current().Name == "farmmachinerysales"
                || _tenantProvider.Current().Name == "constructionsales"
                || _tenantProvider.Current().Name == "trucksales")
            {
                return SasAdTagValues.IndustryDetailsPage;
            }

            return SasAdTagValues.DetailsPage;
        }

        //private static string GetKeyword(DetailsPageContext pageContext)
        //{
        //    // APPS-1086: the "kw" we are looking for is the (first) word within the list of Keywords of the article that have {}
        //    return pageContext.Keywords?.Split(',')
        //        .Select(w => w.Trim())
        //        .Where(w => w.StartsWith("{") && w.EndsWith("}"))
        //        .Select(w => w.TrimStart('{').TrimEnd('}'))
        //        .FirstOrDefault();
        //}

        private static string GetLifestyle(DetailsPageContext pageContext)
        {
            return pageContext.Lifestyles?.FirstOrDefault();
        }

        private static string GetCategory(DetailsPageContext pageContexte)
        {
            return !pageContexte.Categories.IsNullOrEmpty() ? string.Join(",", pageContexte.Categories) : string.Empty;
        }

        public class SasAdTagValues
        {
            public const string DetailsPage = "editorialdetails";
            public const string IndustryDetailsPage = "editorialdetails";
            public const string Homepage = "homepage";
            public const string ResultsHome = "results_home";
            public const string LearnerApproved = "lams";
            public const string Valuation = "valuation";

            private static readonly Regex DisallowedCharsForValueRegex;

            static SasAdTagValues()
            {
                DisallowedCharsForValueRegex = new Regex("[^a-zA-Z0-9,]*");
            }

            public static string Clean(string value)
            {
                return value == null ? null : DisallowedCharsForValueRegex.Replace(value, string.Empty).ToLower();
            }

            public static string CleanMakeModel(string value)
            {
                return Clean(value);
            }

            public static string JoinMakeModelMarketingGroup(string make, string model, string marketingGroup)
            {
                return Clean(make + model + marketingGroup);
            }

            private static readonly Dictionary<string, string> ArticleTypeToSasATypeDictionary = new Dictionary<string, string>
        {
            {"advice", "advice" },
            {"car advice", "advice" },
            {"engine review", "engine-reviews" },
            {"feature", "features" },
            {"finance", "news" },
            {"industry news", "news" },
            {"insurance", "news" },
            {"motorsport", "news" },
            {"motoracing", "motoracing" },
            {"news", "news"},
            {"product", "products" },
            {"recipe", "news" },
            {"review", "reviews" },
            {"riding advice", "riding-advice" },
            {"tips", "tips" },
            {"tow test", "tow-tests" },
            {"video", "videos" }
        };
            //public static string GetArticleTypeValues(DetailsPageContext pageContext)
            //{
            //    if (pageContext.ArticleTypes.Any(x => string.Equals("sponsored", x, StringComparison.OrdinalIgnoreCase)))
            //    {
            //        return "sponsored";
            //    }

            //    var articleType = pageContext.Type;

            //    if (articleType == null)
            //        return null;
            //    string aType;
            //    return ArticleTypeToSasATypeDictionary.TryGetValue(articleType.ToLower(), out aType) && !string.IsNullOrEmpty(aType) ? aType : "news";
            //}
        }
    }
}