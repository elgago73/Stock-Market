using StockMarket.Domain;
using StockMarket.Service.Contract;

namespace StockMarket.Service
{
    internal static class StockMarketMappingExtensions {
        internal static OrderResponse ToData(this Order order) 
        {
            return new OrderResponse
            {
                Id = order.Id,
                Side = order.Side.ToString(),
                Price = order.Price,
                Quantity = order.Quantity,
                IsCanceled = order.IsCanceled
            };

        }
    
    }
}