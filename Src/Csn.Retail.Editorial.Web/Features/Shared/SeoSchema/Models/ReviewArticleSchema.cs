using Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Models
{
    public class ReviewArticleSchema : SeoSchemaBase
    {
        [JsonProperty(PropertyName = "@type")]
        public string Type { get; } = SchemaType.ReviewArticle;

        public string InLanguage { get; set; }
        public string Headline { get; set; }
        public string DatePublished { get; set; }
        public string DateModified { get; set; }
        public string ReviewBody { get; set; }
        public MainEntityOnPage MainEntityOfPage { get; set; }
        public About About { get; set; }
        public IContentContributor Author { get; set; }
        public IContentContributor Publisher { get; set; }
        public IEnumerable<ImageEntity> Image { get; set; }
        public IEnumerable<ItemReviewed> ItemReviewed { get; set; }
        public IEnumerable<ReviewRating> ReviewRating { get; set; }
    }
}