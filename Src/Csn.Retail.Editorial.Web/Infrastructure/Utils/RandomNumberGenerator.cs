using System;

namespace Csn.Retail.Editorial.Web.Infrastructure.Utils
{

    public static class RandomNumberGenerator
    {
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks % 1000000000);

        public static int Generate()
        {
            return Random.Next(100000000, 999999999);
        }
    }
}