﻿namespace StockMarket.Domain.Comparers
{
    internal class MaxComparer : BaseComparer
    {
        protected override int SpecificCompare(Order? x, Order? y)
        {
            if (x?.Price < y?.Price) return 1;
            if (x?.Price > y?.Price) return -1;
            return 0;
        }
    }
}