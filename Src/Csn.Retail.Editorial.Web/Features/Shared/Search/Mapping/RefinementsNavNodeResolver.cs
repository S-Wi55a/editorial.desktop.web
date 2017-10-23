using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Extensions;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Refinements;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping
{
    public class RefinementsNavNodeResolver : IValueResolver<RyvussNavNodeDto, RefinementResult, RefinementNavNode>
    {
        public RefinementNavNode Resolve(RyvussNavNodeDto source, RefinementResult destination, RefinementNavNode destMember, ResolutionContext context)
        {
            return Mapper.Map<RefinementNavNode>(source.GetRefinements(), opt =>
            {
                if (context.Items.TryGetValue("sortOrder", out var sortOrder))
                {
                    opt.Items["sortOrder"] = sortOrder;
                }                    
            });
        }
    }
}