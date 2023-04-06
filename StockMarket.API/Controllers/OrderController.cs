using Microsoft.AspNetCore.Mvc;

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
        public IEnumerable<Order> GetAllOrders()
        {
            return stockMarketService.GetAllOrderes();
        }
    }
}