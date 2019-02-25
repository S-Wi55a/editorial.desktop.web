using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Details.CacheStores;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs.Extended.Filters;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    [AutoBind]
    public class GetArticleQueryProcessFilter : IAsyncQueryProcessFilter<GetArticleQuery, GetArticleResponse>
    {
        private readonly IPageContextStore _contextStore;

        public GetArticleQueryProcessFilter(IPageContextStore contextStore)
        {
            _contextStore = contextStore;
        }

        public Task OnExecutingAsync(GetArticleQuery query)
        {
            return Task.CompletedTask;
        }

        public async Task OnExecutedAsync(GetArticleQuery query, GetArticleResponse result)
        {
            if(result?.ArticleViewModel == null) return;

            var detailsPageContext = new DetailsPageContext
            {
                Items = result.ArticleViewModel.Items,
                Lifestyles = result.ArticleViewModel.Lifestyles,
                Categories = result.ArticleViewModel.Categories,
                Keywords = result.ArticleViewModel.Keywords,
                ArticleType = result.ArticleViewModel.ArticleType,
                ArticleTypes = result.ArticleViewModel.ArticleTypes
            };

            _contextStore.Set(detailsPageContext);

            if (query.DisplayType == DisplayType.DetailsModal)
            {
                if(!result.ArticleViewModel.InsightsData.MetaData.ContainsKey("displayType")) result.ArticleViewModel.InsightsData.MetaData.Add("displayType", "modal");
                if(!result.ArticleViewModel.InsightsData.MetaData.ContainsKey("source")) result.ArticleViewModel.InsightsData.MetaData.Add("source", query.Source);
                result.ArticleViewModel.SocialMetaData = null;
                result.ArticleViewModel.StockListingData = null;
                result.ArticleViewModel.MoreArticleData = null;
                result.ArticleViewModel.SpecDataGetVariantsUrl = null;
                result.ArticleViewModel.DisqusData = null;
                result.ArticleViewModel.PolarNativeAdsData = null;
                foreach (var contributor in result.ArticleViewModel.Contributors)
                {
                    contributor.LinkUrl = null;
                }
            }
        }
    }
}