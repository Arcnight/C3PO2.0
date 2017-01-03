using System;

using System.Net.Http;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using C3PO.Data.Models;
using C3PO.Data.Interfaces;

using C3PO.Web.Utilities;

namespace C3PO.Web.Controllers
{
    [Route("boarding"), Authorize]
    public class Boarding : Controller
    {
        IBoardingRepository _boardingRepo;

        public Boarding(IBoardingRepository boardingRepo)
        {
            _boardingRepo = boardingRepo;
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            return Http.CreateGoodResponse(Request, _boardingRepo.Get(id));
        }
    }
}
