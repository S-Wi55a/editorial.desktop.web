using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Landing.Carousel;
using Csn.Retail.Editorial.Web.Features.Landing.Configurations;
using Csn.Retail.Editorial.Web.Features.Landing.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Formatters;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Services;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;

namespace Csn.Retail.Editorial.Web.Features.Landing.Services
{
    public interface ICarouselDataService
    {
        Task<CarouselViewModel> GetCarouselData(CarouselQuery query);
        Task<CarouselViewModel> GetCarouselData(LandingCarouselConfiguration carouselConfiguration);
    }

    [AutoBind]
    public class CarouselDataService : ICarouselDataService
    {
        private readonly IRyvussDataService _ryvussDataService;
        private readonly IMapper _mapper;

        public CarouselDataService(IRyvussDataService ryvussDataService, IMapper mapper)
        {
            _ryvussDataService = ryvussDataService;
            _mapper = mapper;
        }

        public async Task<CarouselViewModel> GetCarouselData(CarouselQuery query)
        {
            return await CarouselDataResult(query.Q, query.Sort, query.Offset);
        }

        public async Task<CarouselViewModel> GetCarouselData(LandingCarouselConfiguration carouselConfiguration)
        {
            if (carouselConfiguration.CarouselType == CarouselTypes.Driver)
            {
                return GetDriverCarouselData(carouselConfiguration);
            }
            var carouselViewModel = await CarouselDataResult(carouselConfiguration.Query, carouselConfiguration.Sort, 0);

            if (carouselViewModel == null) return null;

            carouselViewModel.HasMrec = carouselConfiguration.DisplayMrec;
            carouselViewModel.Title = carouselConfiguration.Title;
            carouselViewModel.ViewAllLink = $"/editorial{carouselConfiguration.ViewAll}";
            carouselViewModel.HasPolar = carouselConfiguration.DisplayPolar;
            return carouselViewModel;
        }

        private async Task<CarouselViewModel> CarouselDataResult(string query, string sort, int offset)
        {
            var result = await _ryvussDataService.GetResults(query, offset, sort);
            if (result == null) return null;
            var landingResults = _mapper.Map<NavResult>(result);
            return new CarouselViewModel
            {
                CarouselItems = landingResults.SearchResults,
                NextQuery = offset + landingResults.Count > 7 ? $"/editorial/api/v1/carousel/?{EditorialUrlFormatter.GetQueryParam(query, offset + 7, sort)}"
                    : string.Empty
            };
        }

        private CarouselViewModel GetDriverCarouselData(LandingCarouselConfiguration carouselConfiguration)
        {
            return new CarouselViewModel
            {
                CarouselItems = carouselConfiguration.CarouselItems.Select(a => _mapper.Map<SearchResult>(a)).ToList(),
                Title = carouselConfiguration.Title,
                HasMrec = carouselConfiguration.DisplayMrec,
                HasPolar = carouselConfiguration.DisplayPolar
            };
        }
    }
}