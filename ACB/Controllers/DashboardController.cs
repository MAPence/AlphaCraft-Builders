using ACB.Areas.Identity.Data;
using ACB.Data;
using ACB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;

namespace ACB.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ACBContext _context;
        private readonly UserManager<ACBUser> _userManager;
        private readonly SignInManager<ACBUser> _signInManager;

        public DashboardController(UserManager<ACBUser> userManager, SignInManager<ACBUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Home(ContractorVM contractor)
        {
            if (contractor.Email == null)
            {
                contractor = new ContractorVM();
                var user = await _userManager.GetUserAsync(HttpContext.User);

                if (user != null)
                {
                    var userId = user.UserName;

                    if (userId != "System.Func`2[System.Security.Claims.ClaimsPrincipal,System.String]")
                    {
                        contractor = Query.GetContractor(userId);
                        return View(contractor);
                    }
                }
            }
            return View(contractor);
        }

        public async Task<IActionResult> CreateOrder(ContractorVM contractor)
        {
            if (contractor.Email == null)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);


                if (user != null)
                {
                    string? userId = user.UserName;
                    contractor = Query.GetContractor(userId);
                    return View(contractor);
                }
            }
            return View(contractor);
        }

        public IActionResult DisplayQuote(int? Id)
        {
            //var user = await _userManager.GetUserAsync(HttpContext.User);
            var user = User.FindFirstValue(ClaimTypes.Name);

            if (user != null)
            {
                //string? userId = user.UserName;
                ContractorVM contractor = Query.GetContractor(user);

                foreach (var quote in contractor.Quotes)
                {
                    if (quote.Id == Id)
                    {
                        contractor.Quote = quote;
                    }
                }
                contractor.Quote.Images = Query.GetQuoteImages(Id);
                return View(contractor);
            }
            return View("../User/Login");

        }

        public async Task<IActionResult> AllOrders(ContractorVM contractor)
        {
            // Retrieve the current authenticated user
            var user = await _userManager.GetUserAsync(HttpContext.User);

            // Retrieve the orders from the database using the user's username
            contractor = Query.GetContractor(user.UserName);
            contractor.Orders = Query.GetOrders(contractor.Id);

            return View(contractor);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ProfileSettings()
        {
            List<Service> services = Query.ServiceSelection("contractor_service");
            ViewBag.Services = services;

            var user = User.FindFirstValue(ClaimTypes.Name);

            if (user != null)
            {
                //string? userId = user.UserName;
                ContractorVM contractor = Query.GetContractor(user);

                foreach (var service in contractor.Services)
                {
                    foreach (var chk in services)
                    {
                        if (service == chk.Id)
                        {
                            chk.IsOffered = true;
                            break;
                        }
                    }
                }
                return View(contractor);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateServices(int[] data, ContractorVM contractor)
        {
            List<Service> services = Query.ServiceSelection("contractor_service");
            ViewBag.Services = services;
            foreach (var service in data)
            {

                services[service - 1].IsOffered = true;

            }

            foreach (var num in data)
            {
                System.Diagnostics.Debug.WriteLine(num);
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user != null)
            {

                contractor = Query.GetContractor(user.UserName);

                Query.UpdateServices(contractor.Id, data);

                contractor = Query.GetContractor(contractor.Email);



            }

            

            return View("ProfileSettings", contractor);
        }



    }


}