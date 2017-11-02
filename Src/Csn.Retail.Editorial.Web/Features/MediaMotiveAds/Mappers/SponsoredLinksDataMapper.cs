using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds.Mappers
{
    public static class SponsoredLinksDataMapper
    {

        public static SponsoredLinksModel Map(List<string> adUnits)
        {

            return new SponsoredLinksModel()
            {
                ShowSponsoredLinks = adUnits.Any(au => SposoredLinkedTiles.Tiles.Contains(au))
            };

        }
    }
}