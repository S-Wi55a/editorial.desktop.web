using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.Shared.Models
{
    public enum Vertical
    {
        Bikes,
        Cars,
        Boats
    }

    public static class RedbookVerticals
    {
        public static IDictionary<Vertical, string> Items => new Dictionary<Vertical, string>
        {
            { Vertical.Bikes, "Bike"},
            { Vertical.Boats, "Boat"},
            { Vertical.Cars, "Car"},
        };
    }
}