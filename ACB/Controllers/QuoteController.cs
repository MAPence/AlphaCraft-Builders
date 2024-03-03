using Microsoft.AspNetCore.Mvc;
using ACB.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ACB.Controllers
{
    public class QuoteController : Controller
    {
        public IActionResult QuoteForm()
        {
            ViewBag.Services = new SelectList(Query.PopulateDropDown("contractor_service", 1));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create
            ([Bind("service,client_first_name,client_last_name,client_email,details,city,state,zip")] Quote quote, string? service)
        {
            Query.NewQuote(quote);
            Notify.QuoteSuccessful(quote, service);

            ViewBag.Services = new SelectList(Query.PopulateDropDown("contractor_service", 1));
            
            return View("../Home/Index");
        }
    }
}
