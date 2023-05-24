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


        [HttpGet("{id}",Name = "GetOrderById")]
        public async Task<OrderResponse?> GetOrderAsync(long id)
        {
            return await stockMarketService.GetOrderAsync(id) ;


        }


        [HttpPost(Name = "AddOrder")]
        public async Task<long> AddOrderAsync([FromBody] AddOrderRequest order)
        {
            return await stockMarketService.AddOrderAsync(order);
        }


        [HttpDelete("{id}", Name = "CancleOrder")]
        public async Task<long> CancleOrderAsync(long id) {

            return await stockMarketService.CancleOrderAsync(id);
        }


        [HttpPatch("{id}", Name = "ModifyOrder")]
        public async Task<long> ModifyOrderAsync(long id, [FromBody] ModifyOrderRequest order)
        {
            return await stockMarketService.ModifyOrderAsync(id, order);
        }
    }
}