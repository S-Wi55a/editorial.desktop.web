﻿using System.Linq;
using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Shared.Helpers;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping
{
    public class KeywordsPlaceholderResolver<T> : IValueResolver<RyvussNavDto, T, string>
    {
        public string Resolve(RyvussNavDto source, T destination, string destMember, ResolutionContext context)
        {
            var keywordNode = source.Nodes.FirstOrDefault(n => n.Name == "Keywords");
            return ListingUrlHelper.GetPathAndQueryString(keywordNode != null ? keywordNode.QueryWithPlaceholder : string.Empty, sortOrder: context.Items.TryGetValue("sortOrder", out var sortOrder)
                ? sortOrder?.ToString()
                : string.Empty);
        }
    }
}