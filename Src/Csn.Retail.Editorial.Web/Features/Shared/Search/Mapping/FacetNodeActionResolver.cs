using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Shared.Formatters;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping
{
    public class FacetNodeActionResolver : IValueResolver<FacetNodeDto, FacetNode, string>
    {
        public string Resolve(FacetNodeDto source, FacetNode destination, string destMember, ResolutionContext context)
        {
            return UrlParamsFormatter.GetParams(source.Action, 0, context.Items.TryGetValue("sortOrder", out var sortOrder) ? sortOrder?.ToString() : string.Empty, string.Empty);
        }
    }
}