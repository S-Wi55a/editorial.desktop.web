using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Landing.Configurations;
using Csn.Retail.Editorial.Web.Features.Landing.Configurations.Providers;
using Csn.Retail.Editorial.Web.Features.Landing.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Formatters;
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

        public GetLandingQueryHandler(IRyvussDataService ryvussDataService, IMapper mapper, ILandingConfigProvider landingConfigProvider, IExpressionFormatter expressionFormatter)
        {
            _ryvussDataService = ryvussDataService;
            _mapper = mapper;
            _landingConfigProvider = landingConfigProvider;
            _expressionFormatter = expressionFormatter;
        }

        public async Task<GetLandingResponse> HandleAsync(GetLandingQuery query)
        {
            var navResults = _ryvussDataService.GetNavAndResults(string.Empty, false);

            var configResults = await _landingConfigProvider.LoadConfig("default"); //Need to setup types of filter on landing page e.g. Based on Make/Model/Year etc

            var searchResults = GetArticleSets(configResults);

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

        private async Task<List<CarouselViewModel>> GetArticleSets(LandingConfigurationSet landingArticleSet)
        {
            var getArticleSetTasks = landingArticleSet.ArticleSets.Select(GetArticleSet).ToList();

            await Task.WhenAll(getArticleSetTasks);

            return getArticleSetTasks.Select(listofTask => listofTask.Result).ToList();
        }

        private async Task<CarouselViewModel> GetArticleSet(LandingArticleSet articleSet)
        {
            var query = _expressionFormatter.Format(new FacetExpression(articleSet.Aspect, articleSet.Value));
            var result = await _ryvussDataService.GetResults(query, 0, articleSet.Sort);
            if (result == null) return null;
            var landingResults = _mapper.Map<NavResult>(result);
            return new CarouselViewModel
            {
                HasMrec = articleSet.DisplayMrec,
                ArticleSetItems = landingResults.SearchResults,
                Title = articleSet.Title,
                ViewAllLink = $"/editorial/{articleSet.Value}/", //specific to article type
                NextQuery = landingResults.Count > 7
                    ? $"/editorial/api/v1/carousel/?{EditorialUrlFormatter.GetQueryParam(query, 0, articleSet.Sort, 7)}"
                    : string.Empty
            };
        }
    }
}