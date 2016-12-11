using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    public class DetailsController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public DetailsController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [Route("editorial/details/{pageName:regex(^.*-\\d+/?$)}")]
        // GET: Details
        public async Task<ActionResult> Index(ArticleIdentifier articleIdentifier)
        {
            var viewModel =
                await _queryDispatcher.DispatchAsync<GetArticleQuery, ArticleViewModel>(new GetArticleQuery()
                {
                    Id = articleIdentifier.Id
                });

            // TODO: add error handling
            //if (viewModel == null)
            //{
            //    return Redirect("~/error");
            //}

            // load up the appropriate view depending on the template type
            if (viewModel.ArticleTemplateType == ArticleTemplateType.NarrowHero)
            {
                return View("NarrowHeroTemplate", viewModel);
            }

            return View("DefaultTemplate", viewModel);
        }
    }
}