using Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Models
{
    public class NewsArticleSchema : ISeoSchema
    {
        [JsonProperty(PropertyName = "@context")]
        public string Context { get; } = SchemaContext.ForSchemaOrg;

        [JsonProperty(PropertyName = "@type")]
        public string Type { get; } = SchemaType.NewsArticle;

        public string InLanguage { get; set; }
        public string Headline { get; set; }
        public string DatePublished { get; set; }
        public string DateModified { get; set; }
        public string ArticleBody { get; set; }
        public MainEntityOnPage MainEntityOfPage { get; set; }
        public IContentContributor Author { get; set; }
        public IContentContributor Publisher { get; set; }
        public IEnumerable<ImageEntity> Images { get; set; }
    }
}
