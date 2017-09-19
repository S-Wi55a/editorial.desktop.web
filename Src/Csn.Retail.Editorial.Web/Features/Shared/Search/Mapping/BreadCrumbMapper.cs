using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Ingress.Core.Attributes;

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
            if (source.Count < 1)
            {
                return new List<BreadCrumb>();
            }

            var results = _mapper.Map<IList<BreadCrumb>>(source);
            results.Add(new BreadCrumb { RemoveAction = string.Empty, FacetDisplay = "Clear All" });

            return results;
        }
    }
}