using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using StockMarket.Domain.Comparers;

[assembly: InternalsVisibleTo("StockMarket.Domain.Tests")]
[assembly: InternalsVisibleTo("StockMarket.Data.Tests")]

namespace StockMarket.Domain
{
    //amiram :)
    public abstract class StockMarketProcessor : IStockMarketProcessor
    {
        private long lastOrderId;
        private long lastTradeId;
        private readonly List<Order> orders;
        private readonly List<Trade> trades;
        private readonly PriorityQueue<Order, Order> buyOrders;
        private readonly PriorityQueue<Order, Order> sellOrders;
        private ConcurrentDictionary<Guid, MatchContext> contexts;

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
            contexts = new();
            foreach (var order in this.orders)
            {
                enqueueOrder(order);
            }
        }
        public MatchContext? TakeContextBy(Guid refId)
        {
            contexts.TryRemove(refId, out var value);
            return value;
        }

        internal long EnqueueOrder(TradeSide side, decimal price, decimal quantity, Guid? refId = null, MatchContext? context = null)
        {
            context ??= new MatchContext();

            var order = makeOrder(side, price, quantity);
            context.createdOrder = order.Clone();
            enqueueOrder(order, context);

            if (refId.HasValue) contexts.TryAdd(refId.Value, context);

            return order.Id;
        }

        internal long CancelOrder(long orderId, Guid? refId = null, MatchContext? context = null)
        {
            context ??= new MatchContext();
            
            var order = orders.Single(order => order.Id == orderId);
            order.Cancel();
            context.updatedOrders.Add(order.Clone());
            
            if (refId.HasValue) contexts.TryAdd(refId.Value, context);

            return order.Id;
        }

        internal long ModifyOrder(long orderId, decimal price, decimal quantity, Guid? refId = null)
        {
            var context = new MatchContext();

            var order = orders.Single(order => order.Id == orderId);
            CancelOrder(order.Id, context: context);

            var newOrderId = EnqueueOrder(order.Side, price, quantity, context: context);
            if (refId.HasValue) contexts.TryAdd(refId.Value, context);
            
            return newOrderId;
        }

        private Order makeOrder(TradeSide side, decimal price, decimal quantity)
        {
            Interlocked.Increment(ref lastOrderId);
            var order = new Order(lastOrderId, side, price, quantity);
            orders.Add(order);
            return order;
        }

        private void enqueueOrder(Order order, MatchContext? context = null)
        {
            if (order.Side == TradeSide.Buy)
            {
                matchOrder(
                order: order,
                orders: buyOrders,
                matchingOrders: sellOrders,
                comparePriceDelegate: (decimal price1, decimal price2) => price1 <= price2,
                context: context);
            }
            else
            {
                matchOrder(
                order: order,
                orders: sellOrders,
                matchingOrders: buyOrders,
                comparePriceDelegate: (decimal price1, decimal price2) => price1 >= price2,
                context: context);
            }
        }

        private void matchOrder(Order order,
                                PriorityQueue<Order, Order> orders,
                                PriorityQueue<Order, Order> matchingOrders,
                                Func<decimal, decimal, bool> comparePriceDelegate,
                                MatchContext? context)
        {
            while ((matchingOrders.Count > 0) && (order.Quantity > 0) && comparePriceDelegate(matchingOrders.Peek().Price, order.Price))
            {
                var peekedOrder = matchingOrders.Peek();

                if (peekedOrder.IsCanceled)
                {
                    matchingOrders.Dequeue();
                    continue;
                }

                makeTrade(peekedOrder, order, context);

                if (peekedOrder.Quantity == 0) matchingOrders.Dequeue();
            }

            if (order.Quantity > 0) orders.Enqueue(order, order);
        }

        private void makeTrade(Order order1, Order order2, MatchContext? context)
        {
            var matchingOrders = findOrders(order1, order2);
            var minQuantity = Math.Min(matchingOrders.SellOrder.Quantity, matchingOrders.BuyOrder.Quantity);

            Interlocked.Increment(ref lastTradeId);
            var trade = new Trade(
                id: lastTradeId,
                sellOrderId: matchingOrders.SellOrder.Id,
                buyOrderId: matchingOrders.BuyOrder.Id,
                price: matchingOrders.SellOrder.Price,
                quantity: minQuantity);

            trades.Add(trade);
            matchingOrders.SellOrder.DecreaseQuantity(minQuantity);
            matchingOrders.BuyOrder.DecreaseQuantity(minQuantity);

            context?.createdTrades.Add(trade.Clone());
            context?.updatedOrders.Add(matchingOrders.BuyOrder.Clone());
            context?.updatedOrders.Add(matchingOrders.SellOrder.Clone());
        }

        private static (Order SellOrder, Order BuyOrder) findOrders(Order order1, Order order2)
        {
            if (order1.Side == TradeSide.Sell) return (SellOrder: order1, BuyOrder: order2);
            return (SellOrder: order2, BuyOrder: order1);
        }

    }
}