using ACB.Areas.Identity.Data;
using ACB.Data;
using Microsoft.AspNetCore.Http;
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

        // GET: UserController
        public ActionResult Register()
        {
            
            return View();
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterNew(ACB.Models.User userInfo)
        {
            userInfo.Title = "Owner";
            userInfo.Country = "USA";

            if (ModelState.IsValid)
            {
                ACBUser user = new()
                {
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName, Email = userInfo.Email, Country = userInfo.Country, Title = userInfo.Title,
                    Address = userInfo.Address, City = userInfo.City, Zip = userInfo.Zip, State = userInfo.State,
                    Company = userInfo.Company, Phone = userInfo.Phone, UserName = userInfo.Company

                };

                
                var result = await _userManager.CreateAsync(user, userInfo.Password!);

                if (result.Succeeded)
                {
                    return View("../Dashboard/Home");

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
