using System;
using System.Collections.Generic;

namespace SnowJoe
{
    public interface IOrderManager
    {
        public void AddDeal(DealItem dealItem);
        public void AddItem(Item item);
        public void AddOrder(Order order);
        public void PrintItems();
        public void PrintOrders();
        public void RemoveItem(string itemNumber);
        public List<Order> GetAllOrders();
        public List<DealItem> GetAllItems();
    }
}