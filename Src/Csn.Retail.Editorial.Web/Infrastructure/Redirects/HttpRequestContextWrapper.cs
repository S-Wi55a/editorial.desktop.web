using System;
using System.Collections.Specialized;
using System.Web;
using Bolt.Common.Extensions;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Infrastructure.Redirects
{
    public interface IRequestContextWrapper
    {
        bool IsSecure();

        NameValueCollection Headers { get; }

        NameValueCollection QueryString { get; }

        Uri Url { get; }
    }

    [AutoBindAsPerRequest]
    public class HttpRequestContextWrapper : IRequestContextWrapper
    {
        public bool IsSecure()
        {
            return HttpContext.Current.NullSafeGet(context =>
            {
                var isSecure = context.Request.IsSecureConnection;

                return isSecure || context.Request.Headers["IsSecure"].ToBoolean().NullSafe();
            });
        }

        public NameValueCollection Headers
        {
            get { return HttpContext.Current.NullSafeGet(x => x.Request.Headers); }
        }

        public NameValueCollection QueryString
        {
            get { return HttpContext.Current.NullSafeGet(x => x.Request.QueryString); }
        }

        public Uri Url { get { return HttpContext.Current.NullSafeGet(x => x.Request.Url); } }
    }
}