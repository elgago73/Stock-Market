using Microsoft.AspNetCore.Mvc;

namespace StockMarket.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController: ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

    }
}