﻿using System.Web.Mvc;

namespace Csn.Retail.Editorial.Web.Infrastructure.RequestWrapper
{
    public static class RequestWrapperExtensions
    {
        public static IRequestWrapper RequestWrapper(this HtmlHelper html)
        {
            return DependencyResolver.Current.GetService<IRequestWrapper>();
        }
    }
}