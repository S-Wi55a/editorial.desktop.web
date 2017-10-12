using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;

namespace Csn.Retail.Editorial.Web.Features.Shared.HtmlHelpers
{
    public static class SettingsHtmlHelpers
    {
        private static VideosApiSettings _videosApiSettings;

        public static VideosApiSettings VideosApiSettings(this HtmlHelper html)
        {
            return _videosApiSettings ?? (_videosApiSettings = DependencyResolver.Current.GetService<VideosApiSettings>());
        }
    }
}