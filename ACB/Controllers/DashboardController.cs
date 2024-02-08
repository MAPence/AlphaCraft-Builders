using Microsoft.AspNetCore.Mvc;

namespace ACB.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Home()
        {
            return View();
        }


    }
}
