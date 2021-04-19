using System;
using System.Collections.Generic;
using System.Text;

namespace SnowJoe
{
    public class Item
    {
        public string ItemNumber { get; set; }
        public int AvailableQuantity { get; set; }
        public decimal Price { get; set; }
        public bool IsDealItem { get; set; }
    }
}
