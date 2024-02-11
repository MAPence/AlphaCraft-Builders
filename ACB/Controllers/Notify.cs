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

            System.Diagnostics.Debug.WriteLine("HELLLOOOOOO!!!!" + password);

            return password;

        }

        public static void QuoteSuccessful(Quote quote)
        {
            MailMessage mail = new MailMessage();
            
            mail.To.Add(quote.client_email);
            mail.From = new MailAddress("alphaCraftBuilders@gmail.com");
            mail.Subject = "Your Quote";
            string Body = "Thank You, your quote has been sent to contractors in your area.";
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
