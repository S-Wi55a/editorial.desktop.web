using System.Linq;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Extensions
{
    public static class MapperExtensions
    {
        public static bool IsRefineable(this FacetNodeDto source)
        {
            if (source.MetaData?.IsRefineable == null || !source.MetaData.IsRefineable.Any())
                return false;

            return source.MetaData.IsRefineable.First();
        }

        public static RyvussNavNodeDto GetRefinements(this RyvussNavNodeDto source)
        {
            return source.MetaData?.Refinements?.FirstOrDefault();          
        }

        public static RyvussNavNodeDto GetRefinements(this FacetNodeDto source)
        {
            var refinements = source.MetaData?.Refinements?.FirstOrDefault();

            if (refinements != null) return refinements;

            if (source.IsSelected)
            {
                return new RyvussNavNodeDto
                {
                    Facets = source.Refinements.Nodes.FirstOrDefault()?.Facets
                };
            }

            return null;
        }

        public static Refinement GetParentExpression(this RyvussNavNodeDto source)
        {
            if(source.MetaData?.ParentExpression != null){
                return new Refinement
                {
                    ParentExpression = source.MetaData.ParentExpression.FirstOrDefault(),
                    Aspect = source.Name
                };
            }

            return new Refinement();
        }

        public static string GetSponsoredLabel(this SearchResultDto source)
        {
            return source.ArticleTypes?.FirstOrDefault(x => x.Equals(ArticleType.Sponsored.ToString()));
        }

        public static string GetDisqusArticleId(this SearchResultDto source)
        {
            return $"EDITORIAL-{source.Id?.Substring(7)}";
        }

        public static string GetDisplayName(this RyvussNavNodeDto source)
        {
            return source.DisplayName == "Make" ? "Make/Model" : source.DisplayName;
        }
    }
}