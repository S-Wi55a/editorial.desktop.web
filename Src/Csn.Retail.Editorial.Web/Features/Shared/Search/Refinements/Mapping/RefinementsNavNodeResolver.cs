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
}