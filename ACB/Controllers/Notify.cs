using ACB.Models;
using System.Net.Mail;

namespace ACB.Controllers
{
    public static class Notify
    {
        public static string GetPassword()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: false);

            IConfiguration configuration = builder.Build();

            var password = configuration.GetConnectionString("password");
            return password;
        }

        public static void QuoteSuccessful(Quote quote, string? service)
        {
            MailMessage mail = new();

            mail.To.Add(quote.Client_email);
            mail.From = new MailAddress("alphaCraftBuilders@gmail.com");
            mail.Subject = "Your Quote";

            string Body = $@"<p>Hey {quote.Client_first_name}!</p> 

                <p>Thank you for your quote request for {service} in {quote.City}, {quote.State} {quote.Zip}.</p>

                <h3>Project Details</h3>
                <p>
                Description: {quote.Details}
                </p>

                <p> Your request has been sent to contractors in your area. We will notify you when they respond with bids. Feel free to reach out with any other questions.</p>


                <p>Best Regards,
                <br/>  
                The AlphaCraft Builders Team
                <br/>
                www.AlphaCraftBuilders.com
                </p>";

            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("alphacraftbuilders", GetPassword()), // Enter senders User name and password       
                EnableSsl = true
            };
            smtp.Send(mail);
        }

        public static void ContactForm(ContactSubmission contactform)
        {
            MailMessage mail = new();

            mail.To.Add("support@alphacraftbuilders.com");
            mail.From = new MailAddress(contactform.Email, contactform.Name);
            mail.CC.Add(new MailAddress(contactform.Email));
            mail.Subject = $"{contactform.Name} sent a message from the contact form";
            string Body = $@"<p>Hey {contactform.Name}!</p> 

                <p>Thank you for you for reaching out. A representative will get back to you as soon as possible.</p>

                <h3>Your Submission</h3>
                <p>
                {contactform.Message}
                </p>

                <p>Best Regards,
                <br/>  
                AlphaCraft Builders Support Team
                <br/>
                support@alphacraftbuilders.com
                <br/>
                www.AlphaCraftBuilders.com
                </p>";

            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("alphacraftbuilders", GetPassword()), // Enter senders User name and password       
                EnableSsl = true
            };
            smtp.Send(mail);
        }
    }
}