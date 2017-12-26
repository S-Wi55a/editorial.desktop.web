using System;
using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Landing.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Formatters;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Services;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Landing.Carousel
{
    [AutoBind]
    public class CarouselQueryHandler : IAsyncQueryHandler<CarouselQuery, CarouselQueryResponse>
    {
        private readonly IRyvussDataService _ryvussDataService;
        private readonly IMapper _mapper;

        public CarouselQueryHandler(IRyvussDataService ryvussDataService, IMapper mapper)
        {
            _ryvussDataService = ryvussDataService;
            _mapper = mapper;
        }

        public async Task<CarouselQueryResponse> HandleAsync(CarouselQuery query)
        {
            var results = await _ryvussDataService.GetResults(query.Q, query.Offset, query.Sort);

            if (results == null) return null;
            var carouselResults = _mapper.Map<NavResult>(results);
            return new CarouselQueryResponse
            {
                CarouselViewModel = new CarouselViewModel
                {
                    ArticleSetItems = carouselResults.SearchResults,
                    NextQuery = carouselResults.Count > (query.Offset + 7) ? $"/editorial/api/v1/carousel/{EditorialUrlFormatter.GetQueryParam(query.Q, query.Offset + 7, query.Sort, 7)}" : String.Empty
                }
            };
        }
    }
}