using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.HtmlHelpers
{
    public static class DisplayAdsHtmlHelper
    {
        public static void RenderDisplayAd(this HtmlHelper html, DisplayAdPlacements displayAdPlacement)
        {
            html.RenderAction("RenderDisplayAd", "DisplayAds", new DisplayAdQuery() { AdPlacement = displayAdPlacement });
        }
        
        public static void RenderDisplayAdsHeader(this HtmlHelper html)
        {
            html.RenderAction("RenderDisplayAdsHeader", "DisplayAds");
        }
        
        public static void RenderDisplayAdsFooter(this HtmlHelper html)
        {
            html.RenderAction("RenderDisplayAdsFooter", "DisplayAds");
        }
    }
}