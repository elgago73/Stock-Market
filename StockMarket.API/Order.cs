﻿namespace StockMarket.API
{
    public class Order
    {

        public long Id { get; set; }
        public string Side { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public bool IsCanceled { get; set; }
    }
}
