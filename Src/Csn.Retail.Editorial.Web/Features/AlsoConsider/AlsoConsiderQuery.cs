using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.AlsoConsider
{
    public class AlsoConsiderQuery : IQuery
    {
        public string Uri { get; set; }
    }
}