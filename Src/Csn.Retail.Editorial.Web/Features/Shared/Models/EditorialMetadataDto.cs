using System.IO.IsolatedStorage;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;

namespace Csn.Retail.Editorial.Web.Features.Shared.Models
{
    public class Metadata
    {
        // ReSharper disable once InconsistentNaming - needed for Ryvus response parsing
        public string query { get; set; }

        public string Seo { get; set; }
    }

    public class EditorialMetadataDto
    {
        public Metadata Metadata { get; set; }

        public bool HasSeo(string keyword = null)
        {
            return !Metadata.Seo.IsNullOrEmpty() && keyword.IsNullOrEmpty();
        }

        private bool HasNoData => (Metadata?.Seo == null && Metadata?.query == null);

        public string GetRedirectionUrl(string keyword, string offset, string sortOrder)
        {
            if (HasNoData)
                return "";

            var url= HasSeo(keyword)
                ? $"{Metadata.Seo}{GetQueryParameters(keyword, offset, sortOrder)}"
                : $"?Q={Metadata.query}{GetQueryParameters(keyword, offset, sortOrder)}";

            return url;
        }

        public string GetQueryParameters(string keyword, string offset, string sortOrder)
        {
            if (!HasSeo(keyword))
            {
                return (offset.IsNullOrEmpty() ? "" : $"&offset={offset}") +
                       (sortOrder.IsNullOrEmpty() ? "" : $"&sortOrder={sortOrder}") +
                       (keyword.IsNullOrEmpty() ? "" : $"&keyword={keyword}");
            }
            var queryParameters = (!offset.IsNullOrEmpty() || !sortOrder.IsNullOrEmpty() ? "?" : "") +
                          (offset.IsNullOrEmpty() ? "" : $"offset={offset}");
            if (sortOrder != "")
                queryParameters = queryParameters + (offset.IsNullOrEmpty() ? "" : "&") + $"sortOrder={sortOrder}";

            return queryParameters;
        }
    }
}