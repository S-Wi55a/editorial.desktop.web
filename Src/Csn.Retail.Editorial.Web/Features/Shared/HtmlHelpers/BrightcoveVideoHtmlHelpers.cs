using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Details.Mappings;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;

namespace Csn.Retail.Editorial.Web.Features.Shared.HtmlHelpers
{
    public static class BrightcoveVideoHtmlHelpers
    {
        public static string GetBrightcoveIFrameUrl(this HtmlHelper htmlHelper, BrightcoveVideo brightcoveVideo, string networkId)
        {
            return IFrameBuilder().Build(brightcoveVideo, networkId);
        }

        private static IBrightcoveIFrameBuilder IFrameBuilder()
        {
            return DependencyResolver.Current.GetService<IBrightcoveIFrameBuilder>();
        }
    }
}