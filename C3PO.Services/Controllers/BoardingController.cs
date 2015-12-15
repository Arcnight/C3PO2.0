﻿using System;

//using System.Net;
using System.Net.Http;
//using System.Net.Http.Headers;

using System.Web.Http;
//using System.Web.Http.Description;

using Microsoft.Practices.ServiceLocation;

using C3PO.Data.Models;
using C3PO.Data.Interfaces;

using C3PO.Utilities;

namespace C3PO.Services.Controllers
{
    //[Route("boardings")]
    internal class BoardingController : ApiController
    {
        IServiceLocator _svcLocator;

        public BoardingController()
        {
            _svcLocator = ServiceLocator.Current;
        }

        [HttpGet]
        //[Route("{id:int}")]
        public HttpResponseMessage Get(int id)
        {
            return Web.CreateResponse(Request, _svcLocator.GetInstance<IBoardingRepository>().Get(id));
        }
    }
}
