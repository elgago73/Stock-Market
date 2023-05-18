using Microsoft.AspNetCore.Mvc;
using StockMarket.Service.Contract;

namespace StockMarket.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IStockMarketService stockMarketService;

        public OrderController(IStockMarketService stockMarketService)
        {
            this.stockMarketService = stockMarketService;
        }

        [HttpGet(Name = "GetAllOrders")]
        public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
        {
            return await stockMarketService.GetAllOrdersAsync();
        }

        [HttpPost(Name = "AddOrder")]
        public async Task<long> AddOrderAsync([FromBody] AddOrderRequest order)
        {
            return await stockMarketService.AddOrderAsync(order);
        }
    }
}