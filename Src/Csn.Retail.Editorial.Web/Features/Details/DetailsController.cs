﻿using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Details.Models;
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

            return View("DefaultTemplate", viewModel);
        }

        // Temp to mimic ajax request for Expert Ratings
        // In Prod need to null check if there is Expert ratings
        [Route("editorial/details/procon/{pageName:regex(^.*-\\d+/?$)}")]
        // GET: Details
        public async Task<ActionResult> ProCon(ArticleIdentifier articleIdentifier)
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

            return PartialView("Partials/Widgets/_EditorsRating", viewModel);
        }

    }
}

