using System.Text;
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

        public static string ToLowerAlphaNumericOnly(this string data)
        {
            if (string.IsNullOrEmpty(data))
                return data;

            var result = new StringBuilder();

            foreach (var chr in data)
            {
                if (char.IsLetter(chr))
                {
                    result.Append(char.ToLower(chr));
                }
                else if(char.IsNumber(chr))
                {
                    result.Append(chr);
                }
            }

            return result.ToString();
        }

        public static MvcHtmlString ToMvcHtmlString(this string source)
        {
            return string.IsNullOrWhiteSpace(source) ? MvcHtmlString.Empty : MvcHtmlString.Create(source);
        }

        public static string MakeUrlFriendly(this string data)
        {
            if (string.IsNullOrEmpty(data))
                return data;

            var result = new StringBuilder();
            var addHyphen = false;

            foreach (var chr in data.Trim())
            {
                if (char.IsLetterOrDigit(chr) || chr == '!')
                {
                    if (addHyphen)
                    {
                        result.Append('-');
                        addHyphen = false;
                    }

                    result.Append(char.ToLower(chr));
                }
                else
                {
                    addHyphen = true;
                }
            }
            return result.ToString().Trim('-');
        }
    }
}