using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Details;
using Csn.Retail.Editorial.Web.Features.Details.CacheStores;
using Csn.Retail.Editorial.Web.Features.Landing.Configurations.Providers;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.SimpleCqrs.Extended.Filters;

namespace Csn.Retail.Editorial.Web.Features.Landing
{
    public class GetLandingQueryProcessFilter : IAsyncQueryProcessFilter<GetArticleQuery, GetArticleResponse>
    {
        private readonly IPageContextStore _contextStore;
        private readonly ILandingConfigProvider _landingConfigProvider;

        public GetArticleQueryProcessFilter(IPageContextStore contextStore, ILandingConfigProvider landingConfigProvider)
        {
            _contextStore = contextStore;
            _landingConfigProvider = landingConfigProvider;
        }

        public Task OnExecutingAsync(GetArticleQuery query)
        {
            return Task.CompletedTask;
        }

        public async Task OnExecutedAsync(GetArticleQuery query, GetArticleResponse result)
        {
            //var configResults = query.Configuration ?? await _landingConfigProvider.LoadConfig("default");
            // TODO: Get manufacture configuration if its manufacture landing page.
            var configResults = _landingConfigProvider.LoadConfig("default");

            if (configResults == null) return;

            var landingPageContext = new LandingPageContext
            {
                Make = !string.IsNullOrEmpty(configResults.HeroAdSettings?.HeroMake) ? configResults.HeroAdSettings.HeroMake : string.Empty
            };

            _contextStore.Set(landingPageContext);
        }
    }
}