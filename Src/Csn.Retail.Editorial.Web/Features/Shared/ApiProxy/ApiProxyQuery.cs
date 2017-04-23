using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Shared.ApiProxy
{
    public class ApiProxyQuery : IQuery
    {
        public string Uri { get; set; }
    }
}