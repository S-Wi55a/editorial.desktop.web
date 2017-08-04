using System;
using System.Web;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Infrastructure.Redirects
{
    public interface ICookieStore
    {
        string Get(string name);
        void Set(string name, string value, TimeSpan expiredAt, bool isVisibleToClientScript = false);
        bool Remove(string name);
    }

    [AutoBind]
    public class HttpCookieStore : ICookieStore
    {

        private HttpCookie GetCookie(string name)
        {
            return HttpContext.Current?.Request.Cookies[name];
        }

        private void SetCookie(HttpCookie cookie)
        {
            var cookies = HttpContext.Current?.Response.Cookies;

            if (cookies == null) return;

            cookies.Remove(cookie.Name);
            cookies.Add(cookie);
        }

        public string Get(string name)
        {
            return GetCookie(name)?.Value;
        }

        public void Set(string name, string value, TimeSpan expiredAt, bool isVisibleToClientScript = false)
        {
            var cookie = new HttpCookie(name)
            {
                Value = value,
                Name = name,
                Expires = DateTime.UtcNow.Add(expiredAt),
                HttpOnly = !isVisibleToClientScript
            };

            SetCookie(cookie);
        }

        public bool Remove(string name)
        {
            var cookie = GetCookie(name);

            if (cookie == null) return false;

            cookie.Expires = DateTime.UtcNow.AddYears(-1);

            SetCookie(cookie);

            return true;
        }
    }
}