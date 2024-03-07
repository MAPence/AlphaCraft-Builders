using Microsoft.AspNetCore.Mvc;

using ACB.Models;
using System.Data;

using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using NuGet.Protocol.Plugins;
using Azure.Core;

namespace ACB.Controllers
{
    public static class Query
    {
        //Get connection string from Appsettings.Development.Json file and return
        public static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: false);
            IConfiguration configuration = builder.Build();

            var connString = configuration.GetConnectionString("InterserverSQL");
            return connString;

        }

        //return list of strings from all rows in a table
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


        //insert into any table, and return id of new row
        public static int Insert(string insertQuery)
        {
            //var to store new id upon inserted row
            int newId;

            //open db connection and run passed in query
            SqlConnection sqlconn = new SqlConnection(GetConnectionString());
            SqlCommand sqlquery = new SqlCommand(insertQuery, sqlconn);
            sqlconn.Open();
            
            //execute query statement, return new id
            newId = Convert.ToInt32(sqlquery.ExecuteScalar());
            System.Diagnostics.Debug.WriteLine("New ID : " + newId);
            sqlconn.Close();
            return newId;

        }
        //insert a new customer generated quote request
        public static int NewQuote(Quote quote)
        {
            //create query statement, insert quote and return new id
            string query = $"insert into quote (client_first_name, client_last_name, client_email, details, job_zip, job_type) " +
                $"\r\noutput inserted.id " +
                $"\r\nvalues ('{quote.client_first_name}' ,'{quote.client_last_name}', '{quote.client_email}'," +
                $"\r\n'{quote.details}','{quote.zip}', {quote.service});";

            //check query statement in output window
            System.Diagnostics.Debug.WriteLine(query);
            return Insert(query);
        } 

        //get the id of a row in a table wih a unique string value
        public static int GetDBId(string name, string table, string field)
        {
            //query table for specific row, return the id of that row
            SqlConnection sqlconn2 = new SqlConnection(GetConnectionString());
            string sqlquery2 = $"select * from {table} where {field} = '{name}'";
            System.Diagnostics.Debug.WriteLine(sqlquery2);
            SqlCommand sqlcomm2 = new SqlCommand(sqlquery2, sqlconn2);
            sqlconn2.Open();
            SqlDataAdapter adapter2 = new SqlDataAdapter(sqlcomm2);
            DataTable dt2 = new DataTable();
            adapter2.Fill(dt2);
            sqlconn2.Close();
            return int.Parse(dt2.Rows[0][0].ToString()!);

        }

        public static void AddImageToDB(int fk, byte[] image)
        {
            SqlConnection sqlconn = new SqlConnection(GetConnectionString());
            const string sql_insert_string =
                "Insert into quote_image (Quote_id, q_image) values (@image_id, @image_byte_array)";
            var byteParam = new SqlParameter("@image_byte_array", SqlDbType.VarBinary)
            {
                Direction = ParameterDirection.Input,
                Size = image.Length,
                Value = image
            };

            var imageIdParam = new SqlParameter("@image_id", SqlDbType.Int, 4)
            {
                Direction = ParameterDirection.Input,
                Value = fk
            };
            SqlTransaction transaction = null;
            SqlCommand sqlcomm = new SqlCommand(sql_insert_string, sqlconn, transaction);
            sqlcomm.Parameters.Add(byteParam);
            sqlcomm.Parameters.Add(imageIdParam);
            sqlconn.Open();
            sqlcomm.ExecuteNonQuery();
            sqlconn.Close();


        }

        public static List<int> GetServicesoffered(int? contractorId)
        {
            List <int> services = new List<int>();
            SqlConnection sqlconn = new SqlConnection(GetConnectionString());
            string sqlQuery = "select service_id from contractor_service_offered" +
                $"\r\n Where contractor_id = {contractorId}";
            SqlCommand cmnd = new SqlCommand(sqlQuery, sqlconn);
            sqlconn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmnd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            sqlconn.Close();
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                services.Add(Convert.ToInt32(dt.Rows[i][0]));
            }

            return services;
            

        }

        public static List<QuoteVM>? GetQuotes(int? service_type)
        {
            List<QuoteVM>? quotes = new List<QuoteVM>();

            SqlConnection sqlconn = new SqlConnection(GetConnectionString());
            string sqlQuery = "select * from quote" +
                $"\r\n join contractor_service on job_type = contractor_service.id" +
                $"\r\n Where job_type = {service_type}";
            SqlCommand cmnd = new SqlCommand(sqlQuery, sqlconn);
            sqlconn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmnd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            sqlconn.Close();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                QuoteVM quote = new QuoteVM();
                quote.Id = Convert.ToInt32(dt.Rows[i][0]);
                quote.Firstname = (string?)dt.Rows[i][1];
                quote.Lastname = (string?)dt.Rows[i][2];
                quote.Email = (string?)dt.Rows[i][3];
                quote.Service = (string?)dt.Rows[i][12];
                quotes.Add(quote);
            }

            return quotes;


        }

        public static ContractorVM GetContractor(string email)
        {
            var contractor = new ContractorVM();
            SqlConnection sqlconn = new SqlConnection(GetConnectionString());
            string sqlQuery = "select contractor_id, FirstName, LastName from AspNetUsers" +
                $"\r\n Where UserName = '{email}'";
            SqlCommand cmnd = new SqlCommand(sqlQuery, sqlconn);
            sqlconn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmnd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            sqlconn.Close() ;
            contractor.Id = Convert.ToInt32(dt.Rows[0][0]);
            contractor.FirstName = dt.Rows[0][1].ToString();
            contractor.LastName = dt.Rows[0][2].ToString();
            contractor.Email = email;
            contractor.Services = GetServicesoffered(contractor.Id);
            foreach(var service in contractor.Services)
            {
                
                List<QuoteVM> quotes = GetQuotes(service);
                if(quotes != null && quotes.Count > 0)
                {
                    contractor.Quotes = new List<QuoteVM>();
                    foreach (QuoteVM quote in quotes)
                    {
                        if(quote != null)
                        {
                            contractor.Quotes.Add(quote);
                        }
                        
                    }

                }
                
            }
            



            return contractor;

        }



    }
}
