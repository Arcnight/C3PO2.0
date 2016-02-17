using System;
using System.Web.Http;
using System.Net.Http;

namespace C3PO.Web.Controllers
{
    [RoutePrefix("api")]
    public class AccountController : ApiController
    {
        [HttpGet]
        [Route("account/login")]
        public HttpResponseMessage Login()
        {
            var response = new HttpResponseMessage() { };

            return response;
        }

        [HttpGet]
        [Route("account/login")]
        public HttpResponseMessage Login([FromUri] string userName, [FromUri] string password)
        {
            var response = new HttpResponseMessage() { };

            return response;
        }

        [HttpGet]
        [Route("account/logout")]
        public void Logout()
        {
            
        }
    }
}
