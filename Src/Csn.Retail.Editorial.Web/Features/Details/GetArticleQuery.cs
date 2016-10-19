using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    public class GetArticleQuery : IQuery
    {
        public string Id { get; set; }
    }
}