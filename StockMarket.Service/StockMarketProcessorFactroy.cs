
using StockMarket.Domain;
using StockMarket.Domain.Repositories;

namespace StockMarket.Service
{
    public class StockMarketProcessorFactroy : IStockMarketProcessorFactory
    {
        private StockMarketProcessorWithState? processor;
        private readonly object lockObject = new object();
        public IStockMarketProcessorWithState GetStockMarketProcessorAsync(IOrderReadRepository orderReadRepository, ITradeReadRepository tradeReadRepository)
        {
            //double lock check
            if (processor != null) return processor;
            lock (lockObject)
            {
                if (processor != null) return processor;
                var orders = orderReadRepository.GetAllAsync(o => !o.IsCanceled && o.Quantity > 0).GetAwaiter().GetResult();
                var lastOrderId = orderReadRepository.GetLastIdAsync().GetAwaiter().GetResult();
                var lastTradeId = tradeReadRepository.GetLastIdAsync().GetAwaiter().GetResult();
                processor = new StockMarketProcessorWithState(orders, lastOrderId, lastTradeId);
                return processor;
            }
        }

    }
}
