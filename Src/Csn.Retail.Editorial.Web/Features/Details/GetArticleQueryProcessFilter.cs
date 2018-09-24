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
        private readonly IArticleViewModelCacheStore _articleViewModelCacheStore;

        public GetArticleQueryProcessFilter(IPageContextStore contextStore, IArticleViewModelCacheStore articleViewModelCacheStore)
        {
            _contextStore = contextStore;
            _articleViewModelCacheStore = articleViewModelCacheStore;
        }

        public Task OnExecutingAsync(GetArticleQuery query)
        {
            return Task.CompletedTask;
        }

        public async Task OnExecutedAsync(GetArticleQuery query, GetArticleResponse result)
        {
            var cachedArticle = await _articleViewModelCacheStore.GetAsync(query.Id);

            if (!cachedArticle.HasValue) return;

            var detailsPageContext = new DetailsPageContext
            {
                Items = cachedArticle.Value.Items,
                Lifestyles = cachedArticle.Value.Lifestyles,
                Categories = cachedArticle.Value.Categories,
                Keywords = cachedArticle.Value.Keywords,
                ArticleType = cachedArticle.Value.ArticleType,
                ArticleTypes = cachedArticle.Value.ArticleTypes
            };

            _contextStore.Set(detailsPageContext);
        }
    }
}