﻿using ACB.Areas.Identity.Data;
using ACB.Data;
using ACB.Models;
//using Microsoft.AspNet.Identity;
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

        public async Task<IActionResult> DisplayQuote(int? Id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            string? userId = user.UserName;

            if (userId != null)
            {
                ContractorVM contractor = Query.GetContractor(userId);

                foreach(var quote in contractor.Quotes)
                {
                    if(quote.Id == Id)
                    {
                        contractor.Quote = quote;
                    }
                }
                contractor.Quote.Images = Query.GetQuoteImages(Id);
                return View(contractor);

            }

            return View();
        }


    }
}
