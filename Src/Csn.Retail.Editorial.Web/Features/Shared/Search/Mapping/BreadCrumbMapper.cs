using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
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

            var results = _mapper.Map<IList<BreadCrumb>>(source);
            results.Add(new BreadCrumb { RemoveAction = string.Empty, FacetDisplay = "Clear All" });

            return results;
        }
    }
}