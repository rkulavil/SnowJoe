namespace SnowJoe
{
    public interface ICalc
    {
        public decimal GetProfit(Order order, DealItem dealItem);
        public decimal GetProfit(string orderId, string itemNumber, bool isDealItem);
        public decimal GetProfit(Order order, Item item);
        public decimal GetProfitLoss(IOrderManager orderManager);
        public decimal GetProfitLoss();
    }
}