using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SnowJoe
{
    public class Calc : ICalc
    {
        private readonly IOrder _order;
        private readonly IDealItem _item;

        public Calc(IOrder order, IDealItem item)
        {
            _order = order;
            _item = item;
        }

        /// <summary>
        /// Returns the profit made from an order for an item.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public decimal GetProfit(Order order, Item item)
        {
            decimal totalProfit = default(decimal);
            try
            {
                totalProfit = order.QuantityOrdered * (item.Price * 20 / 100);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            return totalProfit;
        }

        /// <summary>
        /// Returns the profit made from an order determined by OrderId for an item determined by Item Number.
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="itemNumber"></param>
        /// <returns></returns>
        public decimal GetProfit(string orderId, string itemNumber, bool isDealItem)
        {
            decimal totalProfit = default(decimal);
            try
            {
                var order = _order.Orders.Where(x => x.OrderId == orderId && x.ItemNumber == itemNumber).SingleOrDefault();

                if (!Object.ReferenceEquals(order, null))
                {
                    if (isDealItem)
                    {
                        var dealItem = _item.Items.Where(x => x.ItemNumber == itemNumber && x.IsDealItem).SingleOrDefault();
                        if (!Object.ReferenceEquals(dealItem, null))
                        {
                            totalProfit = GetProfit(order, dealItem);
                        }
                        else
                            Console.WriteLine($"Item Number {itemNumber} is not present in list of deals.");
                    }
                    else
                    {
                        var item = _item.Items.Where(x => x.ItemNumber == itemNumber && !x.IsDealItem).SingleOrDefault();
                        if (!Object.ReferenceEquals(item, null))
                        {
                            totalProfit = GetProfit(order, item);
                        }
                        else
                            Console.WriteLine($"Item Number {itemNumber} is not present in list of items.");
                    }
                }
                else
                    Console.WriteLine($"Item Number {itemNumber} is not present in the order Id {orderId}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            return totalProfit;
        }

        /// <summary>
        /// Returns the profit made on an order from a deal item.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="dealItem"></param>
        /// <returns></returns>
        public decimal GetProfit(Order order, DealItem dealItem)
        {
            decimal totalProfit = default(decimal);
            try
            {
                totalProfit = order.QuantityOrdered * (dealItem.Price * 20 / 100) - (dealItem.Price - dealItem.Price * (1 - 1 * dealItem.Discount / 100));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            return totalProfit;
        }

        /// <summary>
        /// Returns the profit made from all the orders placed.
        /// </summary>
        /// <param name="orderManager"></param>
        /// <returns></returns>
        /// This method is constructed just to match the document given. A more efficient way to calculate profit from all orders is to call GetProfit() method.
        public decimal GetProfitLoss(IOrderManager orderManager)
        {
            decimal totalProfit = default(decimal);
            try
            {
                var orders = orderManager.GetAllOrders();
                var items = orderManager.GetAllItems();

                foreach (var order in orders)
                {
                    var discountItem = _item.Items.Where(x => x.ItemNumber == order.ItemNumber).SingleOrDefault();

                    if (Object.ReferenceEquals(discountItem, null))
                        Console.WriteLine($"Item Number {order.ItemNumber} present in the list of orders is not available in the list of Items.");

                    //check the if the date of order is between the start and end date of the deal.
                    if (discountItem.IsDealItem && DateTime.Compare(order.DateCreated, discountItem.StartDate) >= 0 && DateTime.Compare(order.DateCreated, discountItem.EndDate) <= 0)
                        totalProfit = GetProfit(order, discountItem);
                    else
                        totalProfit += GetProfit(order, (Item)discountItem);

                }
                Console.WriteLine($"Total Profit: {totalProfit}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            return totalProfit;
        }

        /// <summary>
        /// Returns the profit made from all the orders placed.
        /// </summary>
        /// <returns></returns>
        public decimal GetProfitLoss()
        {
            decimal totalProfit = default(decimal);
            try
            {
                if (_order.Orders.Count == 0)
                    Console.WriteLine("There are no orders.");

                foreach (var order in _order.Orders)
                {
                    var discountItem = _item.Items.Where(x => x.ItemNumber == order.ItemNumber).SingleOrDefault();

                    if (Object.ReferenceEquals(discountItem, null))
                        Console.WriteLine($"Item Number {order.ItemNumber} present in the list of orders is not available in the list of Items.");
                    //check the if the date of order is between the start and end date of the deal.
                    if (discountItem.IsDealItem && DateTime.Compare(order.DateCreated, discountItem.StartDate) >= 0 && DateTime.Compare(order.DateCreated, discountItem.EndDate) <= 0)
                        totalProfit += GetProfit(order, discountItem);
                    else
                        totalProfit += GetProfit(order, (Item)discountItem);

                }
                Console.WriteLine($"Total Profit: {totalProfit}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            return totalProfit;
        }
    }
}
