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

            [HttpPost]
            public async Task<IActionResult> ProfileSettings(ProfileSettings model)
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    AddErrors((IEnumerable<IdentityError>)result);
                    return View(model);
                }

                user.Email = model.Email;
                user.UserName = $"{model.FirstName} {model.LastName}";
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    AddErrors((IEnumerable<IdentityError>)updateResult);
                    return View(model);
                }

                return RedirectToAction("Index", "Home");
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