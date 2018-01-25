using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Shared.Services;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Nav
{
    [AutoBind]
    public class NavQueryHandler : IAsyncQueryHandler<NavQuery, NavResult>
    {
        
        private readonly IRyvussDataService _ryvussDataService;
        private readonly IMapper _mapper;

        public NavQueryHandler(IRyvussDataService ryvussDataService, IMapper mapper)
        {
            _ryvussDataService = ryvussDataService;
            _mapper = mapper;
        }

        public async Task<NavResult> HandleAsync(NavQuery query)
        {
            var resultData = await _ryvussDataService.GetNavAndResults(query.Q, false);

            if (resultData == null) return null;

            return new NavResult
            {
                Count = resultData.Count,
                INav = _mapper.Map<Nav>(resultData.INav, opt => { opt.Items["sortOrder"] = query.Sort; })
            };
        }
    }
}