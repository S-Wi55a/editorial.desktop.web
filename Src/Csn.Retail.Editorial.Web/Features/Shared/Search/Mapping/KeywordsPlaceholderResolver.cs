using System.Linq;
using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Shared.Formatters;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping
{
    public class KeywordsPlaceholderResolver : IValueResolver<RyvussNavResultDto, NavResult, string>
    {
        public string Resolve(RyvussNavResultDto source, NavResult destination, string destMember, ResolutionContext context)
        {
            var keywordNode = source.INav.Nodes.FirstOrDefault(n => n.Name == "Keywords");
            return ListingsUrlFormatter.GetQueryString(keywordNode != null ? keywordNode.QueryWithPlaceholder : string.Empty, sortOrder: context.Items.TryGetValue("sortOrder", out var sortOrder)
                ? sortOrder?.ToString()
                : string.Empty);
        }
    }
}