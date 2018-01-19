﻿using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds.HtmlHelpers
{
    public static class NativeAdHelper
    {
        public static void RenderNativeAd(this HtmlHelper html)
        {
            html.RenderAction("Index", "NativeAd");
        }
    }
}