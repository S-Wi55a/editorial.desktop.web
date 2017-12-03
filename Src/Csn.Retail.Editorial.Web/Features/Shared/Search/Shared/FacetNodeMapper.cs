using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Shared
{
    public interface IFacetNodeMapper
    {
        FacetNode Map(FacetNodeDto source, string aspectName);
    }

    [AutoBindAsSingleton]
    public class FacetNodeMapper : IFacetNodeMapper
    {
        private readonly IMapper _mapper;
        private readonly IRefinementMapper _refinementMapper;

        public FacetNodeMapper(IMapper mapper, IRefinementMapper refinementMapper)
        {
            _mapper = mapper;
            _refinementMapper = refinementMapper;
        }

        public FacetNode Map(FacetNodeDto source, string aspectName)
        {
            var facetNode = _mapper.Map<FacetNode>(source);

            facetNode.Refinement = _refinementMapper.Map(source, aspectName);

            return facetNode;
        }
    }
}