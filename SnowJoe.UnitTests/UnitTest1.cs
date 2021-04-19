using System.Linq;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace SnowJoe.UnitTests
{
    public class Tests
    {
        private IOrderManager _orderManager;

        private ICalc _calc;

        private IOrder _order;

        private IDealItem _item;

        private Fixture _fixture;

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IOrder, Order>();
            services.AddSingleton<IDealItem, DealItem>();
            services.AddSingleton<IOrderManager, OrderManager>();
            services.AddSingleton<ICalc, Calc>();
        }

        [SetUp]
        public void Setup()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var _serviceProvider = serviceCollection.BuildServiceProvider();
            _orderManager = _serviceProvider.GetService<IOrderManager>();
            _calc = _serviceProvider.GetService<ICalc>();
            _order = _serviceProvider.GetService<IOrder>();
            _item = _serviceProvider.GetService<IDealItem>();
            _fixture = new Fixture();
        }

        [Test]
        public void AddItem_Should_Add_A_New_Item_To_List()
        {
            var itemFixture = _fixture.Create<Item>();
            _orderManager.AddItem(itemFixture);

            Assert.AreEqual(1, _item.Items.Count);
            Assert.AreEqual(itemFixture.ItemNumber, _item.Items[0].ItemNumber);
            Assert.AreEqual(itemFixture.AvailableQuantity, _item.Items[0].AvailableQuantity);
            Assert.AreEqual(itemFixture.Price, _item.Items[0].Price);
            Assert.AreEqual(false, _item.Items[0].IsDealItem);
        }

        [Test]
        public void RemoveItem_Should_Remove_Added_Item_From_List()
        {
            var dummyItem = new Item { ItemNumber = "item01" };

            _orderManager.AddItem(dummyItem);

            Assert.True(_item.Items.Any(x => x.ItemNumber == "item01"));

            _orderManager.RemoveItem("item01");
            Assert.False(_item.Items.Any(x => x.ItemNumber == "item01"));
        }

        [Test]
        public void AddDeal_Should_Add_Deal_Item_To_The_List()
        {
            var dealItem = new DealItem { ItemNumber = "deal01" };

            _orderManager.AddDeal(dealItem);

            var deal = _item.Items.Where(x => x.ItemNumber == dealItem.ItemNumber).SingleOrDefault();

            Assert.AreEqual(true, deal.IsDealItem);
        }

        [Test]
        public void AddOrder_Should_Add_Order_To_The_List()
        {
            var itemFixture = new Item { ItemNumber = "orderItem", AvailableQuantity = 2 };
            _orderManager.AddItem(itemFixture);

            var orderFixture = new Order { ItemNumber = "orderItem", QuantityOrdered = 3 };
            _orderManager.AddOrder(orderFixture);

            Assert.False(_order.Orders.Any(x => x.ItemNumber == "orderItem"));

            orderFixture = new Order { ItemNumber = "orderItem", QuantityOrdered = 1 };
            _orderManager.AddOrder(orderFixture);

            Assert.True(_order.Orders.Any(x => x.ItemNumber == "orderItem"));
        }

        [Test]
        public void PrintItems_Should_Print_Items()
        {
            Assert.DoesNotThrow(() => _orderManager.PrintItems());
        }

        [Test]
        public void PrintItems_Should_Print_Orders()
        {
            Assert.DoesNotThrow(() => _orderManager.PrintOrders());
        }
    }
}