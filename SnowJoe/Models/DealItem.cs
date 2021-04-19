using System;
using System.Collections.Generic;
using System.Text;

namespace SnowJoe
{
    public class DealItem : Item, IDealItem
    {
        private List<DealItem> items = new List<DealItem>();
        
        public decimal Discount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<DealItem> Items { get => items; set => items = value; }
    }
}
