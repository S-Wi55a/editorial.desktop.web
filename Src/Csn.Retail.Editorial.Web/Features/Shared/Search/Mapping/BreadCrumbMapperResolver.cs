using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Shared.Helpers;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping
{
    public class BreadCrumbMapperResolver : IValueResolver<RyvussNavDto, Nav.Nav, List<BreadCrumb>>
    {
        public List<BreadCrumb> Resolve(RyvussNavDto source, Nav.Nav destination, List<BreadCrumb> destMember, ResolutionContext context)
        {
            if (source?.BreadCrumbs == null || !source.BreadCrumbs.Any() )
            {
                return new List<BreadCrumb>();
            }

            var results = Mapper.Map<List<BreadCrumb>>(source.BreadCrumbs.Where(a => a.IsFacetBreadCrumb).Flatten(x => x.Children), opt =>
            {
                if (context.Items.TryGetValue("sortOrder", out var sortOrder))
                {
                    opt.Items["sortOrder"] = sortOrder;
                }
            });
            var keywordBreadCrumb = source.BreadCrumbs.FirstOrDefault(a => a.IsKeywordBreadCrumb);
            if (keywordBreadCrumb != null)
            {
                results.Insert(0, new BreadCrumb
                {
                    RemoveAction = ListingUrlHelper.GetPathAndQueryString(keywordBreadCrumb.RemoveAction, sortOrder: context.Items.TryGetValue("sortOrder", out var sortOrder)
                        ? sortOrder?.ToString()
                        : string.Empty, includeResultsSegment: true),
                    FacetDisplay = keywordBreadCrumb.Term.Trim('(', ')'),
                    Type = "KeywordBreadCrumb"
                });
            }
            results.Add(new BreadCrumb
            {
                RemoveAction = ListingUrlHelper.GetPathAndQueryString(sortOrder: context.Items.TryGetValue("sortOrder", out var sort)
                    ? sort?.ToString()
                    : string.Empty, includeResultsSegment: true),
                FacetDisplay = "Clear All",
                Type = "ClearAllBreadCrumb"
            });

            return results;
        }
    }

    public class BreadCrumbRemoveActoinResolver : IValueResolver<BreadCrumbDto, BreadCrumb, string>
    {
        public string Resolve(BreadCrumbDto source, BreadCrumb destination, string destMember, ResolutionContext context)
        {
            var sortOrder = context.Items.TryGetValue("sortOrder", out var sort) ? sort?.ToString() : string.Empty;
            return source.HasSeoLinks ? ListingUrlHelper.GetSeoUrl(source.Metadata.Seo.First(), sortOrder: sortOrder) :
                ListingUrlHelper.GetPathAndQueryString(string.IsNullOrEmpty(source.RemoveAction) ? string.Empty: source.RemoveAction, sortOrder: sortOrder, includeResultsSegment: true);
        }
    }
}