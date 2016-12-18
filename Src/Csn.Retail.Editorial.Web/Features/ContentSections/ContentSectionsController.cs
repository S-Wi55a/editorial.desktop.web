using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;

namespace Csn.Retail.Editorial.Web.Features.ContentSections
{
    public class ContentSectionsController : Controller
    {
        [ChildActionOnly, Route("contentsections")]
        public async Task<ActionResult> ContentSection(ContentSection contentSection)
        {
            // use a convention based lookup :-)
            return PartialView($"_{contentSection.SectionType}", contentSection);
        }
    }
}