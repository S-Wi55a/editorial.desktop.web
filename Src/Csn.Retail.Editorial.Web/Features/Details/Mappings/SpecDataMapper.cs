using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Details.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Details.Mappings
{
    public interface ISpecDataMapper
    {
        SpecData Map(Shared.Proxies.EditorialApi.SpecData article);
    }

    [AutoBind]
    public class SpecDataMapper : ISpecDataMapper
    {

        public SpecData Map(Shared.Proxies.EditorialApi.SpecData specData)
        {
            if (specData == null)
            {
                return null;
            }

            return new SpecData()
            {
                MinLabel = specData.MinLabel,
                MaxLabel = specData.MaxLabel,
                MoreLabel = specData.MoreLabel,
                Items = specData.Items,
                Title1 = "Model Selector",
                Title2 = "Price Min",
                Title3 = "Price Max"
            };
        }
    }
}