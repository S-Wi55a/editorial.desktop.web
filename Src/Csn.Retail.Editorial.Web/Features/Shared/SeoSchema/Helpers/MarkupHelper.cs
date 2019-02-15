using System.Text.RegularExpressions;

namespace Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Helpers
{
    public class MarkupHelper
    {
        public static string RemoveHTMLTags(string HTMLCode)
        {
            return Regex.Replace(
              HTMLCode, "<[^>]*>", "");
        }

        public static string GenerateFullUrlPath(string siteDomain, string path)
        {
            return $"https://{siteDomain}{path}";
        }

    }
} 