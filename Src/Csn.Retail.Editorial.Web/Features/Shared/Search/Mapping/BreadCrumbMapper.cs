using System.Collections.Generic;
using System.Linq;
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

            var results = _mapper.Map<IList<BreadCrumb>>(source.Where(a => a.Type == "FacetBreadCrumb").Flatten(x => x.Children));
            var keywordBreadCrumb = source.FirstOrDefault(a => a.Type == "KeywordBreadCrumb");
            if (keywordBreadCrumb != null)
            {
                results.Insert(0, new BreadCrumb { RemoveAction = keywordBreadCrumb.RemoveAction, FacetDisplay = keywordBreadCrumb.Term.Trim('(', ')') });
            }
            results.Add(new BreadCrumb { RemoveAction = string.Empty, FacetDisplay = "Clear All" });

            return results;
        }
    }
}