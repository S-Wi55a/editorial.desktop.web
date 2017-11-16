using System.IO.IsolatedStorage;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Expresso;

namespace Csn.Retail.Editorial.Web.Features.Shared.Models
{
    public class SeoMetadata
    {
        // ReSharper disable once InconsistentNaming - needed for Ryvus response parsing
        public string query { get; set; }

        public string Seo { get; set; }
    }

    public class EditorialSeoDto
    {
        public SeoMetadata Metadata { get; set; }

        public bool HasSeo()
        {
            return !Metadata.Seo.IsNullOrEmpty();
        }
        private bool HasNoData => (Metadata?.Seo == null && Metadata?.query == null);
    }
}