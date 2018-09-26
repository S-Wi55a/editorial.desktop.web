using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Landing.Configurations.Providers;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs.Extended.Filters;

namespace Csn.Retail.Editorial.Web.Features.Landing
{
    [AutoBind]
    public class GetLandingQueryProcessFilter : IAsyncQueryProcessFilter<GetLandingQuery, GetLandingResponse>
    {
        private readonly IPageContextStore _contextStore;
        private readonly ILandingConfigProvider _landingConfigProvider;

        public GetLandingQueryProcessFilter(IPageContextStore contextStore, ILandingConfigProvider landingConfigProvider)
        {
            _contextStore = contextStore;
            _landingConfigProvider = landingConfigProvider;
        }

        public Task OnExecutingAsync(GetLandingQuery query)
        {
            return Task.CompletedTask;
        }

        public async Task OnExecutedAsync(GetLandingQuery query, GetLandingResponse result)
        {
            var configResults = query.Configuration ?? await _landingConfigProvider.LoadConfig("default");

            if (configResults == null) return;

            var landingPageContext = new LandingPageContext
            {
                Make = !string.IsNullOrEmpty(configResults.HeroAdSettings?.HeroMake) ? configResults.HeroAdSettings.HeroMake : string.Empty
            };

            _contextStore.Set(landingPageContext);
        }
    }
}