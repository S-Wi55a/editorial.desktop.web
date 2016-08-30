using System;

namespace Csn.Retail.Editorial.Web.Infrastructure.UserContext
{
    public interface IUserContext
    {
        Guid? CurrentUserId { get; }
    }
}