using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;

namespace Csn.Retail.Editorial.Web.Features.Shared.HtmlHelpers
{
    public static class SettingsHtmlHelpers
    {
        public static VideosApiSettings VideosApiSettings(this HtmlHelper html)
        {
            return DependencyResolver.Current.GetService<VideosApiSettings>();
        }
    }
}