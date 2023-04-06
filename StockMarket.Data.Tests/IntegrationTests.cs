using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using StockMarket.Domain;
using System.Data;
using Xunit.Abstractions;

namespace StockMarket.Data.Tests
{
    public class IntegrationTests : IClassFixture<StockMarketContextFixture>, IAsyncDisposable
    {
        private readonly StockMarketContextFixture fixture;
        private readonly StockMarketDbContext context;
        private readonly ITestOutputHelper output;

        public IntegrationTests(StockMarketContextFixture fixture, ITestOutputHelper output)
        {
            this.fixture = fixture;
            context = fixture.Context;
            this.output = output;
            this.fixture.Output = output;
            context.Database.OpenConnection();
        }

        public async ValueTask DisposeAsync()
        {
            await context.Database.CloseConnectionAsync();
        }

        [Fact]
        public async Task DbContext_should_save_orders_in_database_test_async()
        {
            // Arrange
            var stockMarket = new TestStockMarketProccessor(
                orders: await context.Orders.Where(o => !o.IsCanceled && o.Quantity > 0).ToListAsync(),
                lastOrderId: await context.Orders.MaxAsync(o => (long?)o.Id) ?? 0,
                lastTradeId: await context.Trades.MaxAsync(t => (long?)t.Id) ?? 0);

            var buyOrderId = stockMarket.EnqueueOrder(TradeSide.Buy, 1500M, 1M);
            var sellOrderId = stockMarket.EnqueueOrder(TradeSide.Sell, 1500M, 1M);
            var buyOrder = stockMarket.Orders.First(o => o.Id == buyOrderId);
            var sellOrder = stockMarket.Orders.First(o => o.Id == sellOrderId);
            // Act
            await context.Orders.AddAsync(buyOrder);
            await context.Orders.AddAsync(sellOrder);
            await context.SaveChangesAsync();
            // Assert
            context.Orders.First(o => o.Id == buyOrderId).Should().BeEquivalentTo(new
            {
                Id = buyOrderId,
                Side = TradeSide.Buy,
                Price = 1500M,
                Quantity = 0M,
                IsCanceled = false,
            });
            context.Orders.First(o => o.Id == sellOrderId).Should().BeEquivalentTo(new
            {
                Id = sellOrderId,
                Side = TradeSide.Sell,
                Price = 1500M,
                Quantity = 0M,
                IsCanceled = false,
            });
        }
        [Fact]
        public async Task DbContext_should_save_orders_and_trades_in_database_test_async()
        {
            // Arrange
            var stockMarket = new TestStockMarketProccessor(
                orders: await context.Orders.Where(o => !o.IsCanceled && o.Quantity > 0).ToListAsync(),
                lastOrderId: await context.Orders.MaxAsync(o => (long?)o.Id) ?? 0,
                lastTradeId: await context.Trades.MaxAsync(t => (long?)t.Id) ?? 0);

            var buyOrderId = stockMarket.EnqueueOrder(TradeSide.Buy, 1500M, 1M);
            var sellOrderId = stockMarket.EnqueueOrder(TradeSide.Sell, 1500M, 1M);
            var buyOrder = stockMarket.Orders.First(o => o.Id == buyOrderId);
            var sellOrder = stockMarket.Orders.First(o => o.Id == sellOrderId);
            var trade = stockMarket.Trades.First();
            // Act
            await context.Orders.AddAsync(buyOrder);
            await context.Orders.AddAsync(sellOrder);
            await context.Trades.AddAsync(trade);
            await context.SaveChangesAsync();
            // Assert
            context.Orders.First(o => o.Id == buyOrderId).Should().BeEquivalentTo(new
            {
                Id = buyOrderId,
                Side = TradeSide.Buy,
                Price = 1500M,
                Quantity = 0M,
                IsCanceled = false,
            });
            context.Orders.First(o => o.Id == sellOrderId).Should().BeEquivalentTo(new
            {
                Id = sellOrderId,
                Side = TradeSide.Sell,
                Price = 1500M,
                Quantity = 0M,
                IsCanceled = false,
            });
            context.Trades.First(t => t.Id == trade.Id).Should().BeEquivalentTo(new
            {
                Id = trade.Id,
                SellOrderId = sellOrderId,
                BuyOrderId = buyOrderId,
                Price = 1500M,
                Quantity = 1,
            });
        }
        [Fact]
        public async Task LockRecord_with_transaction_test_async()
        {
            // Arrange
            var stockMarket = new TestStockMarketProccessor(
               orders: await context.Orders.Where(o => !o.IsCanceled && o.Quantity > 0).ToListAsync(),
               lastOrderId: await context.Orders.MaxAsync(o => (long?)o.Id) ?? 0,
               lastTradeId: await context.Trades.MaxAsync(t => (long?)t.Id) ?? 0);

            var sellOrderId = stockMarket.EnqueueOrder(TradeSide.Sell, 1500M, 1M);
            var sellOrder = stockMarket.Orders.First(o => o.Id == sellOrderId);

            await context.Orders.AddAsync(sellOrder);
            await context.SaveChangesAsync();
            // Act
            var optionsBuilder = new DbContextOptionsBuilder<StockMarketDbContext>();
            optionsBuilder.UseSqlServer("server=.\\sqlexpress;database=StockMarketTests;MultipleActiveResultSets=true;trusted_connection=true;encrypt=yes;trustservercertificate=yes;");
            optionsBuilder.LogTo(output.WriteLine);

            var i = 0;
            var t1 = Task.Run(async () =>
            {
                await using (var context1 = new StockMarketDbContext(optionsBuilder.Options))
                {
                    var stockMarket1 = new TestStockMarketProccessor(
                        orders: await context1.Orders.Where(o => !o.IsCanceled && o.Quantity > 0).ToListAsync(),
                        lastOrderId: await context1.Orders.MaxAsync(o => (long?)o.Id) ?? 0,
                        lastTradeId: await context1.Trades.MaxAsync(t => (long?)t.Id) ?? 0);

                    await using (var tran = context1.Database.BeginTransaction(IsolationLevel.RepeatableRead))
                    {
                        try
                        {
                            await context1.Orders.FirstOrDefaultAsync(o => o.Id == sellOrderId);
                            stockMarket1.CancelOrder(sellOrderId);
                            await context1.SaveChangesAsync();
                            await tran.CommitAsync();
                            Interlocked.Increment(ref i);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            });

            var t2 = Task.Run(async () =>
            {
                await using (var context1 = new StockMarketDbContext(optionsBuilder.Options))
                {
                    var stockMarket1 = new TestStockMarketProccessor(
                        orders: await context1.Orders.Where(o => !o.IsCanceled && o.Quantity > 0).ToListAsync(),
                        lastOrderId: await context1.Orders.MaxAsync(o => (long?)o.Id) ?? 0,
                        lastTradeId: await context1.Trades.MaxAsync(t => (long?)t.Id) ?? 0);

                    await using (var tran = context1.Database.BeginTransaction(IsolationLevel.RepeatableRead))
                    {
                        try
                        {
                            await context1.Orders.FirstOrDefaultAsync(o => o.Id == sellOrderId);
                            stockMarket1.CancelOrder(sellOrderId);
                            await context1.SaveChangesAsync();
                            await tran.CommitAsync();
                            Interlocked.Increment(ref i);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            });

            await Task.WhenAll(t1, t2);
            // Assert
            Assert.Equal(1, i);
        }
        [Fact]
        public async Task Optimistic_concurrency_with_versioning_test_async()
        {
            // Arrange
            var stockMarket = new TestStockMarketProccessor(
                orders: await context.Orders.Where(o => !o.IsCanceled && o.Quantity > 0).ToListAsync(),
                lastOrderId: await context.Orders.MaxAsync(o => (long?)o.Id) ?? 0,
                lastTradeId: await context.Trades.MaxAsync(t => (long?)t.Id) ?? 0);

            var sellOrderId = stockMarket.EnqueueOrder(TradeSide.Sell, 1500M, 1M);
            var sellOrder = stockMarket.Orders.First(o => o.Id == sellOrderId);

            await context.Orders.AddAsync(sellOrder);
            await context.SaveChangesAsync();
            // Act
            var optionsBuilder = new DbContextOptionsBuilder<StockMarketDbContext>();
            optionsBuilder.UseSqlServer("server=.\\sqlexpress;database=StockMarketTests;MultipleActiveResultSets=true;trusted_connection=true;encrypt=yes;trustservercertificate=yes;");
            optionsBuilder.LogTo(output.WriteLine);

            var i = 0;
            var t1 = Task.Run(async () =>
            {
                await using (var context1 = new StockMarketDbContext(optionsBuilder.Options))
                {
                    var stockMarket1 = new TestStockMarketProccessor(
                        orders: await context1.Orders.Where(o => !o.IsCanceled && o.Quantity > 0).ToListAsync(),
                        lastOrderId: await context1.Orders.MaxAsync(o => (long?)o.Id) ?? 0,
                        lastTradeId: await context1.Trades.MaxAsync(t => (long?)t.Id) ?? 0);

                    stockMarket1.CancelOrder(sellOrderId);
                    try
                    {
                        await context1.SaveChangesAsync();
                        Interlocked.Increment(ref i);
                    }

                    catch (Exception)
                    {
                    }
                }
            });


            var t2 = Task.Run(async () =>
            {
                await using (var context1 = new StockMarketDbContext(optionsBuilder.Options))
                {
                    var stockMarket1 = new TestStockMarketProccessor(
                        orders: await context1.Orders.Where(o => !o.IsCanceled && o.Quantity > 0).ToListAsync(),
                        lastOrderId: await context1.Orders.MaxAsync(o => (long?)o.Id) ?? 0,
                        lastTradeId: await context1.Trades.MaxAsync(t => (long?)t.Id) ?? 0);

                    stockMarket1.CancelOrder(sellOrderId);

                    try
                    {
                        await context1.SaveChangesAsync();
                        Interlocked.Increment(ref i);
                    }
                    catch (Exception)
                    {
                    }
                }
            });

            await Task.WhenAll(t1, t2);
            // Assert
            Assert.Equal(1, i);
        }
    }
}