using System;
using System.Web;
using Bolt.Common.Extensions;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;

namespace Csn.Retail.Editorial.Web.Infrastructure.Wrappers
{
    public interface IRequestWrapper
    {
        Uri Url { get; }
        string CorrelationId { get; }
        string Origin { get; }
        string AkamaiEdgescape { get; }
        string GdprRegion { get; }
    }

    [AutoBindAsSingleton]
    public class HttpRequestWrapper : IRequestWrapper
    {
        public Uri Url => HttpContext.Current.Request.Url;
        private const string HeaderKey = "x-cid";
        public string CorrelationId
        {
            get
            {
                var httpContext = HttpContext.Current;

                if (httpContext == null) return Guid.NewGuid().ToShortId();

                if (httpContext.Items.Contains(HeaderKey))
                {
                    return httpContext.Items[HeaderKey].ToString();
                }

                var request = httpContext.Request;

                var cid = request.Headers[HeaderKey];

                if (cid.HasValue())
                {
                    httpContext.Items[HeaderKey] = cid;
                    return cid;
                }

                cid = Guid.NewGuid().ToShortId();

                httpContext.Items[HeaderKey] = cid;

                return cid;
            }
        }

        public string GdprRegion => HttpContext.Current?.Request.QueryString["gdprRegion"];

        private const string AkamaiEdgescapeHeader = "X-Akamai-Edgescape";
        public string AkamaiEdgescape => HttpContext.Current?.Request.Headers[AkamaiEdgescapeHeader];

        private const string OriginHeader = "x-appid";
        public string Origin => HttpContext.Current?.Request.Headers[OriginHeader];
    }
}