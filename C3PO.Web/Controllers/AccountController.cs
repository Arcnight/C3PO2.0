using System;
using System.Net.Http;
using System.Web.Http;

namespace C3PO.Web.Controllers
{
    public class AccountController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Login(string userName, string password)
        {
            var response = new HttpResponseMessage() { };

            return response;
        }

        [HttpGet]
        public void Logout()
        {
            
        }
    }
}
