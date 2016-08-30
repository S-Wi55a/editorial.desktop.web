using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Csn.Retail.Editorial.Web.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        private static readonly Regex RegexAlphaNumericOnly = new Regex("[^a-zA-Z0-9]",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static string AlphaNumericOnly(this string source)
        {
            return string.IsNullOrWhiteSpace(source) ? string.Empty : RegexAlphaNumericOnly.Replace(source, string.Empty);
        }

        public static MvcHtmlString ToMvcHtmlString(this string source)
        {
            return string.IsNullOrWhiteSpace(source) ? MvcHtmlString.Empty : MvcHtmlString.Create(source);
        }
    }
}