using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.Shared.Formatters;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping
{
    public interface IBreadCrumbMapper
    {
        IList<BreadCrumb> GetAggregatedBreadCrumbs(ICollection<BreadCrumbDto> source);
        string GetRemoveActionUrl(BreadCrumbDto source);
    }

    [AutoBind]
    public class BreadCrumbMapper : IBreadCrumbMapper
    {
        private readonly IMapper _mapper;

        public BreadCrumbMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IList<BreadCrumb> GetAggregatedBreadCrumbs(ICollection<BreadCrumbDto> source)
        {
            if (source == null || !source.Any())
            {
                return new List<BreadCrumb>();
            }

            var results =
                _mapper.Map<IList<BreadCrumb>>(source.Where(a => a.IsFacetBreadCrumb).Flatten(x => x.Children));
            var keywordBreadCrumb = source.FirstOrDefault(a => a.IsKeywordBreadCrumb);
            if (keywordBreadCrumb != null)
            {
                results.Insert(0, new BreadCrumb
                {
                    RemoveAction = ListingsUrlFormatter.GetQueryString(keywordBreadCrumb.RemoveAction, 0, "", ""),
                    FacetDisplay = keywordBreadCrumb.Term.Trim('(', ')'),
                    Type = "KeywordBreadCrumb"
                });
            }
            results.Add(new BreadCrumb
            {
                RemoveAction = string.Empty,
                FacetDisplay = "Clear All",
                Type = "ClearAllBreadCrumb"
            });

            return results;
        }

        public string GetRemoveActionUrl(BreadCrumbDto source)
        {
            return string.IsNullOrEmpty(source.RemoveAction)
                ? string.Empty
                : ListingsUrlFormatter.GetQueryString(source.RemoveAction, 0, "", "");
        }
    }
}