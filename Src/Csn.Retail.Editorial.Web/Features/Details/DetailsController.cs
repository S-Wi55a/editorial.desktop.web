using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Details.Models;
using Csn.Retail.Editorial.Web.Features.Home;
using Csn.Retail.Editorial.Web.Features.Shared.GlobalSite;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    public class DetailsController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IEventDispatcher _eventDispatcher;

        public DetailsController(IQueryDispatcher queryDispatcher, IEventDispatcher eventDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _eventDispatcher = eventDispatcher;
        }

        [Route("editorial/details/{pageName:regex(^.*-\\d+/?$)}")]
        // GET: Details
        public async Task<ActionResult> Index(ArticleIdentifier articleIdentifier)
        {
            var dispatchedEvent = _eventDispatcher.DispatchAsync(new DetailsPageRequestEvent());
            
            var dispatchedQuery = _queryDispatcher.DispatchAsync<GetArticleQuery, ArticleViewModel>(new GetArticleQuery()
                {
                    Id = articleIdentifier.Id
                });

            await Task.WhenAll(dispatchedEvent, dispatchedQuery);

            var viewModel = dispatchedQuery.Result;

            // TODO: what happens if this is the result of bad api call? Maybe need more info in the query response?
            if (viewModel == null)
            {
                throw new HttpException(404, "Article not found");
            }

            return View("DefaultTemplate", viewModel);
        }

    }

    public class DetailsPageRequestEvent : IEvent, IRequireGlobalSiteNav
    {
    }
}

