using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Landing.Configurations;
using Csn.Retail.Editorial.Web.Features.Landing.Configurations.Providers;
using Csn.Retail.Editorial.Web.Features.Landing.Models;
using Csn.Retail.Editorial.Web.Features.Landing.Services;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Services;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Csn.SimpleCqrs;
using Ingress.ServiceClient.Abstracts;

namespace Csn.Retail.Editorial.Web.Features.Landing
{
    [AutoBind]
    public class GetLandingQueryHandler : IAsyncQueryHandler<GetLandingQuery, GetLandingResponse>
    {
        private readonly IRyvussDataService _ryvussDataService;
        private readonly IMapper _mapper;
        private readonly ILandingConfigProvider _landingConfigProvider;
        private readonly ICarouselDataService _carouselDataService;
        private ISmartServiceClient _restClient;


        public GetLandingQueryHandler(IRyvussDataService ryvussDataService, ICarouselDataService carouselDataService, IMapper mapper, ILandingConfigProvider landingConfigProvider, ISmartServiceClient restClient)
        {
            _ryvussDataService = ryvussDataService;
            _mapper = mapper;
            _landingConfigProvider = landingConfigProvider;
            _restClient = restClient;
            _carouselDataService = carouselDataService;

    }

    public async Task<GetLandingResponse> HandleAsync(GetLandingQuery query)
        {
            var configResults = await _landingConfigProvider.LoadConfig("default"); //Need to setup types of filter on landing page e.g. Based on Make/Model/Year etc

            var navResults = _ryvussDataService.GetNavAndResults(string.Empty, false);
            var searchResults = GetCarousels(configResults);
            var campaignAd = configResults.HasHeroAddUnit ? GetAdUnit() : Task.FromResult<CampaignAdResult>(null);
     
            await Task.WhenAll(navResults, searchResults, campaignAd);

            return new GetLandingResponse
            {
                LandingViewModel = new LandingViewModel
                {
                    Nav = new Models.Nav
                    {
                        NavResults = _mapper.Map<NavResult>(navResults.Result)
                    },
                    Carousels = searchResults.Result,
                    CampaignAd = campaignAd.Result
                }
            };
        }

        private async Task<List<CarouselViewModel>> GetCarousels(LandingConfigurationSet landingCarousel)
        {
            var getCarouselTasks = landingCarousel.CarouselConfigurations.Select(carouselConfig => _carouselDataService.GetCarouselData(carouselConfig)).ToList();

            await Task.WhenAll(getCarouselTasks);

            return getCarouselTasks.Select(listofTask => listofTask.Result).ToList();
        }

        private async Task<CampaignAdResult> GetAdUnit()
        {
            return await _restClient.Service("api-showroom-promotions")
                .Path("/v1/promotions/carsales-homepage/campaign")
                .GetAsync<CampaignAdResult>()
                .ContinueWith(x => x.Result.Data);
        }
    }
}