using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.MoreArticles
{
    public class MoreArticlesQuery : IQuery
    {
        public string Uri { get; set; }
    }
}