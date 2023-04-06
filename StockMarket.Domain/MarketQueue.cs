using System.Collections.Concurrent;
using StockMarket.Domain.Commands;

namespace StockMarket.Domain
{
    internal class MarketQueue : IAsyncDisposable
    {
        private BlockingCollection<ICommand> queue;
        private Task mainTask;

        public MarketQueue()
        {
            queue = new();
            mainTask = Task.Run(() =>
            {
                while (!queue.IsAddingCompleted || queue.Count > 0)
                {
                    try
                    {
                        var command = queue.Take();
                        command.Execute();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            });
        }

        public async Task<T> ExecuteAsync<T>(BaseCommand<T> command)
        {
            queue.Add(command);
            return await command.WaitForCompletionAsync();
        }

        public async ValueTask DisposeAsync()
        {
            queue.CompleteAdding();
            await mainTask;
        }
    }
}
