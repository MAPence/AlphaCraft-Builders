using Microsoft.AspNetCore.Mvc;
using ACB.Models;

namespace ACB.Controllers
{
    public class QuoteController : Controller
    {
        public IActionResult QuoteForm()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create
            ([Bind("client_first_name,client_last_name,client_email,details,zip")] Quote quote)
        {
            Query.NewQuote(quote);
            return View("QuoteForm");
        }
    }
}
