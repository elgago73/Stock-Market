﻿using Microsoft.AspNetCore.Mvc;
using StockMarket.Service.Contract;
using StockMarket.Service;

namespace StockMarket.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TradeController : ControllerBase
    {
        private readonly IStockMarketService stockMarketService;

        public TradeController(IStockMarketService stockMarketService)
        {
            this.stockMarketService = stockMarketService;
        }

        [HttpGet(Name = "GetAllTrades")]
        public async Task<IEnumerable<TradeResponse>> GetAllOrdersAsync()
        {
            return await stockMarketService.GetAllTradesAsync();
        }
    }
}
