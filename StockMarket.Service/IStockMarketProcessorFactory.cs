using StockMarket.Domain;
using StockMarket.Domain.Repositories;

namespace StockMarket.Service
{
    public interface IStockMarketProcessorFactory
    {
        IStockMarketProcessorWithState GetStockMarketProcessorAsync(IOrderReadRepository orderReadRepository, ITradeReadRepository tradeReadRepository);

    }
}