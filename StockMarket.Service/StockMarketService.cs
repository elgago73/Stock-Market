using StockMarket.API.Controllers;
using StockMarket.Domain;
using StockMarket.Domain.Repositories;
using StockMarket.Service.Contract;

namespace StockMarket.Service
{

    public class StockMarketService : IStockMarketService
    {
        private readonly IOrderReadRepository orderReadRepository;
        private readonly ITradeReadRepository tradeReadRepository;
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
            this.tradeWriteRepository = tradeWriteRepository;
            stockMarketProcessor = stockMarketProcessorFactory.GetStockMarketProcessorAsync(orderReadRepository, tradeReadRepository);
            stockMarketProcessor.OpenMarket();
        }

        public async Task<long> AddOrderAsync(AddOrderRequest order)
        {
            Enum.TryParse(order.Side, out TradeSide side);
            var refId = Guid.NewGuid();

            var orderId = await stockMarketProcessor.EnqueueOrderAsync(side: side,
                                                                price: order.Price,
                                                                quantity: order.Quantity,
                                                                refId: refId);

            var result = stockMarketProcessor.GetContextBy(refId);

            if(result?.CreatedOrder != null) await orderWriteRepository.AddAsync(result.CreatedOrder);
            await orderWriteRepository.UpdateAsync(result.UpdatedOrders);
            await tradeWriteRepository.AddAsync(result.CreatedTrades);
            await orderWriteRepository.SaveChangesAsync();
            return orderId;
        }

        public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
        {
            var orders = await orderReadRepository.GetAllOrdersAsync();
            return orders.Select(o => o.ToData());
        }

        public async Task<IEnumerable<TradeResponse>> GetAllTradesAsync()
        {
            var trades = await tradeReadRepository.GetAllTradesAsync();
            return trades.Select(t => t.ToData());
        }

        public async Task<OrderResponse?> GetOrderAsync(long id)
        {
            var order = await orderReadRepository.GetOrderAsync(id);
            return order?.ToData();

        }
    }
}