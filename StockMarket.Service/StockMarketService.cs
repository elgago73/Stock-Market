using StockMarket.Domain;
using StockMarket.Domain.Reposities;
using StockMarket.Domain.Repositories;
using StockMarket.Service.Contract;

namespace StockMarket.Service
{

    public class StockMarketService : IStockMarketService
    {
        private readonly IOrderReadRepository orderReadRepository;
        private readonly IOrderWriteRepository orderWriteRepository;
        private readonly IStockMarketProcessorWithState stockMarketProcessor;
        //singelton and factory design patterns
        public StockMarketService(IOrderReadRepository orderReadRepository,
                                  IOrderWriteRepository orderWriteRepository,
                                  IStockMarketProcessorFactory stockMarketProcessorFactory,
                                  ITradeReadRepository tradeReadRepository)
        {
            this.orderReadRepository = orderReadRepository;
            this.orderWriteRepository = orderWriteRepository;
            stockMarketProcessor = stockMarketProcessorFactory.GetStockMarketProcessorAsync(orderReadRepository, tradeReadRepository);
        }

        public Task<long> AddOrderAsync(AddOrderRequest order)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
        {
            var orders = await orderReadRepository.GetAllOrdersAsync();
            return orders.Select(o => o.ToData());
        }

        public async Task<OrderResponse?> GetOrderAsync(long id)
        {
            var order = await orderReadRepository.GetOrderAsync(id);
            return order?.ToData();

        }
    }
}