using ACB.Areas.Identity.Data;
using ACB.Data;
using ACB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ACB.Controllers
{
    public class UserController : Controller
    {
        private readonly ACBContext _context;
        private readonly UserManager<ACBUser> _userManager;
        private readonly SignInManager<ACBUser> _signInManager;

        public UserController(UserManager<ACBUser> userManager,
            SignInManager<ACBUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                //login
                var result = await _signInManager.PasswordSignInAsync(model.Username!, model.Password!, model.RememberMe, false);

                if (result.Succeeded)
                {
                    ContractorVM contractor = Query.GetContractor(model.Username);
                    return View("../Dashboard/Home", contractor);
                }

                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }
            return View(model);
        }
        // GET: UserController
        public ActionResult Register()
        {

            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterNew(ACB.Models.User userInfo)
        {
            //setting values for required fields to generic while in dev
            userInfo.Title = "Owner";
            userInfo.Country = "USA";

            if (ModelState.IsValid)
            {
                //bypassing email confirmation requirement for now
                ACBUser user = new()
                {
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName,
                    Email = userInfo.Email,
                    Country = userInfo.Country,
                    Title = userInfo.Title,
                    Address = userInfo.Address,
                    City = userInfo.City,
                    Zip = userInfo.Zip,
                    State = userInfo.State,
                    Company = userInfo.Company,
                    Phone = userInfo.Phone,
                    UserName = userInfo.Email,
                    EmailConfirmed = true

                };

                GeoLocation geo = new GeoLocation();
                Task<LatLong> latlong = geo.GetCoordinatesAsync(user.Address, user.City, user.State, user.Zip);
                user.latitude = latlong.Result.Lat;
                user.longitude = latlong.Result.Long;
                var result = await _userManager.CreateAsync(user, userInfo.Password!);

                if (result.Succeeded)
                {
                    ContractorVM contractor = Query.GetContractor(user.UserName);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return View("../Dashboard/Home", contractor);

                }
                return View("Register");
            }
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}