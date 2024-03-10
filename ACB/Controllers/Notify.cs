using ACB.Models;
using System.Data.SqlClient;
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

        public static void QuoteSuccessful(Quote quote,string? service)
        {
            MailMessage mail = new MailMessage();
            
            mail.To.Add(quote.client_email);
            mail.From = new MailAddress("alphaCraftBuilders@gmail.com");
            mail.Subject = "Your Quote";

            string Body = $@"<p>Hey {quote.client_first_name}!</p> 

                <p>Thank you for your quote request for {service} in {quote.city}, {quote.state} {quote.zip}.</p>

                <h3>Project Details</h3>
                <p>
                Description: {quote.details}
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
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("alphacraftbuilders", GetPassword()); // Enter senders User name and password       
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        public static void ContactForm(ContactSubmission contactform)
        {
            MailMessage mail = new MailMessage();

            mail.To.Add("support@alphacraftbuilders.com");
            mail.From = new MailAddress(contactform.email, contactform.name);
            mail.CC.Add(new MailAddress(contactform.email));
            mail.Subject = $"{contactform.name} sent a message from the contact form";
            string Body = $@"<p>Hey {contactform.name}!</p> 

                <p>Thank you for you for reaching out. A representative will get back to you as soon as possible.</p>

                <h3>Your Submission</h3>
                <p>
                {contactform.message}
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
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("alphacraftbuilders", GetPassword()); // Enter senders User name and password       
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
}