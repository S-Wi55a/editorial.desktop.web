using Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Shared;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Models
{

    public class NewsArticleSchema : SeoSchemaMarkupBase
    {
        [JsonProperty(PropertyName = "@context")]
        public string Context { get; } = SchemaContext.ForSchemaOrg;

        [JsonProperty(PropertyName = "@type")]
        public string Type { get; } = SchemaType.NewsArticle;

        public string Headline { get; set; }
        public string DatePublished { get; set; }
        public string DateModified { get; set; }
        public string ArticleBody { get; set; }
        public MainEntityOnPage MainEntityOfPage { get; set; }
        public IContentContributor Author { get; set; }
        public IContentContributor Publisher { get; set; }
        public IEnumerable<Image> Image { get; set; }
    }
}
