using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping
{
    public interface INavNodeMapper
    {
        IList<NavNode> GetNavNode(IList<RyvussNavNodeDto> source);
    }

    [AutoBind]
    public class NavNodeMapper : INavNodeMapper
    {
        private readonly IMapper _mapper;

        public NavNodeMapper(IMapper mapper)
        {
            _mapper = mapper;
        }
        public IList<NavNode> GetNavNode(IList<RyvussNavNodeDto> source)
        {
            return _mapper.Map<IList<NavNode>>(source.Where(n => n.Name == "Aspect"));
        }
    }
}