using ACB.Areas.Identity.Data;
using ACB.Data;
using ACB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ACB.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ACBContext _context;
        private readonly UserManager<ACBUser> _userManager;
        private readonly SignInManager<ACBUser> _signInManager;

        public DashboardController(UserManager<ACBUser> userManager, SignInManager<ACBUser> signInManager, ACBContext context)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._context = context;
        }

        public List<SelectListItem> JobList(int? id)
        {
            string q = "select id, concat(client_first_name, ' ', client_last_name) from Job" +
                $"\r\nwhere contractor_id = {id};";
            return Query.GetOptions(q);
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
                        System.Diagnostics.Debug.WriteLine(contractor.Services.Count());
                        return View(contractor);
                    }
                }
            }
            return View(contractor);
        }

        public async Task<IActionResult> CreateOrder(ContractorVM contractor)
        {
            //ViewBag.jobs = JobList(contractor.Id);
            if (contractor.Email == null)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user != null)
                {
                    string? userId = user.UserName;
                    contractor = Query.GetContractor(userId);
                    ViewBag.jobs = JobList(contractor.Id);
                    return View(contractor);
                }
            }
            ViewBag.jobs = JobList(0);
            return View(contractor);
        }
        [HttpPost]

        public IActionResult CreateOrder(int? job, decimal? sub, decimal? tax, string? notes)
        {
            var user = User.FindFirstValue(ClaimTypes.Name);

            if (user != null)
            {
                ContractorVM contractor = Query.GetContractor(user);
                NewOrder order = new NewOrder()
                {
                    Case_id = job,
                    Subtotal = sub,
                    SalesTax = tax,
                    Notes = notes

                };

                string insertOrder = "Insert into Orders (co_id, case_id, sub_total, tax, notes, created)" +
                    "\r\nOutput Inserted.Id" +
                    $"\r\nvalues ({contractor.Id}, {order.Case_id}, {order.Subtotal}, {order.SalesTax}, '{order.Notes}', Getdate())";
                Query.Insert(insertOrder);
                return View(contractor);
            }
            
            return View();
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

        public IActionResult DisplayOrders(int? Id)
        {
            var user = User.FindFirstValue(ClaimTypes.Name);

            if (user != null)
            {
                ContractorVM contractor = Query.GetContractor(user);

                // Retrieve the single order based on the provided Id
                if (Id.HasValue)
                {
                    contractor.NewOrder = Query.GetSingleOrder(Id.Value);
                }
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
        public async Task<IActionResult> AllJobs(ContractorVM contractor)
        {
            // Retrieve the current authenticated user
            var user = await _userManager.GetUserAsync(HttpContext.User);

            // Retrieve the orders from the database using the user's username
            contractor = Query.GetContractor(user.UserName);
            contractor.Jobs = Query.GetJobs(contractor.Id);

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
        public void UpdateServices(int[] u_serv, ContractorVM contractor)
        {
            var user = User.FindFirstValue(ClaimTypes.Name);

            if (user != null)
            {
                contractor = Query.GetContractor(user);
                Query.UpdateServices(contractor.Id, u_serv);
            }    
        }
        
        public async Task<IActionResult> NewJob(int id)
        {
            ContractorVM contractor = new ContractorVM();
            System.Diagnostics.Debug.WriteLine(id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                string? userId = user.UserName;
                contractor = Query.GetContractor(userId);

                if(id != 0)
                {
					contractor.Job = Query.ConvertQuote(id);
				}
                return View("NewJob",contractor);
            }
            contractor.Job = new JobVM();

            return View(contractor);
        }

        [HttpPost]
        public async Task<IActionResult> NewJob([Bind("Firstname,Lastname,Email,Address,City,State,Zip,Details,Start,End,Amount")] JobVM job)
        {
			ContractorVM contractor = new ContractorVM();
			var user = await _userManager.GetUserAsync(HttpContext.User);

            //ensure valid user is logged in and insert new job
            if(user != null)
            {
                string? userId = user.UserName;
                contractor = Query.GetContractor(userId);

                string query = "insert into Job " +
                    "(contractor_id, client_first_name, client_last_name, client_email, job_zip, " +
                    "job_address, details, quote_total,job_start, job_end)" +
                    $"\r\nvalues ({contractor.Id}, '{job.Firstname}', '{job.Lastname}', '{job.Email}', {job.Zip}," +
                    $"\r\n'{job.Address} {job.City}, {job.State}', '{job.Details}', {job.Amount}, '{job.Start}', '{job.End}');";
                Query.Insert(query);
                System.Diagnostics.Debug.WriteLine("Hello");

                //populate contractor jobs and route to Jobs table
                contractor.Jobs = Query.GetJobs(contractor.Id);
                return View("AllJobs", contractor);

            }
            return View();			
        }
    }
}