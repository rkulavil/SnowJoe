using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnowJoe
{
    public class OrderManager : IOrderManager
    {
        private readonly IOrder _order;
        private readonly IDealItem _item;

        public OrderManager(IOrder order, IDealItem dealItem)
        {
            _item = dealItem;
            _order = order;
        }

        public List<Order> GetAllOrders()
        {
            return _order.Orders;
        }

        public List<DealItem> GetAllItems()
        {
            return _item.Items;
        }
        /// <summary>
        /// Adds an item to the list of items.
        /// </summary>
        /// <param name="item">item to be added.</param>
        public void AddItem(Item item)
        {
            try
            {
                if (!_item.Items.Any(x => x.ItemNumber == item.ItemNumber))
                {
                    _item.Items.Add(new DealItem { ItemNumber = item.ItemNumber, AvailableQuantity = item.AvailableQuantity, Price = item.Price });
                    Console.WriteLine($"Item Number {item.ItemNumber} is added to the list of items.");
                }
                else
                {
                    Console.WriteLine("Duplicate ItemNumber. The item you are trying to add is already available in the list of items.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
        /// <summary>
        /// Removes an item from the list of items.
        /// </summary>
        /// <param name="itemNumber">Item Number representing the item to be deleted.</param>
        public void RemoveItem(string itemNumber)
        {
            try
            {
                if (_item.Items.Count == 0)
                {
                    Console.WriteLine("The list of items is empty");
                }
                else if (_item.Items.Any(i => i.ItemNumber == itemNumber))
                {
                    _item.Items.RemoveAll(x => x.ItemNumber == itemNumber);
                    _order.Orders.RemoveAll(o => o.ItemNumber == itemNumber);
                    Console.WriteLine($"Item {itemNumber} has been removed from the list of items and corresponding orders have been cancelled.");
                }
                else
                {
                    Console.WriteLine($"Item Number {itemNumber} is invalid.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Adds an item to the list of deals.
        /// </summary>
        /// <param name="dealItem">item to be added as deal</param>
        public void AddDeal(DealItem dealItem)
        {
            try
            {
                if (DateTime.Compare(dealItem.EndDate, dealItem.StartDate) < 0)
                    Console.WriteLine("Deal Item's End Date has to be greater than Start Date.");
                else if (!_item.Items.Any(di => di.ItemNumber == dealItem.ItemNumber))
                {
                    dealItem.IsDealItem = true;
                    _item.Items.Add(dealItem);
                    Console.WriteLine($"Item Number {dealItem.ItemNumber} is added to the list of deal items.");
                }
                else
                    Console.WriteLine($"Deal already exists for Item Number: {dealItem.ItemNumber}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Adds order to the list of orders.
        /// </summary>
        /// <param name="order">order to be added.</param>
        public void AddOrder(Order order)
        {
            try
            {
                if (_item.Items.Any(i => i.ItemNumber == order.ItemNumber))
                {
                    if (_item.Items.Any(i => i.AvailableQuantity >= order.QuantityOrdered))
                    {
                        order.OrderId = Guid.NewGuid().ToString();
                        _order.Orders.Add(order);
                        _item.Items[_item.Items.FindIndex(i => i.ItemNumber == order.ItemNumber)].AvailableQuantity -= order.QuantityOrdered;
                        Console.WriteLine($"Item Number {order.ItemNumber} is added to the list of orders.");
                    }
                    else
                    {
                        Console.WriteLine($"Item: {order.ItemNumber} is currently out of stock.");
                    }
                }
                else
                {
                    Console.WriteLine($"Item: {order.ItemNumber} is an invalid item.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Prints in a new line all items with their ItemNumber-AvailableQuantity-Price properties separated with dash.
        /// </summary>
        public void PrintItems()
        {
            try
            {
                if (_item.Items?.Count > 0)
                {
                    Console.WriteLine("The list of Items are (Item Number - Available Quantity - Price - Is Available in Deal?):");
                    _item.Items.ForEach(item => Console.WriteLine($"{item.ItemNumber}-{item.AvailableQuantity}-{item.Price}-{(item.IsDealItem ? "Yes" : "No")}"));
                }
                else
                    Console.WriteLine("The list of items is empty.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Prints in a new line all orders with their properties separated with dash.
        /// </summary>
        public void PrintOrders()
        {
            try
            {
                if (_order.Orders?.Count > 0)
                {
                    Console.WriteLine("The list of Items are (Order Id - Date Created - Item Number - Quantity Ordered)");
                    _order.Orders.ForEach(order => Console.WriteLine($"{order.OrderId}-{order.DateCreated}-{order.ItemNumber}-{order.QuantityOrdered}"));
                }
                else
                    Console.WriteLine("The list of orders is empty.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
