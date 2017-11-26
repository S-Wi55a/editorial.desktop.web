using System.Linq;
using Csn.Retail.Editorial.Web.Features.Shared.Formatters;
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

        public static Refinement GetRefinement(this FacetNodeDto source)
        {
            if (source.MetaData?.RefineableAspects == null || !source.MetaData.RefineableAspects.Any())
                return null;

            var refineableAspect = source.MetaData.RefineableAspects.First();

            return new Refinement()
            {
                Aspect = refineableAspect.Name,
                ParentExpression = source.Expression
            };
        }

        public static RefinementsNodeDto GetRefinements(this RyvussNavNodeDto source)
        {
            return source.MetaData?.Refinements?.FirstOrDefault();            
        }

        public static RefinementsNodeDto GetRefinements(this FacetNodeDto source)
        {
            if (source.IsSelected)
            {
                return new RefinementsNodeDto
                {
                    Facets = source.Refinements.Nodes.FirstOrDefault()?.Facets
                };
            }
            return null;
        }
        public static Refinement GetParentExpression(this RefinementsNodeDto source)
        {
            if(source.Metadata?.ParentExpression != null){
                
                return new Refinement {
                    ParentExpression =  source.Metadata.ParentExpression.FirstOrDefault(),
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

        public static string GetRemoveActionUrl(this BreadCrumbDto source)
        {
            return string.IsNullOrEmpty(source.RemoveAction)
                ? string.Empty
                : ListingsUrlFormatter.GetPathAndQueryString(source.RemoveAction);
        }
    }
}