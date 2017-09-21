using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;

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
    }
}