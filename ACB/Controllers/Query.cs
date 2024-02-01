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
        
        public static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: false);

            
            IConfiguration configuration = builder.Build();

            

            var connString = configuration.GetConnectionString("alphacraftdb");

            System.Diagnostics.Debug.WriteLine("HELLLOOOOOO!!!!" + connString);

            return connString;

        }

        public static List<string> PopulateDropDown(string table, int column)
        {
            // new list that will be retunred for drop down menu
            List<string> list = new List<string>();

            // query table for values to populate list using column and table parameters
            SqlConnection sqlconn = new SqlConnection(GetConnectionString());
            string sqlquery = $"select * from {table}";
            SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
            sqlconn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                // add value to list
                list.Add(dt.Rows[i][column].ToString()!);

            }
            sqlconn.Close();
            return list;
        }



        public static void Insert(string insertQuery)
        {
            

            SqlConnection sqlconn = new SqlConnection(GetConnectionString());
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
