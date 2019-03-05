using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    public class GetArticleQuery : IQuery
    {
        public string Id { get; set; }
        public bool IsPreview { get; set; }
        public DisplayType DisplayType { get; set; }
        public string Source { get; set; }
    }

    public enum DisplayType
    {
        DetailsPage,
        DetailsModal
    }
}