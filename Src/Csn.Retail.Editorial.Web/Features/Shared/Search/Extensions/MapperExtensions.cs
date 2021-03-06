using System.Linq;
using System.Web.Mvc;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Culture;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
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

        public static string GetLabel(this SearchResultDto source)
        {
            if (source.ArticleTypes == null) return null;

            if (source.ArticleTypes.Any(x => x.Equals(ArticleType.Sponsored.ToString())))
            {
                return ArticleType.Sponsored.ToString();
            }

            return source.ArticleTypes.Any(x => x.Equals(ArticleType.Carpool.ToString())) ? ArticleType.Carpool.ToString() : null;
        }

        public static string GetDisqusArticleId(this SearchResultDto source)
        {
            var tenantProvider = DependencyResolver.Current.GetService<ITenantProvider<TenantInfo>>();

            return string.IsNullOrEmpty(tenantProvider.Current().DisqusSource) ? string.Empty : $"EDITORIAL-{source.Id?.Substring(7)}";
        }

        public static string GetDisplayName(this RyvussNavNodeDto source)
        {
            return source.Name == "Make" ? LanguageResourceValueProvider.GetValue(LanguageConstants.MakeModelNavLabel) : source.DisplayName;
        }
    }
}