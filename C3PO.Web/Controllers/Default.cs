using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace C3PO.Web.Controllers
{
    public class Default : Controller
    {
        public IActionResult Index()
        {
            var currentTime = DateTime.Now;
            ViewBag.propertyOnViewBag = "This is set in the controller";
            ViewBag.currentDate = currentTime;
            return View("js-{auto}", new { CurrentTime = currentTime });
        }
    }
}