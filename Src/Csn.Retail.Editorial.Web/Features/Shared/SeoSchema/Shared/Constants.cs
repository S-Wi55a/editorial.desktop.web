namespace Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Shared
{
    public static class SchemaContext
    {
        public static string ForSchemaOrg = "http://schema.org";
    }

    public static class ReviewRatingValues
    {
        public static int OverallBestRating = 100;
        public static int OverallWorstRating = 0;

        public static int AttributeBestRating = 20;
        public static int AttributeWorstRating = 0;
    }

    public static class SchemaType
    {
        public static string Webpage = "Webpage";
        public static string NewsArticle = "NewsArticle";
        public static string ReviewArticle = "Review";
        public static string Brand = "Brand";
        public static string Person = "Person";
        public static string Image = "ImageObject";
        public static string Organization = "Organization";
        public static string Rating = "Rating";
        public static string Thing = "Thing";
    }
}
