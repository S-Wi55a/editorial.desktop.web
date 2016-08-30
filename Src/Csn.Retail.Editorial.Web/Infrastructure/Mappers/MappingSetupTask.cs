using System.Collections.Generic;
using AutoMapper;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.StartUpTasks;

namespace Csn.Retail.Editorial.Web.Infrastructure.Mappers
{
    public interface IMappingSetupTask
    {
        void Run(IMapperConfigurationExpression cfg);
    }

    [AutoBind]
    public class RunMappingSetupTask : IStartUpTask
    {
        private readonly IEnumerable<IMappingSetupTask> _tasks;

        public RunMappingSetupTask(IEnumerable<IMappingSetupTask> tasks)
        {
            _tasks = tasks;
        }

        public void Run()
        {
            if (_tasks == null) return;

            Mapper.Initialize(cfg =>
            {
                foreach (var mappingSetupTask in _tasks)
                {
                    mappingSetupTask.Run(cfg);
                }
            });

            Mapper.AssertConfigurationIsValid();
        }
    }
}