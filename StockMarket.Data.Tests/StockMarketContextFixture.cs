using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace StockMarket.Data.Tests
{
    public class StockMarketContextFixture : IAsyncDisposable
    {
        public StockMarketDbContext Context { get; }
        public ITestOutputHelper? Output { get; set; }

        public StockMarketContextFixture()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StockMarketDbContext>();
            optionsBuilder.UseSqlServer("server=.\\sqlexpress;database=StockMarketTest;MultipleActiveResultSets=true;trusted_connection=true;encrypt=yes;trustservercertificate=yes;");
            optionsBuilder.LogTo(msg => Output?.WriteLine(msg));
            Context = new StockMarketDbContext(optionsBuilder.Options);
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        }

        public async ValueTask DisposeAsync()
        {
            await Context.DisposeAsync();
        }
    }
}