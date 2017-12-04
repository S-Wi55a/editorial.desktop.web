using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Expresso.Expressions;
using Expresso.Syntax;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Shared
{
    public interface IRefinementMapper
    {
        Refinement Map(FacetNodeDto source, string aspectName);
    }

    [AutoBindAsSingleton]
    public class RefinementMapper : IRefinementMapper
    {
        private readonly IExpressionFormatter _expressionFormatter;

        public RefinementMapper(IExpressionFormatter expressionFormatter)
        {
            _expressionFormatter = expressionFormatter;
        }

        public Refinement Map(FacetNodeDto source, string aspectName)
        {
            if (string.IsNullOrEmpty(aspectName) || source.MetaData?.RefineableAspects == null || !source.MetaData.RefineableAspects.Any())
            {
                return null;
            }

            var refineableAspect = source.MetaData.RefineableAspects.First();

            // if a child refinement has been selected on the facet then the facet expression will not be usable and
            // we have to construct it using the aspect name and facet value
            return new Refinement()
            {
                Aspect = refineableAspect.Name,
                ParentExpression = source.IsSelected ? _expressionFormatter.Format(new FacetExpression(aspectName, source.Value)) : source.Expression
            };
        }
    }
}