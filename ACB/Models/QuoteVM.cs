﻿namespace ACB.Models
{
    public class QuoteVM
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set;}    
        public string? Email { get; set;}
        public string? Service { get; set;}
        public List<Byte[]>? Images { get; set; }

    }
}