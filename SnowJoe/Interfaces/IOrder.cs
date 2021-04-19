using System;
using System.Collections.Generic;

namespace SnowJoe
{
    public interface IOrder
    {
        public string OrderId { get; set; }
        public DateTime DateCreated { get; set; }
        public string ItemNumber { get; set; }
        public int QuantityOrdered { get; set; }
        public List<Order> Orders { get; set; }
    }
}