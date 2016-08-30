using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.StartUpTasks;

namespace Csn.Retail.Editorial.Web.Infrastructure.AssetMappers
{
    [AutoBind]
    public class InitAssetMapStartUpTask : IStartUpTask
    {
        private readonly IAssetMapProvider _provider;

        public InitAssetMapStartUpTask(IAssetMapProvider provider)
        {
            _provider = provider;
        }

        public void Run()
        {
            _provider.Init();
        }
    }
}