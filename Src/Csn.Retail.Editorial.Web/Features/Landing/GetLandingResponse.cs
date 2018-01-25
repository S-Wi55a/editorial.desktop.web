﻿using System.Net;
using Csn.Retail.Editorial.Web.Features.Landing.Models;

namespace Csn.Retail.Editorial.Web.Features.Landing
{
    public class GetLandingResponse
    {
        public LandingViewModel LandingViewModel { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}