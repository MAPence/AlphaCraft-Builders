namespace ACB.Models
{   
    //this class is used to hold a label and id from a table, useful in dropdown menus
    public class Pair
    {

        public int? Value { get; set; }
        public string? Name { get; set; }

        public Pair()
        {
            Name = null;
            Value = 0;
        }

        public Pair(string name, int value = 0)
        {
            Name = name;
            Value = value;
        }
    }
}
