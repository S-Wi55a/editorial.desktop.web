using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;

namespace Csn.Retail.Editorial.Web.Infrastructure.Extensions
{
    public static class SlugExtentions
    {
        public static string GetSlug(this SearchResultDto source)
        {
            // this is required for articles published from legacy CMS
            return !string.IsNullOrEmpty(source.Slug) ? source.Slug : $"{source.Headline.MakeUrlFriendly()}-{source.Id.Substring(7)}";
        }
    }
}