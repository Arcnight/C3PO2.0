using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace C3PO.Web.Controllers
{
    [Route("search"), Authorize]
    public class Search: Controller
    {
    }
}
