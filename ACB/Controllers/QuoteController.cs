using ACB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace ACB.Controllers
{
    public class QuoteController : Controller
    {
        public List<SelectListItem> PopulateStates() 
        {
            List<SelectListItem> States = new()
            {
            new SelectListItem { Text = "Alabama", Value = "AL" },
            new SelectListItem { Text = "Alaska", Value = "AK" },
            new SelectListItem { Text = "Arizona", Value = "AZ" },
            new SelectListItem { Text = "Arkansas", Value = "AR" },
            new SelectListItem { Text = "California", Value = "CA" },
            new SelectListItem { Text = "Colorado", Value = "CO" },
            new SelectListItem { Text = "Connecticut", Value = "CT" },
            new SelectListItem { Text = "Delaware", Value = "DE" },
            new SelectListItem { Text = "Florida", Value = "FL" },
            new SelectListItem { Text = "Georgia", Value = "GA" },
            new SelectListItem { Text = "Hawaii", Value = "HI" },
            new SelectListItem { Text = "Idaho", Value = "ID" },
            new SelectListItem { Text = "Illinois", Value = "IL" },
            new SelectListItem { Text = "Indiana", Value = "IN" },
            new SelectListItem { Text = "Iowa", Value = "IA" },
            new SelectListItem { Text = "Kansas", Value = "KS" },
            new SelectListItem { Text = "Kentucky", Value = "KY" },
            new SelectListItem { Text = "Louisiana", Value = "LA" },
            new SelectListItem { Text = "Maine", Value = "ME" },
            new SelectListItem { Text = "Maryland", Value = "MD" },
            new SelectListItem { Text = "Massachusetts", Value = "MA" },
            new SelectListItem { Text = "Michigan", Value = "MI" },
            new SelectListItem { Text = "Minnesota", Value = "MN" },
            new SelectListItem { Text = "Mississippi", Value = "MS" },
            new SelectListItem { Text = "Missouri", Value = "MO" },
            new SelectListItem { Text = "Montana", Value = "MT" },
            new SelectListItem { Text = "Nebraska", Value = "NE" },
            new SelectListItem { Text = "Nevada", Value = "NV" },
            new SelectListItem { Text = "New Hampshire", Value = "NH" },
            new SelectListItem { Text = "New Jersey", Value = "NJ" },
            new SelectListItem { Text = "New Mexico", Value = "NM" },
            new SelectListItem { Text = "New York", Value = "NY" },
            new SelectListItem { Text = "North Carolina", Value = "NC" },
            new SelectListItem { Text = "North Dakota", Value = "ND" },
            new SelectListItem { Text = "Ohio", Value = "OH" },
            new SelectListItem { Text = "Oklahoma", Value = "OK" },
            new SelectListItem { Text = "Oregon", Value = "OR" },
            new SelectListItem { Text = "Pennsylvania", Value = "PA" },
            new SelectListItem { Text = "Rhode Island", Value = "RI" },
            new SelectListItem { Text = "South Carolina", Value = "SC" },
            new SelectListItem { Text = "South Dakota", Value = "SD" },
            new SelectListItem { Text = "Tennessee", Value = "TN" },
            new SelectListItem { Text = "Texas", Value = "TX" },
            new SelectListItem { Text = "Utah", Value = "UT" },
            new SelectListItem { Text = "Vermont", Value = "VT" },
            new SelectListItem { Text = "Virginia", Value = "VA" },
            new SelectListItem { Text = "Washington", Value = "WA" },
            new SelectListItem { Text = "West Virginia", Value = "WV" },
            new SelectListItem { Text = "Wisconsin", Value = "WI" },
            new SelectListItem { Text = "Wyoming", Value = "WY" }
            };
            return States;
        }
        
        public IActionResult QuoteForm(int? id)
        {
            Quote q = new Quote();

            if (id != null)
            {
                q.contractor = id;
                q.company = Query.GetCompany(id);
            }
            
            ViewBag.StateOptions = PopulateStates();

            ViewBag.Services = new SelectList(Query.PopulateDropDown("contractor_service", 1));
            return View(q);
        }
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create
            ([Bind("Client_first_name,Client_last_name,Client_email,Details,Address,City,State,Zip")] Quote quote, 
            string? service, List<IFormFile> imageFile)
        {
             if(service == null)
             {

                ModelState.AddModelError(nameof(service), "Please select a valid service type");
                ViewBag.StateOptions = PopulateStates();
                ViewBag.Services = new SelectList(Query.PopulateDropDown("contractor_service", 1));
                return View("QuoteForm",quote);
             }

            quote.Service = Query.GetDBId(service, "contractor_service", "service_type");

            //set long/lat on quote...
            GeoLocation geo = new GeoLocation();
            quote.LatLong = await geo.GetCoordinatesAsync(quote.Address,quote.City,quote.State,quote.Zip.ToString());   

            //inserts quote into db
            int fk = Query.NewQuote(quote);

            foreach (var image in imageFile)
            {
                Query.AddImageToDB(fk, ImageHandler.ConvertImageFile(image));
                System.Diagnostics.Debug.WriteLine(fk);
            }
            Notify.QuoteSuccessful(quote, service);

            ViewBag.StateOptions = PopulateStates();
            ViewBag.Services = new SelectList(Query.PopulateDropDown("contractor_service", 1));
            return View("../Home/Index");
        }
    }
}