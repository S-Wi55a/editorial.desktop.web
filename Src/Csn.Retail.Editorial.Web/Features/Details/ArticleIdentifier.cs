using System.Web.Mvc;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    [ModelBinder(typeof(ArticleIdentifierModelBinder))]
    public class ArticleIdentifier
    {
        public string Id { get; set; }
        public string PageName { get; set; }
    }
}