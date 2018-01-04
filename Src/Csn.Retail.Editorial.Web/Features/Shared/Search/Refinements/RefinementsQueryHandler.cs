using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Shared.Services;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Refinements
{
    [AutoBind]
    public class RefinementsQueryHandler : IAsyncQueryHandler<RefinementsQuery, RefinementResult>
    {
        private readonly IRyvussDataService _ryvussDataService;
        private readonly IMapper _mapper;

        public RefinementsQueryHandler(IRyvussDataService ryvussDataService, IMapper mapper)
        {
            _ryvussDataService = ryvussDataService;
            _mapper = mapper;
        }

        public async Task<RefinementResult> HandleAsync(RefinementsQuery query)
        {
            var resultData = await _ryvussDataService.GetRefinements(query);

            if (resultData == null) return null;

            return new RefinementResult()
            {
                Count = resultData.Count,
                Nav = _mapper.Map<RefinementNav>(resultData.INav, opt => {opt.Items["sortOrder"] = query.Sort; })
            };
        }
    }
}