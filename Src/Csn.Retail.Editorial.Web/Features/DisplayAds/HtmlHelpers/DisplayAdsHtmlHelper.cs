using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.HtmlHelpers
{
    public static class DisplayAdsHtmlHelper
    {
        public static void RenderDisplayAds(this HtmlHelper html, DisplayAdsQuery query)
        {
            html.RenderAction("Index", "DisplayAds", query);
        }
    }
}