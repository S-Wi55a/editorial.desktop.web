using System;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;

namespace Csn.Retail.Editorial.Web.Features.Shared.Extensions
{
    public static class SearchResultDtoExtensions
    {
        public static string GetNumericId(this SearchResultDto searchResult)
        {
            if (string.IsNullOrEmpty(searchResult.Id))
            {
                return string.Empty;
            }

            var hyphen = searchResult.Id.LastIndexOf("-", StringComparison.InvariantCultureIgnoreCase);
            if (hyphen < 0)
            {
                return string.Empty;
            }

            var substr = searchResult.Id.Substring(hyphen + 1);

            if (string.IsNullOrEmpty(substr))
            {
                return string.Empty;
            }

            return int.TryParse(substr, out var sourceIdInt)
                ? sourceIdInt.ToString()
                : string.Empty;
        }
    }
}