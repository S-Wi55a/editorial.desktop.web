using System;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Landing
{
    public class GetLandingQuery: IQuery
    {
        public Guid? PromotionId { get; set; }
    }
}