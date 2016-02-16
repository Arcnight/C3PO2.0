using System.Web.Http;
using System.Security.Claims;

namespace C3PO.Controllers
{
    public class SecurityController : ApiController
    {
        [HttpGet]
        [Route("authentication/login")]
        public IHttpActionResult Login()
        {
            var caller = User as ClaimsPrincipal;

            return Json(new
            {
                message = "OK computer",
                client = caller.FindFirst("client_id").Value
            });
        }
    }
}
