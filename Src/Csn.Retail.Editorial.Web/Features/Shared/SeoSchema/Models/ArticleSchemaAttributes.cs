using Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Shared;
using Newtonsoft.Json;

namespace Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Models
{    
    public interface IContentContributor{}

    public class Author : IContentContributor
    {
        [JsonProperty(PropertyName = "@type")]
        public string Type { get; } = SchemaType.Person;
        public string Name { get; set; }
        public string Url { get; set; }
        public ImageEntity Image { get; set; }
    }

    public class Publisher : IContentContributor
    {
        [JsonProperty(PropertyName = "@type")]
        public string Type { get; } = SchemaType.Organisation;
        public string Name { get; set; }
        public Logo Logo { get; set; }
    }

    public class ReviewRating
    {
        [JsonProperty(PropertyName = "@type")]
        public string ItemReviewed { get; set; } = SchemaType.Rating;
        public string ReviewAspect { get; set; }
        public int RatingValue { get; set; }
        public int BestRating { get; set; }
        public int WorstRating { get; set; }
    }

    public class ItemReviewed
    {
        [JsonProperty(PropertyName = "@type")]
        public string Type { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string ModelDate { get; set; }
        public Brand Brand { get; set; }
    }

    public class MainEntityOnPage
    {
        [JsonProperty(PropertyName = "@type")]
        public string Type { get; set; } = SchemaType.Webpage;

        [JsonProperty(PropertyName = "@id")]
        public string Id { get; set; }
    }

    public class ImageEntity
    {
        [JsonProperty(PropertyName = "@type")]
        public string Type { get; set; } = SchemaType.Image;
        public string Url { get; set; }
    }

    public class Logo
    {
        [JsonProperty(PropertyName = "@type")]
        public string Type { get; set; } = SchemaType.Image;
        public string Url { get; set; }
    }

    public class About
    {
        [JsonProperty(PropertyName = "@type")]
        public string Type { get; } = SchemaType.Thing;
        public string Name { get; set; }
    }

    public class Brand
    {
        [JsonProperty(PropertyName = "@type")]
        public string Type { get; set; } = SchemaType.Brand;
        public string Name { get; set; }
    }

}