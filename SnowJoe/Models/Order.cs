using System;
using System.Collections.Generic;
using System.Text;

namespace SnowJoe
{
    public class Order : IOrder
    {
        private List<Order> orders = new List<Order>();

        public string OrderId { get; set; }
        public DateTime DateCreated { get; set; }
        public string ItemNumber { get; set; }
        public int QuantityOrdered { get; set; }
        public List<Order> Orders { get => orders; set => orders = value; }
    }
}
