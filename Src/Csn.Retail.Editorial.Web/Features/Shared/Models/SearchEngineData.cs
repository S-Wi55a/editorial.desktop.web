namespace Csn.Retail.Editorial.Web.Features.Shared.Models
{
    public class SearchEngineData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Canonical { get; set; }
        public string Viewport { get; set; }
        public AlternateItems Alternate { get; set; }
        public string Keywords { get; set; }


        public class AlternateItems
        {
            public string Media { get; set; }
            public string Href { get; set; }
        }
    }
}