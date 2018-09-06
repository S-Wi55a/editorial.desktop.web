using System.Web.Mvc;
using System.Web.Mvc.Html;
using Csn.Retail.Editorial.Web.Features.MediaMotiveAds.Models;
using Csn.Retail.Editorial.Web.Features.Shared.HtmlHelpers;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.HtmlHelpers
{
    public static class DisplayAdsHtmlHelper
    {
        
        public static void RenderDisplayAds(this HtmlHelper html, DisplayAdsQuery query)
        {
            if (html.CurrentTenant().HasMediaMotive)
            {
                html.RenderAction("MediaMotive", "DisplayAds", query);
            }

            if (html.CurrentTenant().HasGoogleAds)
            {
                html.RenderAction("GoogleAds", "DisplayAds", query);
            }
            
        }

        public static void RenderTeAds(this HtmlHelper html)
        {
            if (html.CurrentTenant().HasMediaMotive)
            {
                html.RenderAction("MediaMotive", "DisplayAds", new DisplayAdsQuery() { AdType = DisplayAdsTypes.TEADS });
            }
        }

        public static void RenderTrackingAds(this HtmlHelper html, MediaMotiveModel model)
        {
            if (html.CurrentTenant().HasMediaMotive)
            {
                html.RenderAction("MediaMotive", "DisplayAds",
                    new DisplayAdsQuery()
                    {
                        AdType = DisplayAdsTypes.Tracking,
                        Make = model != null && !model.Make.IsNullOrEmpty() ? model.Make : string.Empty
                    });
            }
        }
    }
}