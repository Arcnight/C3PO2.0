using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace C3PO.Web.Controllers
{
    [Route("person"), Authorize]
    public class PersonController : Controller
    {
    }
}
