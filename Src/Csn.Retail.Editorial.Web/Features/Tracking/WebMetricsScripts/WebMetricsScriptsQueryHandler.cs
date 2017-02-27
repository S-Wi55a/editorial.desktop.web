using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs;
using Csn.WebMetrics.Ext.Interfaces;

namespace Csn.Retail.Editorial.Web.Features.Tracking.WebMetricsScripts
{
    [AutoBind]
    public class WebMetricsScriptsQueryHandler : IQueryHandler<WebMetricsScriptsQuery, WebMetricsScriptsViewModel>
    {
        private readonly IWebMetricsTrackingScriptBuilder _scriptBuilder;
        private readonly HttpContextBase _httpContext;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public WebMetricsScriptsQueryHandler(IWebMetricsTrackingScriptBuilder scriptBuilder,
            HttpContextBase httpContext, ITenantProvider<TenantInfo> tenantProvider)
        {
            _scriptBuilder = scriptBuilder;
            _httpContext = httpContext;
            _tenantProvider = tenantProvider;
        }

        public WebMetricsScriptsViewModel Handle(WebMetricsScriptsQuery query)
        {
            string scriptBlock = _scriptBuilder.GetScriptBlock(_httpContext, new List<string> { _tenantProvider.Current().Name.Replace("sales", string.Empty).Replace("point", String.Empty), "sales" });
            return new WebMetricsScriptsViewModel { ScriptBlock = scriptBlock };
        }
    }
}