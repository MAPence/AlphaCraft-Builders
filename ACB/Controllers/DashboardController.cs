using ACB.Areas.Identity.Data;
using ACB.Data;
using ACB.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics.Contracts;
//using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace ACB.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ACBContext _context;
        private readonly UserManager<ACBUser> _userManager;
        private readonly SignInManager<ACBUser> _signInManager;

        public DashboardController(UserManager<ACBUser> userManager,
            SignInManager<ACBUser> signInManager)
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

                if(user != null)
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
            if(contractor.Email == null)
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

        /*public async Task<IActionResult> DisplayQuote(int? Id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            

            if (user != null)
            {
                string? userId = user.UserName;
                ContractorVM contractor = Query.GetContractor(userId);

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
            return View();
        }*/

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
        public async Task<IActionResult> ProfileSettings()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new ProfileSettings
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}