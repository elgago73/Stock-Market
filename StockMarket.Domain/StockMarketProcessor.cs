using System.Runtime.CompilerServices;
using StockMarket.Domain.Comparers;

[assembly: InternalsVisibleTo("StockMarket.Domain.Tests")]
[assembly: InternalsVisibleTo("StockMarket.Data.Tests")]

namespace StockMarket.Domain
{
    //amiram :)
    public abstract class StockMarketProcessor
    {
        private long lastOrderId;
        private long lastTradeId;
        private readonly List<Order> orders;
        private readonly List<Trade> trades;
        private readonly PriorityQueue<Order, Order> buyOrders;
        private readonly PriorityQueue<Order, Order> sellOrders;

        public IEnumerable<Order> Orders => orders;
        public IEnumerable<Trade> Trades => trades;

        internal StockMarketProcessor(List<Order>? orders = null, long lastOrderId = 0, long lastTradeId = 0)
        {
            this.lastOrderId = lastOrderId;
            this.lastTradeId = lastTradeId;
            this.orders = orders ?? new();
            trades = new();
            buyOrders = new(new MaxComparer());
            sellOrders = new(new MinComparer());

            foreach (var order in this.orders)
            {
                enqueueOrder(order);
            }
        }

        internal long EnqueueOrder(TradeSide side, decimal price, decimal quantity)
        {
            var order = makeOrder(side, price, quantity);
            enqueueOrder(order);
            return order.Id;
        }

        internal long CancelOrder(long orderId)
        {
            var order = orders.Single(order => order.Id == orderId);
            order.Cancel();
            return order.Id;
        }

        internal long ModifyOrder(long orderId, decimal price, decimal quantity)
        {
            var order = orders.Single(order => order.Id == orderId);
            CancelOrder(order.Id);
            return EnqueueOrder(order.Side, price, quantity);
        }

        private Order makeOrder(TradeSide side, decimal price, decimal quantity)
        {
            Interlocked.Increment(ref lastOrderId);
            var order = new Order(lastOrderId, side, price, quantity);
            orders.Add(order);
            return order;
        }

        private void enqueueOrder(Order order)
        {
            if (order.Side == TradeSide.Buy)
            {
                matchOrder(
                order: order,
                orders: buyOrders,
                matchingOrders: sellOrders,
                comparePriceDelegate: (decimal price1, decimal price2) => price1 <= price2);
            }
            else
            {
                matchOrder(
                order: order,
                orders: sellOrders,
                matchingOrders: buyOrders,
                comparePriceDelegate: (decimal price1, decimal price2) => price1 >= price2);
            }
        }

        private void matchOrder(Order order,
                                PriorityQueue<Order, Order> orders,
                                PriorityQueue<Order, Order> matchingOrders,
                                Func<decimal, decimal, bool> comparePriceDelegate)
        {
            while ((matchingOrders.Count > 0) && (order.Quantity > 0) && comparePriceDelegate(matchingOrders.Peek().Price, order.Price))
            {
                var peekedOrder = matchingOrders.Peek();

                if (peekedOrder.IsCanceled)
                {
                    matchingOrders.Dequeue();
                    continue;
                }

                makeTrade(peekedOrder, order);

                if (peekedOrder.Quantity == 0) matchingOrders.Dequeue();
            }

            if (order.Quantity > 0) orders.Enqueue(order, order);
        }

        private void makeTrade(Order order1, Order order2)
        {
            var matchingOrders = findOrders(order1, order2);
            var minQuantity = Math.Min(matchingOrders.SellOrder.Quantity, matchingOrders.BuyOrder.Quantity);

            Interlocked.Increment(ref lastTradeId);
            trades.Add(new Trade(
                id: lastTradeId,
                sellOrderId: matchingOrders.SellOrder.Id,
                buyOrderId: matchingOrders.BuyOrder.Id,
                price: matchingOrders.SellOrder.Price,
                quantity: minQuantity));

            matchingOrders.SellOrder.DecreaseQuantity(minQuantity);
            matchingOrders.BuyOrder.DecreaseQuantity(minQuantity);
        }

        private static (Order SellOrder, Order BuyOrder) findOrders(Order order1, Order order2)
        {
            if (order1.Side == TradeSide.Sell) return (SellOrder: order1, BuyOrder: order2);
            return (SellOrder: order2, BuyOrder: order1);
        }
    }
}