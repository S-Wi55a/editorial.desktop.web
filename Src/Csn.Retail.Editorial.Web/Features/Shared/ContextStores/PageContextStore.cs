using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.ContextStores;
using Csn.WebMetrics.Core.Model;
using Ingress.Web.Common.Abstracts;

namespace Csn.Retail.Editorial.Web.Features.Shared.ContextStores
{
    public interface IPageContextStore
    {
        IPageContext Get();
        void Set(IPageContext pageContext);
        bool Exists();
    }

    [AutoBind]
    public class PageContextStore : IPageContextStore
    {
        private readonly IContextStore _contextStore;

        public PageContextStore(IContextStore contextStore)
        {
            _contextStore = contextStore;
        }

        public IPageContext Get()
        {
            var result = _contextStore.Get(ContextStoreKeys.CurrentPageContext.ToString());

            return result as IPageContext;
        }

        public void Set(IPageContext search)
        {
            _contextStore.Set(ContextStoreKeys.CurrentPageContext.ToString(), search);
        }

        public bool Exists()
        {
            return _contextStore.Exists(ContextStoreKeys.CurrentPageContext.ToString());
        }
    }
}