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

        // call this from the master template! Should not be called from any other pages!
        public static void RenderDisplayAdsHeader(this HtmlHelper html)
        {
            html.RenderAction("RenderHeader", "DisplayAds");
        }

        // call this from the master template! Should not be called from any other pages!
        public static void RenderDisplayAdsFooter(this HtmlHelper html)
        {
            // TODO: call krux partial view from here and add the html helper call to the master template (remove it from individual pages)
            html.RenderAction("RenderFooter", "DisplayAds");
        }

        public static void RenderAdsTracking(this HtmlHelper html, MediaMotiveModel model)
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