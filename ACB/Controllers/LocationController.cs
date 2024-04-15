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
            ViewBag.Services = new SelectList(Query.PopulateDropDown("contractor_service", 1));
            return View();
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
        //[HttpPost]
        //public IActionResult LocationQuery(latitude, longitude)
        //{

        //    return View();
        //}

    }
}