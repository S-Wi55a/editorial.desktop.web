using System.Linq;
using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Shared.Formatters;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping
{
    public class KeywordsPlaceholderResolver : IValueResolver<RyvussNavDto, Nav.Nav, string>
    {
        public string Resolve(RyvussNavDto source, Nav.Nav destination, string destMember, ResolutionContext context)
        {
            var keywordNode = source.Nodes.FirstOrDefault(n => n.Name == "Keywords");
            return ListingsUrlFormatter.GetPathAndQueryString(keywordNode != null ? keywordNode.QueryWithPlaceholder : string.Empty, sortOrder: context.Items.TryGetValue("sortOrder", out var sortOrder)
                ? sortOrder?.ToString()
                : string.Empty);
        }
    }
}