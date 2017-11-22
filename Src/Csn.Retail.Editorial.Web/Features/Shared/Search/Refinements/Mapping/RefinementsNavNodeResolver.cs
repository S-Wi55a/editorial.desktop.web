using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Extensions;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Refinements;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping
{
    public class RefinementsNavNodeResolver : IValueResolver<RyvussNavNodeDto, RefinementNavNode, NavNode>
    {
        public NavNode Resolve(RyvussNavNodeDto source, RefinementNavNode destination, NavNode destMember, ResolutionContext context)
        {
            return Mapper.Map<NavNode>(source.GetRefinements(), opt =>
            {
                if (context.Items.TryGetValue("sortOrder", out var sortOrder))
                {
                    opt.Items["sortOrder"] = sortOrder;
                }                    
            });
        }
    }
}