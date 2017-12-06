using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Shared
{
    public class FacetNodeResolver : IValueResolver<RyvussNavNodeDto, NavNode, List<FacetNode>>
    {
        private static IRefinementMapper _refinementMapper;

        public List<FacetNode> Resolve(RyvussNavNodeDto source, NavNode destination, List<FacetNode> destMember, ResolutionContext context)
        {
            if (source?.Facets == null || !source.Facets.Any()) return null;

            if (_refinementMapper == null)
            {
                // I love implementing anti-patterns!!
                // ....do this for now until we figure out how to do dependency injection properly :-)
                _refinementMapper = DependencyResolver.Current.GetService(typeof(IRefinementMapper)) as IRefinementMapper;
            }

            return source.Facets.Select(f => MapFacetNode(f, source.Name, context)).ToList();
        }

        private FacetNode MapFacetNode(FacetNodeDto facetNodeDto, string aspectName, ResolutionContext context)
        {
            var facetNode = Mapper.Map<FacetNode>(facetNodeDto, opt =>
            {
                if (context.Items.TryGetValue("sortOrder", out var sortOrder))
                {
                    opt.Items["sortOrder"] = sortOrder;
                }
            });

            facetNode.Refinement = _refinementMapper.Map(facetNodeDto, aspectName);

            return facetNode;
        }
    }
}