using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Errors;
using Csn.Retail.Editorial.Web.Features.Shared.GlobalSite;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Filters;
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
        [RedirectAttributeFilter]
        // GET: Details
        public async Task<ActionResult> Index(ArticleIdentifier articleIdentifier, bool __preview=false)
        {
            var dispatchedEvent = _eventDispatcher.DispatchAsync(new DetailsPageRequestEvent());
            
            var dispatchedQuery = _queryDispatcher.DispatchAsync<GetArticleQuery, GetArticleResponse>(new GetArticleQuery()
                {
                    Id = articleIdentifier.Id,
                    IsPreview = __preview
                });

            await Task.WhenAll(dispatchedEvent, dispatchedQuery);

            var response = dispatchedQuery.Result;

            if (response.ArticleViewModel != null)
            {
                return View("DefaultTemplate", response.ArticleViewModel);
            }

            var errorsController = DependencyResolver.Current.GetService<ErrorsController>();
            errorsController.ControllerContext = new ControllerContext(this.Request.RequestContext, errorsController);

            return await (response.HttpStatusCode == HttpStatusCode.NotFound ? errorsController.Error404() : errorsController.ErrorGeneric());
        }
    }

    public class DetailsPageRequestEvent : IEvent, IRequireGlobalSiteNav, IRequiredGoogleAnalyticsTrackingScript
    {
    }
}