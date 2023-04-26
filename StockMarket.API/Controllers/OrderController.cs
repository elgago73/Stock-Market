using Microsoft.AspNetCore.Mvc;
using StockMarket.Service.Contract;

namespace StockMarket.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController: ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IStockMarketService stockMarketService;

        public OrderController(ILogger<OrderController> logger, IStockMarketService stockMarketService)
        {
            _logger = logger;
            this.stockMarketService = stockMarketService;
        }

        [HttpGet (Name = "GetAllOrders")]
        public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
        {
            return await stockMarketService.GetAllOrdersAsync();
        }
    }
}