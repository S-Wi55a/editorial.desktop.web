using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.SiteNav
{
    public class SiteNavQuery : IQuery
    {
        public bool RefreshCache { get; set; }
    }
}