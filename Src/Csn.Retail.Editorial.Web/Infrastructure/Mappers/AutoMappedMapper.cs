namespace Csn.Retail.Editorial.Web.Infrastructure.Mappers
{
    public interface IMapper
    {
        TOutput Map<TOutput>(object input);
    }

    public class AutoMappedMapper : IMapper
    {
        public TOutput Map<TOutput>(object input)
        {
            return AutoMapper.Mapper.Map<TOutput>(input);
        }
    }
}