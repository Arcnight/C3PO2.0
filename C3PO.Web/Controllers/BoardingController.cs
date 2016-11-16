using System;

//using System.Net;
using System.Net.Http;
//using System.Net.Http.Headers;

using System.Web.Http;
//using System.Web.Http.Description;

using Microsoft.Practices.ServiceLocation;

using C3PO.Data.Models;
using C3PO.Data.Interfaces;

using C3PO.Web.Utilities;

namespace C3PO.Web.Controllers
{
    public class BoardingController : ApiController
    {
        IServiceLocator _svcLocator;

        public BoardingController()
        {
            _svcLocator = Bootstrapper.InitializeServiceLocator();
        }

        [Authorize]
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            return Web.CreateResponse(Request, _svcLocator.GetInstance<IBoardingRepository>().Get(id));
        }
    }
}
