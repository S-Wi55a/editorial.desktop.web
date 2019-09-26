using System;
using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.MediaMotive.TagBuilders
{
    public interface IListingsBreadCrumbTagBuilder
    {
        IEnumerable<MediaMotiveTag> BuildTags(RyvussNavResultDto navResult);
    }

    [AutoBind]
    public class ListingsBreadCrumbTagBuilder : IListingsBreadCrumbTagBuilder
    {
        // this is the list of aspects which we will process from the breadcrumbs
        private readonly Dictionary<string, string> _aspectToAdTagMapping = new Dictionary<string, string>()
        {
            { "Make", SasAdTags.SasAdTagKeys.Make },
            { "Model", SasAdTags.SasAdTagKeys.Model },
            { "Type", SasAdTags.SasAdTagKeys.ArticleType },
            { "PostType", SasAdTags.SasAdTagKeys.ArticleType },
            { "Lifestyle", SasAdTags.SasAdTagKeys.Lifestyle },
            { "Category", SasAdTags.SasAdTagKeys.Category },
            { "ItemCategory", SasAdTags.SasAdTagKeys.Category },
            { "MarketingGroup", SasAdTags.SasAdTagKeys.MarketingGroup }
        };

        public IEnumerable<MediaMotiveTag> BuildTags(RyvussNavResultDto navResult)
        {
            var results = new List<MediaMotiveTag>();
            var isAspectProcessed = new Dictionary<string, bool>();

            if (navResult?.INav?.BreadCrumbs == null || !navResult.INav.BreadCrumbs.Any()) return results;

            foreach (var breadcrumb in navResult.INav.BreadCrumbs)
            {
                if (breadcrumb.IsFacetBreadCrumb)
                {
                    // check if we have previously processed the same aspect...we only process an aspect once
                    if (isAspectProcessed.TryGetValue(breadcrumb.Aspect, out var isProcessed) && isProcessed)
                    {
                        continue;
                    }
                    isAspectProcessed.Add(breadcrumb.Aspect, true);
                    results.AddRange(GetTagsForBreadcrumbAndChildren(breadcrumb));
                }
                else if(breadcrumb.IsKeywordBreadCrumb)
                {
                    results.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Keyword, breadcrumb.Term.Replace(" ", String.Empty).Trim('(', ')')));
                }
            }
            return results;
        }

        private IEnumerable<MediaMotiveTag> GetTagsForBreadcrumbAndChildren(BreadCrumbDto breadCrumb)
        {
            var adTags = new List<MediaMotiveTag>();

            if (breadCrumb == null) return adTags;

            if (_aspectToAdTagMapping.TryGetValue(breadCrumb.Aspect, out var adTag))
            {
                adTags.Add(new MediaMotiveTag(adTag, breadCrumb.Facet.ToLowerAlphaNumericOnly()));
            }

            if (breadCrumb.Children != null)
            {
                // only process the first child by default...we may need to extend this at a future date
                adTags.AddRange(GetTagsForBreadcrumbAndChildren(breadCrumb.Children.FirstOrDefault()));
            }

            return adTags;
        }
    }
}