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
        private readonly IArticleDetailsCacheStore _articleDetailsCacheStore;

        public GetArticleQueryProcessFilter(IPageContextStore contextStore, IArticleDetailsCacheStore articleDetailsCacheStore)
        {
            _contextStore = contextStore;
            _articleDetailsCacheStore = articleDetailsCacheStore;
        }

        public Task OnExecutingAsync(GetArticleQuery query)
        {
            return Task.CompletedTask;
        }

        public async Task OnExecutedAsync(GetArticleQuery query, GetArticleResponse result)
        {
            var cachedArticle = await _articleDetailsCacheStore.GetAsync(query.Id);

            if (!cachedArticle.HasValue || cachedArticle.Value?.ArticleViewModel == null) return;

            var detailsPageContext = new DetailsPageContext
            {
                Items = cachedArticle.Value.ArticleViewModel.Items,
                Lifestyles = cachedArticle.Value.ArticleViewModel.Lifestyles,
                Categories = cachedArticle.Value.ArticleViewModel.Categories,
                Keywords = cachedArticle.Value.ArticleViewModel.Keywords,
                ArticleType = cachedArticle.Value.ArticleViewModel.ArticleType,
                ArticleTypes = cachedArticle.Value.ArticleViewModel.ArticleTypes
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