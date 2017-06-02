using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Home;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.React
{
    public class ReactController : Controller
    {
        private readonly IEventDispatcher _eventDispatcher;

        public ReactController(IEventDispatcher eventDispatcher)
        {
            _eventDispatcher = eventDispatcher;
        }

        // GET: Home
        public async Task<ActionResult> Index()
        {
            await _eventDispatcher.DispatchAsync(new HomePageRequestEvent());

            return View();
        }
    }
}
