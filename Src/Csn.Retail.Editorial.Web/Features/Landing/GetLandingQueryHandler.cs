using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Landing.Configurations;
using Csn.Retail.Editorial.Web.Features.Landing.Configurations.Providers;
using Csn.Retail.Editorial.Web.Features.Landing.Mappings;
using Csn.Retail.Editorial.Web.Features.Landing.Models;
using Csn.Retail.Editorial.Web.Features.Landing.Services;
using Csn.Retail.Editorial.Web.Features.Shared.Constants;
using Csn.Retail.Editorial.Web.Features.Shared.Formatters;
using Csn.Retail.Editorial.Web.Features.Shared.Mappers;
using Csn.Retail.Editorial.Web.Features.Shared.HeroAdUnit.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
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
        private readonly ISmartServiceClient _restClient;
        private readonly IPolarNativeAdsDataMapper _polarNativeAdsDataMapper;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly ISeoDataMapper _seoDataMapper;


        public GetLandingQueryHandler(IRyvussDataService ryvussDataService, ICarouselDataService carouselDataService, IMapper mapper, ILandingConfigProvider landingConfigProvider, 
            ISmartServiceClient restClient, IPolarNativeAdsDataMapper polarNativeAdsDataMapper, ITenantProvider<TenantInfo> tenantProvider,
            ISeoDataMapper seoDataMapper)
        {
            _ryvussDataService = ryvussDataService;
            _mapper = mapper;
            _landingConfigProvider = landingConfigProvider;
            _restClient = restClient;
            _polarNativeAdsDataMapper = polarNativeAdsDataMapper;
            _tenantProvider = tenantProvider;
            _seoDataMapper = seoDataMapper;
            _carouselDataService = carouselDataService;

        }

        public async Task<GetLandingResponse> HandleAsync(GetLandingQuery query)
        {
            var configResults = await _landingConfigProvider.LoadConfig("default"); //Need to setup types of filter on landing page e.g. Based on Make/Model/Year etc

            var ryvussResults = _ryvussDataService.GetNavAndResults(string.Empty, false);
            var searchResults = GetCarousels(configResults);
            var campaignAd = configResults.HeroAdSettings.HasHeroAd ? GetAdUnit() : Task.FromResult<CampaignAdResult>(null);
     
            await Task.WhenAll(ryvussResults, searchResults, campaignAd);

            if (ryvussResults.Result == null || searchResults.Result == null) return null;

            var navResults = _mapper.Map<NavResult>(ryvussResults.Result);

            navResults.INav.CurrentUrl = EditorialUrlFormatter.GetPathAndQueryString();

            return new GetLandingResponse
            {
                LandingViewModel = new LandingViewModel
                {
                    Nav = new Models.Nav
                    {
                        NavResults = navResults,
                        DisqusSource = _tenantProvider.Current().DisqusSource,
                    },
                    Title = _tenantProvider.Current().DefaultPageTitle,
                    Carousels = searchResults.Result,
                    CampaignAd = campaignAd.Result,
                    PolarNativeAdsData = _polarNativeAdsDataMapper.Map(ryvussResults.Result.INav.BreadCrumbs, MediaMotiveScriptAdTypes.EditorialHomePage),
                    InsightsData = LandingInsightsDataMapper.Map(),
                    SeoData = _seoDataMapper.MapLandingSeoData(ryvussResults.Result),
                    HeroTitle = configResults.HeroAdSettings.HeroTitle
                }
            };
        }

        private async Task<List<CarouselViewModel>> GetCarousels(LandingConfigurationSet landingCarousel)
        {
            var getCarouselTasks = landingCarousel.CarouselConfigurations.Select(carouselConfig => _carouselDataService.GetCarouselData(carouselConfig)).ToList();

            await Task.WhenAll(getCarouselTasks);

            return getCarouselTasks.Where(tasks => tasks.Result != null).Select(listofTask => listofTask.Result).ToList();
        }

        private async Task<CampaignAdResult> GetAdUnit()
        {
            return await _restClient.Service("api-showroom-promotions")
                .Path($"/v1/promotions/campaign?PromotionType=EditorialHomePage&Vertical={_tenantProvider.Current().Name}")
                .GetAsync<CampaignAdResult>()
                .ContinueWith(x => x.Result.Data);
        }
    }
}