using System;
using System.Web.Http;
using System.Net.Http;

using C3PO.Web.Models;

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

        [HttpPost]
        [Route("account/login")]
        public HttpResponseMessage Login([FromBody] UserCredentials creds)
        {
            var response = new HttpResponseMessage() { };

            return response;
        }

        [HttpGet]
        [Route("account/logout")]
        public void Logout()
        {
            
        }

        bool AuthenticateUser(UserCredentials creds)
        {
            return true;
        }
    }
}
