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

        public static string MakeUrlFriendly(this string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return data;
            }

            data = data.Replace("/", "-");
            // remove entities
            data = Regex.Replace(data, @"&\w+;", "");
            // remove anything that is not letters, numbers, dash, ! or space. Volkswagen up! requires !
            data = Regex.Replace(data, @"[^A-Za-z0-9!\-\s]", "");
            // remove any leading or trailing spaces left over
            data = data.Trim();
            // replace spaces with single dash
            data = Regex.Replace(data, @"\s+", "-");
            // if we end up with multiple dashes, collapse to single dash
            data = Regex.Replace(data, @"\-{2,}", "-");
            // make it all lower case
            data = data.ToLower();

            // remove trailing dash, if there is one
            if (data.EndsWith("-"))
            {
                data = data.Substring(0, data.Length - 1);
            }

            return data;
        }
    }
}