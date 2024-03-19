using ACB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ACB.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
            private readonly UserManager<IdentityUser> _userManager;

            public ProfileController(UserManager<IdentityUser> userManager)
            {
                _userManager = userManager;
            }

            public IActionResult ProfileSettings()
            {
                return View();
            }

        private void AddErrors(IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, $"Code: {error.Code}, Message: You have encountered an error");
            }
        }
    }
}