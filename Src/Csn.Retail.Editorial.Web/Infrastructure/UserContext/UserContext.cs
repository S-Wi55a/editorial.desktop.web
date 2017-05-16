using System;
using System.Web;
using Csn.Member.AuthenticationModule;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Infrastructure.UserContext
{
    [AutoBindAsSingleton]
    public class UserContext : IUserContext
    {
        public Guid? CurrentUserId => (HttpContext.Current?.User.Identity as UserIdentity)?.Id;
        public Guid? MemberTrackingId => (HttpContext.Current?.User.Identity as UserIdentity)?.TrackingId;
    }
}