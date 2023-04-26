namespace StockMarket.Domain
{
    public class Order
    {
        public long Id { get; }
        public TradeSide Side { get; }
        public decimal Price { get; }
        public decimal Quantity { get; private set; }
        public bool IsCanceled { get; private set; }
        private byte[]? version;

        internal Order(long id, TradeSide side, decimal price, decimal quantity)
        {
            Id = id;
            Side = side;
            Price = price;
            Quantity = quantity;
        }

        internal void DecreaseQuantity(decimal amount)
        {
            Quantity -= amount;
        }

        internal void Cancel()
        {
            IsCanceled = true;
        }
    }
}