using ACB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;

namespace ACB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _recaptchaSecretKey;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _recaptchaSecretKey = Environment.GetEnvironmentVariable("6LcCx9ApAAAAAGWJ-zKvI6n2YfGXCiSNmIHfTOlL");
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult ContactUs(ContactSubmission submission)
        {
            // Verify reCAPTCHA
            var recaptchaResponse = Request.Form["g-recaptcha-response"];
            if (!VerifyRecaptcha(recaptchaResponse))
            {
                ModelState.AddModelError("", "reCAPTCHA validation failed.");
                return View(submission);
            }

            // Process form submission
            Notify.ContactForm(submission);
            return View("Index");
        }

        private bool VerifyRecaptcha(string recaptchaResponse)
        {
            using (var client = new WebClient())
            {
                var response = client.DownloadString($"https://www.google.com/recaptcha/api/siteverify?secret={_recaptchaSecretKey}&response={recaptchaResponse}");
                var reCaptchaResult = JsonConvert.DeserializeObject<ReCaptchaResponse>(response);

                return reCaptchaResult.Success;
            }
        }

        public IActionResult FindContractors()
        {
            return View();
        }

        public class ReCaptchaResponse
        {
            public bool Success { get; set; }
            public string ChallengeTs { get; set; }
            public string Hostname { get; set; }
            public List<string> ErrorCodes { get; set; }
        }
    }
}

