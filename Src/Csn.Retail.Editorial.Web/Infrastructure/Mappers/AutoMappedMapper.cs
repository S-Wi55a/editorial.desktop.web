using System;
using AutoMapper;

namespace Csn.Retail.Editorial.Web.Infrastructure.Mappers
{
    public interface IMapper
    {
        TOutput Map<TOutput>(object input);
        TOutput Map<TOutput>(object input, Action<IMappingOperationOptions> options);
    }

    public class AutoMappedMapper : IMapper
    {
        public TOutput Map<TOutput>(object input)
        {
            return Mapper.Map<TOutput>(input);
        }

        public TOutput Map<TOutput>(object input, Action<IMappingOperationOptions> options)
        {
            return Mapper.Map<TOutput>(input, options);
        }
    }
}