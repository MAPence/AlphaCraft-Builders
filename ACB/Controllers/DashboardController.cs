using ACB.Areas.Identity.Data;
using ACB.Data;
using ACB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics.Contracts;

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
            if(contractor.Email == null)
            {

                contractor = new ContractorVM();

                var user = await _userManager.GetUserAsync(HttpContext.User);
                var userId = user.UserName;

                if (userId != "System.Func`2[System.Security.Claims.ClaimsPrincipal,System.String]")
                {
                    contractor = Query.GetContractor(userId);
                    return View(contractor);
                }
            }
            return View(contractor);
        }
        public async Task<IActionResult> CreateOrder(ContractorVM contractor)
        {
            if(contractor == null)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                string? userId = user.UserName;

                if (userId != null)
                {
                    contractor = Query.GetContractor(userId);

                    return View(contractor);
                }
            }
            return View(contractor);
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
    }
}