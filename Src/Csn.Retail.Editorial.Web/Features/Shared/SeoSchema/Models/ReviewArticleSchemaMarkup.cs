﻿using Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Shared;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Models
{
    public class ReviewArticleSchema : SeoSchemaMarkup
    {
        [JsonProperty(PropertyName = "@context")]
        public string Context { get; } = SchemaContext.ForSchemaOrg;

        [JsonProperty(PropertyName = "@type")]
        public string Type { get; } = SchemaType.ReviewArticle;

        public string Headline { get; set; }
        public string DatePublished { get; set; }
        public string DateModified { get; set; }
        public string ReviewBody { get; set; }
        public MainEntityOnPage MainEntityOfPage { get; set; }
        public About About { get; set; }
        public IContentContributor Author { get; set; }
        public IContentContributor Publisher { get; set; }
        public IEnumerable<Image> Image { get; set; }
        public List<ItemReviewed> ItemReviewed { get; set; }
        public List<ReviewRating> ReviewRating { get; set; }

    }
}