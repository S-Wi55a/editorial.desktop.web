﻿using System.Linq;
using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Shared.Helpers;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping
{
    public class FacetNodeUrlResolver : IValueResolver<FacetNodeDto, FacetNode, string>
    {
        public string Resolve(FacetNodeDto source, FacetNode destination, string destMember, ResolutionContext context)
        {
            var sortOrder = context.Items.TryGetValue("sortOrder", out var result) ? result?.ToString() : string.Empty;

            return source.HasSeoLinks ? ListingUrlHelper.GetSeoUrl(source.MetaData.Seo.First(), sortOrder: sortOrder) : ListingUrlHelper.GetPathAndQueryString(source.Action, sortOrder: sortOrder);
        }
    }
    
    public class FacetNodeActionResolver : IValueResolver<FacetNodeDto, FacetNode, string>
    {
        public string Resolve(FacetNodeDto source, FacetNode destination, string destMember, ResolutionContext context)
        {
            var sortOrder = context.Items.TryGetValue("sortOrder", out var result) ? result?.ToString() : string.Empty;

            return  ListingUrlHelper.GetQueryString(source.HasSeoLinks ? source.MetaData.Seo.First(): source.Action, sortOrder);
        }
    }
}