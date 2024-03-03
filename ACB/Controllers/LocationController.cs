using ACB.Data;
using ACB.Models;
using Microsoft.AspNetCore.Mvc;


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
        public IActionResult Index(Location location)
        {
            if (ModelState.IsValid)
            {
                (location.Latitude, location.Longitude) = GeocodeAddress(location.Address);

                _context.Add(location);
                _context.SaveChanges();

                return RedirectToAction("Confirmation");
            }

            return View(location);
        }

        private (double, double) GeocodeAddress(string address)
        {
            // Use Google Maps Geocoding API to geocode the address
            // This is a simplified example; you'll need to implement this logic
            // Refer to Google Maps Geocoding API documentation for details
            // Mocked values for demonstration purposes

            return (47.6062, -122.3321);
        }

        public IActionResult Confirmation()
        {
            return View();
        }
    }

}