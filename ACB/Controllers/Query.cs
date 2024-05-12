using ACB.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using MessagePack.Resolvers;

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

        public static string FormatString(string original)
        {
            string corrected = original;
            int index = 1;
            for (int i = 0; i < original.Length; i++)
            {
                if (original[i] == '\'')
                {
                    corrected = corrected.Insert(i + index, "\'");
                    index++;
                }
            }

            return corrected;
        }

        public static DataTable GetDataTable(string query)
        {
            SqlConnection sqlconn = new(GetConnectionString());
            
            SqlCommand cmnd = new(query, sqlconn);
            sqlconn.Open();
            SqlDataAdapter adapter = new(cmnd);
            DataTable dt = new();
            adapter.Fill(dt);
            sqlconn.Close();
            return dt;
        }

        public static List<SelectListItem> GetOptions(string query)
        {
            // new list that will be retunred for drop down menu
            List<SelectListItem> list = new();
            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "General Expense"
            });
            DataTable dt = GetDataTable(query);
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                // add value to list
                SelectListItem option = new()
                {
                    Value = Convert.ToInt32(dt.Rows[i][0]).ToString(),
                    Text = $"{dt.Rows[i][1].ToString()} - {Convert.ToInt32(dt.Rows[i][0]).ToString()}"
                };

                list.Add(option);

            }
    
            return list;
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

        public static void DeleteQuery(string table, string col, string val)
        {
            string query = $"Delete from {table} where {col} = {val};";

            //open db connection and run passed in query
            SqlConnection sqlconn = new(GetConnectionString());
            SqlCommand sqlquery = new(query, sqlconn);
            
            sqlconn.Open();
            sqlquery.ExecuteNonQuery();
            sqlconn.Close();
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
            quote.Details = FormatString(quote.Details);    
            //create query statement, insert quote and return new id
            string query = $"insert into quote (client_first_name, client_last_name, client_email, job_address, city, State, job_zip, details, job_type, latitude, longitude) " +
                $"\r\noutput inserted.id " +
                $"\r\nvalues ('{quote.Client_first_name}' ,'{quote.Client_last_name}', '{quote.Client_email}'," +
                $"\r\n'{quote.Address}', '{quote.City}', '{quote.State}', {quote.Zip}, " +
                $"\r\n'{quote.Details}', {quote.Service}, {quote.LatLong.Lat}, {quote.LatLong.Long});";

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

        public static List<QuoteVM>? GetQuotes(int? service_type, LatLong? latlong, int dist)
        {
            List<QuoteVM>? quotes = new();

            SqlConnection sqlconn = new(GetConnectionString());
            string sqlQuery = $"Declare @LocStart GEOGRAPHY = GEOGRAPHY::Point({latlong.Lat},{latlong.Long}, 4326)" +
                "select * from quote" +
                $"\r\n join contractor_service on job_type = contractor_service.id" +
                $"\r\n Where job_type = {service_type}" +
                $"\r\n and latitude is not null" +
                $"\r\n and  @LocStart.STDistance(GEOGRAPHY::Point(latitude, longitude, 4326))/1609.344 <= {dist};";
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
                    Address = $"{(string?)dt.Rows[i][6]} {(string?)dt.Rows[i][13]}, {(string?)dt.Rows[i][14]}",
                    Details = (string?)dt.Rows[i][7],
                    Service = (string?)dt.Rows[i][16]
                };
                quotes.Add(quote);
            }
            return quotes;
        }

        public static ContractorVM GetContractor(string email)
        {
            var contractor = new ContractorVM();
            SqlConnection sqlconn = new(GetConnectionString());
            string sqlQuery = "select contractor_id, FirstName, LastName, Company, Address, City, State, Zip, latitude, longitude from AspNetUsers" +
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
            contractor.Company = dt.Rows[0][3].ToString();
            contractor.Address = dt.Rows[0][4].ToString();
            contractor.City = dt.Rows[0][5].ToString();
            contractor.State = dt.Rows[0][6].ToString();
            contractor.Zip = Convert.ToInt32(dt.Rows[0][7]);
            contractor.LatLong.Lat = (decimal)dt.Rows[0][8];
            contractor.LatLong.Long = (decimal)dt.Rows[0][9];
            contractor.Email = email;
            contractor.Services = GetServicesoffered(contractor.Id);
            foreach (var service in contractor.Services)
            {
                List<QuoteVM> quotes = GetQuotes(service, contractor.LatLong, 25);
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

        public static List<NewOrder> GetOrders(int? co_id)
        {
            List<NewOrder> orders = new();

            using (SqlConnection sqlconn = new(GetConnectionString()))
            {
                string sqlQuery = "SELECT * FROM Orders WHERE co_id = @co_id";
                SqlCommand cmnd = new(sqlQuery, sqlconn);
                cmnd.Parameters.AddWithValue("@co_id", co_id);

                sqlconn.Open();
                using SqlDataReader reader = cmnd.ExecuteReader();
                while (reader.Read())
                {
                    NewOrder order = new()
                    {
                        Id = reader.GetInt32(0),
                        Co_id = reader.IsDBNull(1) ? null : (int?)reader.GetInt32(1),
                        Case_id = reader.IsDBNull(2) ? null : (int?)reader.GetInt32(2),

                        Subtotal = (decimal?)(reader.IsDBNull(3) ? null : (decimal?)reader.GetDecimal(3)),
                        SalesTax = (decimal?)(reader.IsDBNull(4) ? null : (decimal?)reader.GetDecimal(4)),
                        Created = (DateTime?)(reader.IsDBNull(6) ? null : (DateTime?)reader.GetDateTime(6)),
                        
                    };
                    order.Total = order.Subtotal + order.SalesTax;
                    orders.Add(order);
                }
            }
            return orders;
        }
        public static List<JobVM> GetJobs(int? co_id)
        {
            List<JobVM> jobs = new();

            using (SqlConnection sqlconn = new(GetConnectionString()))
            {
                string sqlQuery = "SELECT * FROM Job WHERE contractor_id = @co_id";
                SqlCommand cmnd = new(sqlQuery, sqlconn);
                cmnd.Parameters.AddWithValue("@co_id", co_id);

                sqlconn.Open();
                using SqlDataReader reader = cmnd.ExecuteReader();
                while (reader.Read())
                {
                    JobVM job = new()
                    {
                        Id = reader.GetInt32(0),
                        Firstname = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Lastname = reader.IsDBNull(4) ? null : reader.GetString(4),
                        Address = reader.IsDBNull(8) ? null : reader.GetString(8),    
                        Start = reader.IsDBNull(12) ? null : (DateTime?)reader.GetDateTime(12),
                        Amount = reader.IsDBNull(11) ? null : (decimal?)reader.GetDecimal(11)
                    };
                    jobs.Add(job);
                }
            }
            return jobs;
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

        public static void UpdateServices(int? Id, int[] services)
        {
            DeleteQuery("contractor_service_offered", "contractor_id", Id.ToString());

            foreach(var service in services)
            {
                string newService = $"insert into contractor_service_offered (contractor_id, service_Id)" +
                    $"\b\n values ({Id}, {service});";
                Insert(newService);
            }
        }

        public static JobVM ConvertQuote(int id)
        {
            string query = "select * from quote" +
                "\r\njoin contractor_service on job_type = contractor_service.Id" +
                $"\r\nwhere quote.id = {id};";

            DataTable dt = GetDataTable(query);

            JobVM job = new JobVM()
            {
                Firstname = (string?)dt.Rows[0][1],
                Lastname = (string?)dt.Rows[0][2],
                Email = (string?)dt.Rows[0][3],
                Service = (string?)dt.Rows[0][16],
                Address = (string?)dt.Rows[0][6],
                City = (string?)dt.Rows[0][13],
                State = (string?)dt.Rows[0][14],
                Zip = Convert.ToInt32(dt.Rows[0][5]),
                Details = (string?)dt.Rows[0][7],
            };
            return job;
        }

        public static List<ContractorTile> FindContractors(string service, decimal? latitude, decimal? longitude, int? distance) 
        { 
            List<ContractorTile> contractors = new List<ContractorTile>();

            string query = $"Declare @LocStart GEOGRAPHY = GEOGRAPHY::Point({latitude},{longitude}, 4326);" +
                "\r\nselect contractor_service_offered.contractor_id, Company, Email, latitude, longitude, service_type from contractor_service_offered" +
                "\r\njoin AspNetUsers on ASPNetUsers.contractor_id = contractor_service_offered.contractor_id" +
                "\r\njoin contractor_service on contractor_service.Id = service_id" +
                $"\r\nwhere service_type = '{service}'" +
                $"\r\nand @LocStart.STDistance(GEOGRAPHY::Point(latitude, longitude, 4326))/1609.344 <= {distance};";

            ContractorTile t = new()
            {
                Id = 0,
                Company = query,
                Email = "Hello"
            };

            contractors.Add(t);

            DataTable dt = GetDataTable(query); 

            for(int i = 0; i < dt.Rows.Count; i++)
            {
                ContractorTile c = new()
                {
                    Id = Convert.ToInt32(dt.Rows[i][0]),
                    Company = (string)dt.Rows[i][1],
                    Email = (string)dt.Rows[i][2],
                    
                };
                contractors.Add(c);
            }

            return contractors;
        
        }
    }
}