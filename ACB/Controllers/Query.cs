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

            

            var connString = configuration.GetConnectionString("InterserverSQL");

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



        public static int Insert(string insertQuery)
        {
            int test =0;
            //open db connection and run passed in query
            SqlConnection sqlconn = new SqlConnection(GetConnectionString());
            SqlCommand sqlquery = new SqlCommand(insertQuery, sqlconn);
            sqlconn.Open();
            //sqlquery.ExecuteNonQuery();
            //return the id of the inserted row for foreign key use
            test = Convert.ToInt32(sqlquery.ExecuteScalar());
            System.Diagnostics.Debug.WriteLine("Correct Quote Id?????" + test);
            sqlconn.Close();
            return test;

        }

        public static int NewQuote(Quote quote)
        {
            
            string query = $"insert into quote (client_first_name, client_last_name, client_email, details, job_zip, job_type) " +
                $"\r\noutput inserted.id " +
                $"\r\nvalues ('{quote.client_first_name}' ,'{quote.client_last_name}', '{quote.client_email}'," +
                $"\r\n'{quote.details}','{quote.zip}', {quote.service});";
            System.Diagnostics.Debug.WriteLine(query);
            return Insert(query);
        } 

        public static int GetDBId(string name, string table, string field)
        {
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



    }
}
