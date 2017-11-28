using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Extensions;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Refinements.Mapping
{
    public class RefinementsNavNodeResolver : IValueResolver<RyvussNavNodeDto, NavNodeWithRefinements, RefinementsNavNode>
    {
        public RefinementsNavNode Resolve(RyvussNavNodeDto source, NavNodeWithRefinements destination, RefinementsNavNode destMember, ResolutionContext context)
        {
            return Mapper.Map<RefinementsNavNode>(source.GetRefinements(), opt =>
            {
                if (context.Items.TryGetValue("sortOrder", out var sortOrder))
                {
                    opt.Items["sortOrder"] = sortOrder;
                }                    
            });
        }
    }

    public class NodeRefinementsResolver : IValueResolver<RyvussNavDto, RefinementNav, List<NavNodeWithRefinements>>
    {
        public List<NavNodeWithRefinements> Resolve(RyvussNavDto source, RefinementNav destination, List<NavNodeWithRefinements> destMember, ResolutionContext context)
        {
            return source.Nodes.Where(n => n.Type == "Aspect").Select(n => Mapper.Map<NavNodeWithRefinements>(n, opt =>
            {
                if (context.Items.TryGetValue("sortOrder", out var sortOrder))
                {
                    opt.Items["sortOrder"] = sortOrder;
                }
            })).ToList();
        }
    }
}