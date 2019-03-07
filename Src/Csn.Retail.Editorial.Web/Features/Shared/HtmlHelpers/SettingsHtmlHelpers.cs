using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;

namespace Csn.Retail.Editorial.Web.Features.Shared.HtmlHelpers
{
    public static class SettingsHtmlHelpers
    {
        private static VideosApiSettings _videosApiSettings;
        private static IEditorialRouteSettings _detailsModalSettings;

        public static VideosApiSettings VideosApiSettings(this HtmlHelper html)
        {
            return _videosApiSettings ?? (_videosApiSettings = DependencyResolver.Current.GetService<VideosApiSettings>());
        }

        public static IEditorialRouteSettings DetailsModalSettings(this HtmlHelper html)
        {
            return _detailsModalSettings ?? (_detailsModalSettings = DependencyResolver.Current.GetService<IEditorialRouteSettings>());
        }

        
    }
}