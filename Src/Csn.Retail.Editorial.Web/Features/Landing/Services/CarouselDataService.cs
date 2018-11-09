using System.Linq;
using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Landing.Carousel;
using Csn.Retail.Editorial.Web.Features.Landing.Configurations;
using Csn.Retail.Editorial.Web.Features.Landing.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Services;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Csn.Retail.Editorial.Web.Infrastructure.Wrappers;
using NewRelic.Api.Agent;

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
        private readonly IUrlRouteHelper _urlRouteHelper;
        private readonly IEditorialRouteSettings _routeSettings;

        public CarouselDataService(IRyvussDataService ryvussDataService, IMapper mapper, IUrlRouteHelper urlRouteHelper, IEditorialRouteSettings routeSettings)
        {
            _ryvussDataService = ryvussDataService;
            _mapper = mapper;
            _urlRouteHelper = urlRouteHelper;
            _routeSettings = routeSettings;
        }

        public async Task<CarouselViewModel> GetCarouselData(CarouselQuery query)
        {
            return await CarouselDataResult(query.Q, query.Sort, query.Offset, query.Limit);
        }

        [Trace]
        public async Task<CarouselViewModel> GetCarouselData(LandingCarouselConfiguration carouselConfiguration)
        {
            if (carouselConfiguration.CarouselType == CarouselTypes.Driver)
            {
                return GetDriverCarouselData(carouselConfiguration);
            }
            var carouselViewModel = await CarouselDataResult(carouselConfiguration.Query, carouselConfiguration.Sort, carouselConfiguration.Offset ?? 0, carouselConfiguration.Limit ?? 20);

            if (carouselViewModel == null) return null;

            carouselViewModel.HasMrec = carouselConfiguration.DisplayMrec;
            carouselViewModel.Title = carouselConfiguration.Title;
            carouselViewModel.ViewAllLink = carouselConfiguration.ViewAll != null ? $"{_routeSettings.BasePath.TrimEnd('/')}{carouselConfiguration.ViewAll}": null;
            carouselViewModel.CarouselType = carouselConfiguration.CarouselType;
            carouselViewModel.PolarAds = carouselConfiguration.PolarAds;
            carouselViewModel.HasNativeAd = carouselConfiguration.DisplayNativeAd;
            return carouselViewModel;
        }   

        private async Task<CarouselViewModel> CarouselDataResult(string query, string sort, int offset, int limit)
        {
            var result = await _ryvussDataService.GetResults(query, offset, sort);
            if (result == null) return null;
            var landingResults = _mapper.Map<NavResult>(result);

            return new CarouselViewModel
            {
                CarouselItems = landingResults.SearchResults,
                NextQuery = landingResults.Count - offset > 7 && offset < limit ?
                                _urlRouteHelper.HttpRouteUrl(RouteNames.WebApi.ApiCarousel, new CarouselQuery { Q = query, Offset = offset + 7, Sort = sort, Limit = limit }) : string.Empty
            };
        }

        private CarouselViewModel GetDriverCarouselData(LandingCarouselConfiguration carouselConfiguration)
        {
            return new CarouselViewModel
            {
                CarouselItems = carouselConfiguration.CarouselItems.Select(a => _mapper.Map<SearchResult>(a)).ToList(),
                Title = carouselConfiguration.Title,
                HasMrec = carouselConfiguration.DisplayMrec,
                PolarAds = carouselConfiguration.PolarAds,
                CarouselType = carouselConfiguration.CarouselType,
                HasNativeAd = carouselConfiguration.DisplayNativeAd
        };
        }
    }
}