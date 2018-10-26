﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Csn.Retail.Editorial.Web
{
    public static class RouteNames
    {
        public static class Mvc
        {
            public static string TrackingRoute => "TrackingRoute";
            public static string DisplayAdsRoute => "DisplayAdsRoute";
            public static string NativeAdRoute => "NativeAdRoute";
            public static string MediaMotiveAdRoute => "MediaMotiveAdRoute";
            public static string LandingHome => "LandingHome";
            public static string DetailsV2 => "DetailsV2";
            public static string DetailsV1 => "DetailsV1";
            public static string LandingManufacturer => "LandingManufacturer";
            public static string DetailsLegacyUrls => "DetailsLegacyUrls";
            public static string ArticleType => "ArticleType";
            public static string RedbookHome => "RedbookHome";
            public static string RedbookResults => "RedbookResults";
            public static string ListingPagePreInternational => "ListingPagePreInternational";
            public static string ListingPage => "ListingPage";
            public static string LatamInherentListing => "LatamInherentListing";
            public static string UnknownRoute => "UnknownRoute";

        }

        public static class WebApi
        {
            public static string ApiProxy => "ApiProxy";
            public static string ApiCarousel => "ApiCarousel";
            public static string GetNav => "GetNav";
            public static string GetNavRefinements => "GetNavRefinements";
        }
    }
}