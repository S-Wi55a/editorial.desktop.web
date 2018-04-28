using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.Shared.Models;

namespace Csn.Retail.Editorial.Web.Features.Details.Mappings
{
    public static class ArticleLabelHelper
    {
        public static string GetLabel(List<string> articleTypes)
        {
            if (articleTypes == null) return null;

            if (articleTypes.Any(x => x.Equals(ArticleType.Sponsored.ToString())))
            {
                return ArticleType.Sponsored.ToString();
            }

            if (articleTypes.Any(x => x.Equals(ArticleType.Carpool.ToString())))
            {
                return ArticleType.Carpool.ToString();
            }

            return null;
        }
    }
}