using Bolt.Common.Extensions;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Details.Mappings
{
    public interface IBrightcoveIFrameBuilder
    {
        string Build(BrightcoveVideo video, string articleId);
    }

    [AutoBind]
    public class BrightcoveIFrameBuilder : IBrightcoveIFrameBuilder
    {
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly VideosApiSettings _videosApiSettings;

        private readonly string _brightcoveIFrameUrl = "https://players.brightcove.net/674523943001/{0}_default/index.html?videoId={1}&csnVideosApiUrl={2}&csnArticleId={3}&csnService={4}";

        public BrightcoveIFrameBuilder(ITenantProvider<TenantInfo> tenantProvider, VideosApiSettings videosApiSettings)
        {
            _tenantProvider = tenantProvider;
            _videosApiSettings = videosApiSettings;
        }

        public string Build(BrightcoveVideo video, string articleId)
        {
            return video == null ? null : _brightcoveIFrameUrl.FormatWith(video.PlayerId, video.VideoId, _videosApiSettings.Url, articleId, _tenantProvider.Current().Name);
        }
    }
}