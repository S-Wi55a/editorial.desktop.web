using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.Shared.Models
{
    public enum RedbookVertical
    {
        Bikes,
        Cars,
        Boats
    }

    public static class RedbookVerticals
    {
        public static IDictionary<RedbookVertical, string> Items => new Dictionary<RedbookVertical, string>
        {
            { RedbookVertical.Bikes, "Bike"},
            { RedbookVertical.Boats, "Boat"},
            { RedbookVertical.Cars, "Car"},
        };
    }
}