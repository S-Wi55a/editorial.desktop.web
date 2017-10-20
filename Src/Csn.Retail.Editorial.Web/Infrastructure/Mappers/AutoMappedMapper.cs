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
        private Action<IMappingOperationOptions> _options;
        public TOutput Map<TOutput>(object input)
        {
            if(_options != null)
            { return Mapper.Map<TOutput>(input, _options);}
            return Mapper.Map<TOutput>(input);
        }

        public TOutput Map<TOutput>(object input, Action<IMappingOperationOptions> options)
        {
            _options = options;
            return Mapper.Map<TOutput>(input, options);
        }
    }
}