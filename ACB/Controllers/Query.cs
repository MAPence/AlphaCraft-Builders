using Microsoft.AspNetCore.Mvc;

using ACB.Models;
using System.Data;

using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace ACB.Controllers
{
    public static class Query
    {
        private static string conn = "Server=tcp:pruv.database.windows.net,1433;Initial Catalog=AlphaCraftDB;Persist Security Info=False;User ID=CloudSAb0e3e238;Password=alphaCraft24;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder();

            builder.AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            
            var connString = configuration.GetConnectionString("alphacraftdb");

            

            return connString;


        }
        
     

        public static void Insert(string insertQuery)
        {
            System.Diagnostics.Debug.WriteLine(GetConnectionString());

            SqlConnection sqlconn = new SqlConnection(conn);
            SqlCommand sqlquery = new SqlCommand(insertQuery, sqlconn);
            sqlconn.Open();
            sqlquery.ExecuteNonQuery();
            sqlconn.Close();

        }

        public static void NewQuote(Quote quote)
        {
            string query = $"insert into quote (client_first_name, client_last_name, client_email, details, job_zip)" +
                $"\r\n values ('{quote.client_first_name}' ,'{quote.client_last_name}', '{quote.client_email}'," +
                $"\r\n '{quote.details}','{quote.zip}');";
            System.Diagnostics.Debug.WriteLine(query);
            Insert(query);
        }



    }
}
