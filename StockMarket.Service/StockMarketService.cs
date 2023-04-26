using StockMarket.Domain.Reposities;
using StockMarket.Service.Contract;

namespace StockMarket.Service
{

    public class StockMarketService : IStockMarketService
    {
        private readonly IOrderReadRepository orderReadRepository;
        private readonly IOrderWriteRepository orderWriteRepository;

        public StockMarketService(IOrderReadRepository orderReadRepository, IOrderWriteRepository orderWriteRepository)
        {
            this.orderReadRepository = orderReadRepository;
            this.orderWriteRepository = orderWriteRepository;
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