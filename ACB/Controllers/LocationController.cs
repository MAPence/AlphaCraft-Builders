using ACB.Data;
using ACB.Models;
using Microsoft.AspNetCore.Mvc;
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
    }

}