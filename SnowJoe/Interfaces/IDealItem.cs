using System;
using System.Collections.Generic;

namespace SnowJoe
{
    public interface IDealItem
    {
        public decimal Discount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<DealItem> Items { get; set; }
    }
}