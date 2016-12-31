using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace C3PO.Web.Controllers
{
    [Route("vessel"), Authorize]
    public class VesselController : Controller
    {
    }
}
