using ACB.Data;
using ACB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.ProjectModel;

namespace ACB.Controllers
{
    public class LocationController : Controller
    {
        private readonly ACBContext _context;

        public LocationController(ACBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Location()
        {
            Location user = new Location();
            user.Distance = 30;
            ViewBag.Services = new SelectList(Query.PopulateDropDown("contractor_service", 1));
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Location location)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Location");
            }
            return View("../Location/Location");
        }

        public IActionResult Confirmation()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Location(string stype, decimal? latitude, decimal? longitude, string? Address, string? City, int? Zip, string? State, int? distance)
        { 
            ViewBag.Services = new SelectList(Query.PopulateDropDown("contractor_service", 1));
            Location user = new Location();
            user.Results = Query.FindContractors(stype, latitude, longitude, distance);

            user.Zip = Zip.ToString();
            user.State = State;
            user.Address = Address;
            user.City = City;
            user.WorkNeeded = stype;
            user.Distance = distance;

            return View("Location",user);
        }

    }
}