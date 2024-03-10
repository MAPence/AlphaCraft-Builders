﻿namespace ACB.Models
{
    public class DashboardQuote
    {
        //Displays a list of Quotes on the Dashboard
        public int id { get; set; }
        public string? client_first_name { get; set; }
        public string? client_last_name { get; set; }
        public string? client_email { get; set; }
        public DateTime? created { get; set; }
        public string? details { get; set; }
        public string? address { get; set; }
        public int zip { get; set; }

        public string? city { get; set; }
        public string? state { get; set; }
        public int? service { get; set; }
        // Usage to show list
        public IList<Quote> Quotes { get; set; }
    }
}