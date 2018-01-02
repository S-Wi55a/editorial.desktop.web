using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Landing.Configurations;
using Csn.Retail.Editorial.Web.Features.Landing.Configurations.Providers;
using Csn.Retail.Editorial.Web.Features.Landing.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Formatters;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Services;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Csn.SimpleCqrs;
using Expresso.Expressions;
using Expresso.Syntax;

namespace Csn.Retail.Editorial.Web.Features.Landing
{
    [AutoBind]
    public class GetLandingQueryHandler : IAsyncQueryHandler<GetLandingQuery, GetLandingResponse>
    {
        private readonly IRyvussDataService _ryvussDataService;
        private readonly IMapper _mapper;
        private readonly ILandingConfigProvider _landingConfigProvider;
        private readonly IExpressionFormatter _expressionFormatter;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public GetLandingQueryHandler(IRyvussDataService ryvussDataService, IMapper mapper, ILandingConfigProvider landingConfigProvider, IExpressionFormatter expressionFormatter, ITenantProvider<TenantInfo> tenantProvider)
        {
            _ryvussDataService = ryvussDataService;
            _mapper = mapper;
            _landingConfigProvider = landingConfigProvider;
            _expressionFormatter = expressionFormatter;
            _tenantProvider = tenantProvider;
        }

        public async Task<GetLandingResponse> HandleAsync(GetLandingQuery query)
        {
            var navResults = _ryvussDataService.GetNavAndResults(string.Empty, false);

            var configResults = await _landingConfigProvider.LoadConfig("default"); //Need to setup types of filter on landing page e.g. Based on Make/Model/Year etc

            var searchResults = GetCarousels(configResults);

            await Task.WhenAll(navResults, searchResults);

            return new GetLandingResponse
            {
                LandingViewModel = new LandingViewModel
                {
                    Nav = new Models.Nav
                    {
                        NavResults = _mapper.Map<NavResult>(navResults.Result)
                    },
                    Carousels = searchResults.Result
                }
            };
        }

        private async Task<List<CarouselViewModel>> GetCarousels(LandingConfigurationSet landingCarousel)
        {
            var getCarouselTasks = landingCarousel.Carousels.Select(GetCarouselData).ToList();

            await Task.WhenAll(getCarouselTasks);

            return getCarouselTasks.Select(listofTask => listofTask.Result).ToList();
        }

        private async Task<CarouselViewModel> GetCarouselData(LandingCarousel carousel)
        {
            var query = _expressionFormatter.Format(new FacetExpression(carousel.Aspect, carousel.Value) & new FacetExpression("Service", _tenantProvider.Current().Name));
            var result = await _ryvussDataService.GetResults(query, 0, carousel.Sort);
            if (result == null) return null;
            var landingResults = _mapper.Map<NavResult>(result);
            return new CarouselViewModel
            {
                HasMrec = carousel.DisplayMrec,
                CarouselItems = landingResults.SearchResults,
                Title = carousel.Title,
                ViewAllLink = $"/editorial{carousel.ViewAll}", //specific to article type
                NextQuery = landingResults.Count > 7
                    ? $"/editorial/api/v1/carousel/?{EditorialUrlFormatter.GetQueryParam(query, 7, carousel.Sort)}"
                    : string.Empty
            };
        }
    }
}