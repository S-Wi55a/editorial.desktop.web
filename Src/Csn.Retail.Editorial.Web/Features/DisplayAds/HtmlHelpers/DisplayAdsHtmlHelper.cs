using System.Web.Mvc;
using System.Web.Mvc.Html;
using Csn.Retail.Editorial.Web.Features.MediaMotiveAds.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.HtmlHelpers
{
    public static class DisplayAdsHtmlHelper
    {
        public static void RenderDisplayAd(this HtmlHelper html, DisplayAdPlacements displayAdPlacement)
        {
            html.RenderDisplayAd(new DisplayAdQuery() { AdPlacement = displayAdPlacement });
        }

        public static void RenderDisplayAd(this HtmlHelper html, DisplayAdQuery query)
        {
            html.RenderAction("RenderDisplayAd", "DisplayAds", query);
        }

        public static void RenderDisplayAdTracking(this HtmlHelper html)
        {
            html.RenderDisplayAd(
                new DisplayAdQuery()
                {
                    AdPlacement = DisplayAdPlacements.Tracking,
                    //Make = model != null && !model.Make.IsNullOrEmpty() ? model.Make : string.Empty
                }
            );
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