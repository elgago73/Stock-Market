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
        private readonly ITradeWriteRepository tradeWriteRepository;
        private readonly IStockMarketProcessorWithState stockMarketProcessor;

        //singelton and factory design patterns
        public StockMarketService(IOrderReadRepository orderReadRepository,
                                  IOrderWriteRepository orderWriteRepository,
                                  IStockMarketProcessorFactory stockMarketProcessorFactory,
                                  ITradeReadRepository tradeReadRepository,
                                  ITradeWriteRepository tradeWriteRepository)
        {
            this.orderReadRepository = orderReadRepository;
            this.orderWriteRepository = orderWriteRepository;
            stockMarketProcessor = stockMarketProcessorFactory.GetStockMarketProcessorAsync(orderReadRepository, tradeReadRepository);
            this.tradeWriteRepository = tradeWriteRepository;
        }

        public async Task<long> AddOrderAsync(AddOrderRequest order)
        {
            Enum.TryParse(order.Side, out TradeSide side);

            var orderId = await stockMarketProcessor.EnqueueOrderAsync(side: side,
                                                                price: order.Price,
                                                                quantity: order.Quantity);
            var result = stockMarketProcessor.ResultContext; 
            if(result.CreatedOrder != null) await orderWriteRepository.AddAsync(result.CreatedOrder);
            await orderWriteRepository.UpdateAsync(result.UpdatedOrders);
            await tradeWriteRepository.AddAsync(result.CreatedTrades);
            return orderId;
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