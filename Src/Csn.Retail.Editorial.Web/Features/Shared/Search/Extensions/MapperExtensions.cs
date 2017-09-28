using System.Linq;
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
            if (source.MetaData?.Refinement == null || !source.MetaData.Refinement.Any())
                return null;

            return source.MetaData.Refinement.First();
        }


        public static RefinementsNodeDto GetRefinements(this RyvussNavNodeDto source)
        {
            return source.MetaData?.Refinements?.FirstOrDefault();            
        }

        public static RefinementsNodeDto GetRefinements(this FacetNodeDto source)
        {
            return source.MetaData?.Refinements?.FirstOrDefault();
        }

        public static string GetParentExpression(this RefinementsNodeDto source)
        {
            return source.Metadata != null && source.Metadata.ParentExpression != null
                ? source.Metadata.ParentExpression.FirstOrDefault()
                : string.Empty;
        }
    }
}