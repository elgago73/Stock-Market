using StockMarket.Domain.States;

/*
 * We used inheritance in this example,
 * To make proxy design pattern more understandable.
 * But it's best practice to favor composition over inheritance.
 */

namespace StockMarket.Domain
{
    public class StockMarketProcessorWithState : StockMarketProcessorWithQueue, IStockMarketProcessorWithState
    {
        private IStockMarketProcessorWithState state;

        public StockMarketProcessorWithState(List<Order>? allOrders = null, long lastOrderNumber = 0, long lastTradeNumber = 0)
            : base(allOrders, lastOrderNumber, lastTradeNumber)
        {
            state = new CloseState(this);
        }

        public void OpenMarket()
        {
            state.OpenMarket();
        }

        public void CloseMarket()
        {
            state.CloseMarket();
        }

        public override async Task<long> EnqueueOrderAsync(TradeSide side, decimal price, decimal quantity, Guid? refId = null)
        {
            return await state.EnqueueOrderAsync(side, price, quantity, refId);
        }

        public override async Task<long> CancelOrderAsync(long orderId, Guid? refId = null)
        {
            return await state.CancelOrderAsync(orderId, refId); 

        }

        public override async Task<long> ModifyOrderAsync(long orderId, decimal price, decimal quantity, Guid? refId = null)
        {
            return await state.ModifyOrderAsync(orderId, price, quantity, refId);
        }

        internal void openMarket()
        {
            state = new OpenState(this);
        }

        internal void closeMarket()
        {
            state = new CloseState(this);
        }

        internal async Task<long> enqueueOrderAsync(TradeSide side, decimal price, decimal quantity, Guid? refId = null)
        {
            return await base.EnqueueOrderAsync(side, price, quantity, refId);
        }

        internal async Task<long> cancelOrderAsync(long orderId, Guid? refId = null)
        {
            return await base.CancelOrderAsync(orderId , refId);
        }

        internal async Task<long> modifyOrderAsync(long orderId, decimal price, decimal quantity, Guid? refId = null)
        {
            return await base.ModifyOrderAsync(orderId, price, quantity, refId);
        }
    }
}
