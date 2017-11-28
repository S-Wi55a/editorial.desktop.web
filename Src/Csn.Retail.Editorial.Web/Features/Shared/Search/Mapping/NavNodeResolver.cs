using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping
{
    public class NavNodeResolver : IValueResolver<RyvussNavDto, Nav.Nav, List<NavNode>>
    {
        public List<NavNode> Resolve(RyvussNavDto source, Nav.Nav destination, List<NavNode> destMember, ResolutionContext context)
        {
            return source.Nodes.Where(n => n.Type == "Aspect").Select(n => Mapper.Map<NavNode>(n, opt =>
            {
                if (context.Items.TryGetValue("sortOrder", out var sortOrder))
                {
                    opt.Items["sortOrder"] = sortOrder;
                }
            })).ToList();
        }
    }
}