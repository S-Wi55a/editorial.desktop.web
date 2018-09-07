using System.Web.Mvc;
using System.Web.Mvc.Html;
using Csn.Retail.Editorial.Web.Features.MediaMotiveAds.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.HtmlHelpers
{
    public static class DisplayAdsHtmlHelper
    {
        public static void RenderDisplayAd(this HtmlHelper html, DisplayAdsQuery query)
        {
            html.RenderAction("DisplayAds", "DisplayAds", query);
        }
        
        public static void RenderDisplayAdsHeader(this HtmlHelper html)
        {
            html.RenderAction("RenderDisplayAdsHeader", "DisplayAds");
        }
        
        public static void RenderDisplayAdsFooter(this HtmlHelper html)
        {
            html.RenderAction("RenderDisplayAdsFooter", "DisplayAds");
        }

        public static void RenderDisplayAdsTracking(this HtmlHelper html, MediaMotiveModel model)
        {
            html.RenderAction("DisplayAds", "DisplayAds",
                new DisplayAdsQuery()
                {
                    AdType = DisplayAdsTypes.Tracking,
                    Make = model != null && !model.Make.IsNullOrEmpty() ? model.Make : string.Empty
                });
        }
    }
}