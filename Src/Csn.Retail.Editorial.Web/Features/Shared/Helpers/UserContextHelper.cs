using System;
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

        public static string GetMemberTrackingId()
        {
            var userContext = DependencyResolver.Current.GetService<IUserContext>();

            return userContext.MemberTrackingId.HasValue && userContext.MemberTrackingId.Value != Guid.Empty ? userContext.MemberTrackingId.ToString() : string.Empty;
        }
    }
}