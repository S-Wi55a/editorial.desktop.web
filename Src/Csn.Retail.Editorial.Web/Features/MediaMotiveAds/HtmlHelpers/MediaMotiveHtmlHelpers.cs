using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds.HtmlHelpers
{
    public static class MediaMotiveHtmlHelpers
    {
        public static void RenderMediaMotiveDetailsAd(this HtmlHelper html, MediaMotiveDetailsAdQuery query)
        {
            html.RenderAction("Index", "MediaMotiveDetailsAd", query);
        }
    }
}