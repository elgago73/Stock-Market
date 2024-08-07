﻿namespace StockMarket.Domain
{
    public class Order
    {
        public long Id { get; }
        public TradeSide Side { get; private set; }
        public decimal Price { get; private set; }
        public decimal Quantity { get; private set; }
        public bool IsCanceled { get; private set; }
        private byte[]? version;

        internal Order(long id, TradeSide side, decimal price, decimal quantity, bool isCanceled = false)
        {
            Id = id;
            Side = side;
            Price = price;
            Quantity = quantity;
            IsCanceled = isCanceled;
        }

        internal void DecreaseQuantity(decimal amount)
        {
            Quantity -= amount;
        }

        internal void Cancel()
        {
            IsCanceled = true;
        }

        public void UpdateBy(Order order)
        {
            Side = order.Side;
            Price = order.Price;
            Quantity = order.Quantity;
            IsCanceled = order.IsCanceled;

        }
        internal Order Clone()
        {
            return new Order(Id, Side, Price, Quantity, IsCanceled);
        }
    }
}