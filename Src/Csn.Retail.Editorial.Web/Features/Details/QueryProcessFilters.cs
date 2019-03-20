using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs.Extended.Filters;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    [AutoBind]
    public class QueryProcessFilters : IAsyncQueryProcessFilter<GetArticleQuery, GetArticleResponse>, 
                                        IAsyncQueryProcessFilter<GetModalQuery, GetArticleResponse>
    {
        private readonly IPageContextStore _contextStore;

        public QueryProcessFilters(IPageContextStore contextStore)
        {
            _contextStore = contextStore;
        }

        #region GetArticleQuery
        public Task OnExecutingAsync(GetArticleQuery query)
        {
            return Task.CompletedTask;
        }

        public async Task OnExecutedAsync(GetArticleQuery query, GetArticleResponse result)
        {
            SetDetailsPageContext(result);
        }
#endregion

        #region GetModalQuery
        public Task OnExecutingAsync(GetModalQuery query)
        {
            return Task.CompletedTask;
        }

        public async Task OnExecutedAsync(GetModalQuery query, GetArticleResponse result)
        {
            SetDetailsPageContext(result);
        }
    #endregion

        private void SetDetailsPageContext(GetArticleResponse result)
        {
            if (result?.ArticleViewModel == null) return;

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
        }
    }
}