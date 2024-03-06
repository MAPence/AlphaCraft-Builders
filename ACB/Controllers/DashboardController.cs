using ACB.Areas.Identity.Data;
using ACB.Data;
using ACB.Models;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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

        public IActionResult SideBar( ContractorVM contractor) 
        {
            var currentUser = _userManager.GetUserName;

            var user = currentUser.ToString();

            if(user != "System.Func`2[System.Security.Claims.ClaimsPrincipal,System.String]")
            {
                contractor = Query.GetContractor(user);
                return PartialView("../Shared/_sidebar", contractor);
            }
            
            return PartialView("../Shared/_sidebar");
        }

        public IActionResult Home(ContractorVM contractor)
        {
            if(contractor == null)
            {
                contractor = new ContractorVM();
            }
            SideBar(contractor);
            return View(contractor);
        }


    }
}
