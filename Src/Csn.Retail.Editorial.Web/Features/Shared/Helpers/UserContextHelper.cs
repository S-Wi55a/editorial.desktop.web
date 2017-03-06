using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Infrastructure.UserContext;

namespace Csn.Retail.Editorial.Web.Features.Shared.Helpers
{
    public static class UserContextHelper
    {
        public static string GetMemberId()
        {
            var userContext = DependencyResolver.Current.GetService<IUserContext>();

            return userContext.CurrentUserId.ToString();
        }
    }
}