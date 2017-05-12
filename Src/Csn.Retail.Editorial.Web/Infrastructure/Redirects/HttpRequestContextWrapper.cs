using System;
using System.Collections.Specialized;
using System.Web;
using Bolt.Common.Extensions;

namespace Csn.Retail.Editorial.Web.Infrastructure.Redirects
{
    public interface IRequestContextWrapper
    {
        bool IsSecure();

        NameValueCollection Headers { get; }

        NameValueCollection QueryString { get; }

        Uri Url { get; }

        bool IsMobile();

        bool IsIpad();

        bool IsTouchDevice();
    }

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

        public bool IsMobile()
        {
            if (HttpContext.Current == null || HttpContext.Current.Request == null) return false;

            var request = HttpContext.Current.Request;

            bool isMobile = (request.Browser != null && request.Browser.IsMobileDevice);

            string userAgent = (!string.IsNullOrEmpty(request.UserAgent)) ? request.UserAgent.ToLower() : string.Empty;

            // Check for mobile devices, excluding iPad.
            if ((isMobile
                || userAgent.Contains("iphone")
                || userAgent.Contains("ipod")
                || userAgent.Contains("blackberry")
                || userAgent.Contains("mobile")
                || userAgent.Contains("windows ce")
                || userAgent.Contains("opera mini")
                || userAgent.Contains("palm"))
                && !userAgent.Contains("ipad"))
            {
                return true;
            }
            return false;
        }

        public bool IsIpad()
        {
            return HttpContext.Current.NullSafeGet(cxt => cxt
                        .Request
                        .UserAgent
                        .NullSafe()
                        .IndexOf("ipad", StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public bool IsTouchDevice()
        {
            if (HttpContext.Current == null || HttpContext.Current.Request == null) return false;

            var request = HttpContext.Current.Request;

            bool isMobile = (request.Browser != null && request.Browser.IsMobileDevice);

            string userAgent = (!string.IsNullOrEmpty(request.UserAgent)) ? request.UserAgent.ToLower() : string.Empty;

            if (isMobile
               || userAgent.Contains("iphone")
               || userAgent.Contains("ipod")
               || userAgent.Contains("blackberry")
               || userAgent.Contains("mobile")
               || userAgent.Contains("windows ce")
               || userAgent.Contains("opera mini")
               || userAgent.Contains("palm")
               || userAgent.Contains("ipad")
               || userAgent.Contains("touch"))
            {
                return true;
            }
            return false;
        }
    }
}