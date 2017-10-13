using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Listings.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Shared.Helpers
{
    public interface ISortingHelper
    {
        SortingViewModel GenerateSortByViewModel(IDictionary<string, ISortKeyItem> sortKeys, IDictionary<string, string> parameters, string currrentSort);
    }

    [AutoBind]
    public class SortingHelper : ISortingHelper
    {

        public SortingViewModel GenerateSortByViewModel(IDictionary<string, ISortKeyItem> sortKeys, IDictionary<string, string> parameters, string currrentSort)
        {
            throw new System.NotImplementedException();
        }
    }
}