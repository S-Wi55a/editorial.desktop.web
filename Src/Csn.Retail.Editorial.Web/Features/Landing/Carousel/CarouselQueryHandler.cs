using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Landing.Services;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Landing.Carousel
{
    [AutoBind]
    public class CarouselQueryHandler : IAsyncQueryHandler<CarouselQuery, CarouselQueryResponse>
    {
        private readonly ICarouselDataService _carouselDataService;

        public CarouselQueryHandler(ICarouselDataService carouselDataService)
        {
            _carouselDataService = carouselDataService;
        }

        public async Task<CarouselQueryResponse> HandleAsync(CarouselQuery query)
        {
            var results = await _carouselDataService.GetCarouselData(query);

            return results == null ? null : new CarouselQueryResponse {CarouselViewModel = results};
        }
    }
}