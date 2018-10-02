using System.Web.Mvc;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    [ModelBinder(typeof(ArticleIdentifierV1ModelBinder))]
    public class ArticleIdentifierV1
    {
        public string NetworkId { get; set; }
        public string Slug { get; set; }
    }

    [ModelBinder(typeof(ArticleIdentifierV2ModelBinder))]
    public class ArticleIdentifierV2
    {
        public string NetworkId { get; set; }
    }
}