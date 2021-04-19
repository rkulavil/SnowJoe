using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace SnowJoe
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);
                var _serviceProvider = serviceCollection.BuildServiceProvider();
                var orderManager = _serviceProvider.GetService<IOrderManager>();
                var calc = _serviceProvider.GetService<ICalc>();
                int choice = default(int);
                var selection = true;
                
                ShowSelections();

                while (selection)
                {
                    int.TryParse(Console.ReadLine(), out choice);

                    switch (choice)
                    {
                        case 1:
                            var newItem = new Item();
                            Console.Write("Enter Item Number: ");
                            newItem.ItemNumber = Console.ReadLine();
                            Console.Write("Enter Available Quantity: ");
                            newItem.AvailableQuantity = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Enter Price: ");
                            newItem.Price = Convert.ToDecimal(Console.ReadLine());
                            orderManager.AddItem(newItem);
                            break;
                        case 2:
                            Console.Write("Enter Item Number: ");
                            var itemNumber = Console.ReadLine();
                            orderManager.RemoveItem(itemNumber);
                            break;
                        case 3:
                            var newDeal = new DealItem();
                            Console.Write("Enter Deal Item Number: ");
                            newDeal.ItemNumber = Console.ReadLine();

                            Console.Write("Enter Available Quantity: ");
                            newDeal.AvailableQuantity = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Enter Price: ");
                            newDeal.Price = Convert.ToDecimal(Console.ReadLine());

                            Console.Write("Enter Start Date (MM/dd/yyyy): ");
                            DateTime.TryParse(Console.ReadLine(), out DateTime startDate);
                            newDeal.StartDate = startDate;

                            Console.Write("Enter End Date (MM/dd/yyyy): ");
                            DateTime.TryParse(Console.ReadLine(), out DateTime endDate);
                            newDeal.EndDate = endDate;

                            Console.Write("Enter Discount (percentage): ");
                            newDeal.Discount = Convert.ToDecimal(Console.ReadLine());

                            orderManager.AddDeal(newDeal);
                            break;
                        case 4:
                            var order = new Order { DateCreated = DateTime.Now };

                            Console.Write("Enter Item Number to Order: ");
                            order.ItemNumber = Console.ReadLine();

                            Console.Write("Enter Quantity: ");
                            order.QuantityOrdered = Convert.ToInt32(Console.ReadLine());

                            orderManager.AddOrder(order);
                            break;
                        case 5:
                            orderManager.PrintItems();
                            break;
                        case 6:
                            orderManager.PrintOrders();
                            break;
                        case 7:
                            Console.Write("Enter Order Id: ");
                            var orderId = Console.ReadLine();
                            Console.Write("Enter Item Number: ");
                            var itmNbr = Console.ReadLine();

                            Console.WriteLine($"Profit made from Order Number - {orderId} with Item Number - {itmNbr} is { calc.GetProfit(orderId, itmNbr, false) }");
                            break;
                        case 8:
                            Console.Write("Enter Order Id: ");
                            var ordrId = Console.ReadLine();
                            Console.Write("Enter Item Number: ");
                            var itemNbr = Console.ReadLine();

                            Console.WriteLine($"Profit made from Order Number - {ordrId} with Item Number - {itemNbr} is { calc.GetProfit(ordrId, itemNbr, true) }");
                            break;
                        case 9:
                            Console.WriteLine($"Profit made from all the existing orders is { calc.GetProfitLoss() }");
                            break;
                        case 10:
                            selection = false;
                            break;
                        default:
                            Console.WriteLine("Choose a valid option from the menu.");
                            break;
                    }

                    if (selection)
                    {
                        Console.WriteLine(Environment.NewLine);
                        ShowSelections();
                    }
                }
                Console.WriteLine("Process Exited");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IOrder, Order>();
            services.AddSingleton<IDealItem, DealItem>();
            services.AddSingleton<IOrderManager, OrderManager>();
            services.AddSingleton<ICalc, Calc>();
        }

        private static void ShowSelections()
        {
            Console.WriteLine("1. Add Item");
            Console.WriteLine("2. Remove Item");
            Console.WriteLine("3. Add Deal");
            Console.WriteLine("4. Add Order");
            Console.WriteLine("5. Print Items");
            Console.WriteLine("6. Print Orders");
            Console.WriteLine("7. Get Profit For An Item");
            Console.WriteLine("8. Get Profit For A Deal Item");
            Console.WriteLine("9. Get Profit For All Orders");
            Console.WriteLine("10. Exit");
            Console.Write("Select your option: ");
        }
    }
}
