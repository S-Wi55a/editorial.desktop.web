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
            public static string Landing => "Landing";
            public static string ArticleType => "ArticleType";
            public static string RedbookHome => "RedbookHome";
            public static string RedbookResults => "RedbookResults";
            public static string ListingPagePreInternational => "ListingPagePreInternational";
            public static string ListingPage => "ListingPage";
            public static string ConfigBasedRedirect => "ConfigBasedRedirect";
            public static string UnknownRoute => "UnknownRoute";
            public static string Error => "Error";

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