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

        public static DataTable GetDataTable(string query)
        {
            SqlConnection sqlconn = new(GetConnectionString());
            string sqlQuery = query;
            SqlCommand cmnd = new(sqlQuery, sqlconn);
            sqlconn.Open();
            SqlDataAdapter adapter = new(cmnd);
            DataTable dt = new();
            adapter.Fill(dt);

            return dt;
        }

        //return list of strings from all rows in a table
        public static List<string> PopulateDropDown(string table, int column)
        {
            // new list that will be retunred for drop down menu
            List<string> list = new();

            // query table for values to populate list using column and table parameters
            SqlConnection sqlconn = new(GetConnectionString());
            string sqlquery = $"select * from {table}";
            SqlCommand sqlcomm = new(sqlquery, sqlconn);
            sqlconn.Open();
            SqlDataAdapter adapter = new(sqlcomm);
            DataTable dt = new();
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
            SqlConnection sqlconn = new(GetConnectionString());
            SqlCommand sqlquery = new(insertQuery, sqlconn);
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
                $"\r\nvalues ('{quote.Client_first_name}' ,'{quote.Client_last_name}', '{quote.Client_email}'," +
                $"\r\n'{quote.Details}','{quote.Zip}', {quote.Service});";

            //check query statement in output window
            System.Diagnostics.Debug.WriteLine(query);
            return Insert(query);
        }

        //get the id of a row in a table wih a unique string value
        public static int GetDBId(string name, string table, string field)
        {
            //query table for specific row, return the id of that row
            SqlConnection sqlconn2 = new(GetConnectionString());
            string sqlquery2 = $"select * from {table} where {field} = '{name}'";
            System.Diagnostics.Debug.WriteLine(sqlquery2);
            SqlCommand sqlcomm2 = new(sqlquery2, sqlconn2);
            sqlconn2.Open();
            SqlDataAdapter adapter2 = new(sqlcomm2);
            DataTable dt2 = new();
            adapter2.Fill(dt2);
            sqlconn2.Close();
            return int.Parse(dt2.Rows[0][0].ToString()!);
        }

        public static void AddImageToDB(int fk, byte[] image)
        {
            SqlConnection sqlconn = new(GetConnectionString());
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
            SqlCommand sqlcomm = new(sql_insert_string, sqlconn, transaction);
            sqlcomm.Parameters.Add(byteParam);
            sqlcomm.Parameters.Add(imageIdParam);
            sqlconn.Open();
            sqlcomm.ExecuteNonQuery();
            sqlconn.Close();
        }

        public static List<int> GetServicesoffered(int? contractorId)
        {
            List<int> services = new();
            SqlConnection sqlconn = new(GetConnectionString());
            string sqlQuery = "select service_id from contractor_service_offered" +
                $"\r\n Where contractor_id = {contractorId}";
            SqlCommand cmnd = new(sqlQuery, sqlconn);
            sqlconn.Open();
            SqlDataAdapter adapter = new(cmnd);
            DataTable dt = new();
            adapter.Fill(dt);
            sqlconn.Close();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                services.Add(Convert.ToInt32(dt.Rows[i][0]));
            }
            return services;
        }

        public static List<QuoteVM>? GetQuotes(int? service_type)
        {
            List<QuoteVM>? quotes = new();

            SqlConnection sqlconn = new(GetConnectionString());
            string sqlQuery = "select * from quote" +
                $"\r\n join contractor_service on job_type = contractor_service.id" +
                $"\r\n Where job_type = {service_type}";
            SqlCommand cmnd = new(sqlQuery, sqlconn);
            sqlconn.Open();
            SqlDataAdapter adapter = new(cmnd);
            DataTable dt = new();
            adapter.Fill(dt);
            sqlconn.Close();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                QuoteVM quote = new()
                {
                    Id = Convert.ToInt32(dt.Rows[i][0]),
                    Firstname = (string?)dt.Rows[i][1],
                    Lastname = (string?)dt.Rows[i][2],
                    Email = (string?)dt.Rows[i][3],
                    Zip = Convert.ToInt32(dt.Rows[i][5]),
                    //Address = (string?)dt.Rows[i][6],
                    Details = (string?)dt.Rows[i][7],
                    Service = (string?)dt.Rows[i][12]
                };
                quotes.Add(quote);
            }
            return quotes;
        }

        public static ContractorVM GetContractor(string email)
        {
            var contractor = new ContractorVM();
            SqlConnection sqlconn = new(GetConnectionString());
            string sqlQuery = "select contractor_id, FirstName, LastName from AspNetUsers" +
                $"\r\n Where UserName = '{email}'";
            SqlCommand cmnd = new(sqlQuery, sqlconn);
            sqlconn.Open();
            SqlDataAdapter adapter = new(cmnd);
            DataTable dt = new();
            adapter.Fill(dt);
            sqlconn.Close();
            contractor.Id = Convert.ToInt32(dt.Rows[0][0]);
            contractor.FirstName = dt.Rows[0][1].ToString();
            contractor.LastName = dt.Rows[0][2].ToString();
            contractor.Email = email;
            contractor.Services = GetServicesoffered(contractor.Id);
            foreach (var service in contractor.Services)
            {
                List<QuoteVM> quotes = GetQuotes(service);
                if (quotes != null && quotes.Count > 0)
                {
                    //contractor.Quotes = new List<QuoteVM>();
                    foreach (QuoteVM quote in quotes)
                    {
                        if (quote != null)
                        {
                            contractor.Quotes.Add(quote);
                        }
                    }
                }
            }
            return contractor;
        }

        public static List<byte[]>? GetQuoteImages(int? quoteID)
        {
            List<byte[]>? quoteImages = new();

            string query = $"select q_image from quote_image " +
                $"\b\n where quote_id = {quoteID};";

            DataTable dt = GetDataTable(query);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                quoteImages.Add((byte[])dt.Rows[i][0]);
            }
            return quoteImages;
        }

        public static List<OrdersVM> GetOrders(int? co_id)
        {
            List<OrdersVM> orders = new();

            using (SqlConnection sqlconn = new(GetConnectionString()))
            {
                string sqlQuery = "SELECT * FROM Orders WHERE co_id = @co_id";
                SqlCommand cmnd = new(sqlQuery, sqlconn);
                cmnd.Parameters.AddWithValue("@co_id", co_id);

                sqlconn.Open();
                using SqlDataReader reader = cmnd.ExecuteReader();
                while (reader.Read())
                {
                    OrdersVM order = new()
                    {
                        Id = reader.GetInt32(0),
                        Co_id = reader.IsDBNull(1) ? null : (int?)reader.GetInt32(1),
                        Case_id = reader.IsDBNull(2) ? null : (int?)reader.GetInt32(2),
                        Total = (float?)(reader.IsDBNull(3) ? null : (decimal?)reader.GetDecimal(3)),
                        //Created = reader.IsDBNull(4) ? null : (DateTime?)reader.GetDateTime(4)
                    };
                    orders.Add(order);
                }
            }
            return orders;
        }

        public static List<Service> ServiceSelection(string table)
        {
            // new list that will be retunred for drop down menu
            List<Service> list = new();

            // query table for values to populate list using column and table parameters
            SqlConnection sqlconn = new(GetConnectionString());
            string sqlquery = $"select * from {table}";
            SqlCommand sqlcomm = new(sqlquery, sqlconn);
            sqlconn.Open();
            SqlDataAdapter adapter = new(sqlcomm);
            DataTable dt = new();
            adapter.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Service service = new Service();

                service.Id = Convert.ToInt32(dt.Rows[i][0]);
                service.Name = (string?)(dt.Rows[i][1]);
                service.IsOffered = false;
                list.Add(service);

            }
            sqlconn.Close();
            return list;
        }

    }
}