using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    public class PreviewQuery : IQuery
    {
        public string Id { get; set; }
    }
}