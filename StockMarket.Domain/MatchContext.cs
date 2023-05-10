namespace StockMarket.Domain
{
    public class MatchContext
    {
        internal Order? createdOrder;
        internal readonly List<Order> updatedOrders;
        internal readonly List<Trade> createdTrades;

        public Order? CreatedOrder => createdOrder;
        public IEnumerable<Order> UpdatedOrders => updatedOrders;
        public IEnumerable<Trade> CreatedTrades => createdTrades;
        internal MatchContext()
        {
            createdOrder = null;
            updatedOrders = new List<Order>();
            createdTrades = new List<Trade>();
        }


    }
}