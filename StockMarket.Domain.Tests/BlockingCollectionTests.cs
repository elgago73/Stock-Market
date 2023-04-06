using System.Collections.Concurrent;
using StockMarket.Domain.Commands;

namespace StockMarket.Domain.Tests
{
    public class BlockingCollectionTests
    {
        private struct QueueItem
        {
            public int Data { get; }
            public TaskCompletionSource<int> Completion { get; }

            public QueueItem(int data)
            {
                Data = data;
                Completion = new TaskCompletionSource<int>();
            }
        }

        [Fact]
        public async Task BlockingCollection_add_and_take_test_async()
        {
            // Arrange
            var queue = new BlockingCollection<int>();
            var sum = 0;
            // Act
            var producer = Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    queue.Add(i);
                }

                queue.CompleteAdding();
            });

            var consumer = Task.Run(() =>
            {
                while (!queue.IsAddingCompleted || queue.Count > 0)
                {
                    if (!queue.TryTake(out int item)) continue;
                    sum += item;
                }
            });

            await Task.WhenAll(producer, consumer);
            // Assert
            Assert.Equal(45, sum);
        }
        [Fact]
        public async Task BlockingCollection_with_TaskCompletionSource_test_async()
        {
            // Arrange
            var queue = new BlockingCollection<QueueItem>();
            var producers = new Task[10];
            var sum = 0;
            var j = -1;
            // Act
            for (int i = 0; i < 10; i++)
            {
                producers[i] = Task.Run(async () =>
                {
                    var item = new QueueItem(Interlocked.Increment(ref j));
                    queue.Add(item);
                    if (j == 9) queue.CompleteAdding();

                    var data = await item.Completion.Task;
                    Interlocked.Add(ref sum, data);
                });
            }

            var consumer = Task.Run(() =>
            {
                while (!queue.IsAddingCompleted || queue.Count > 0)
                {
                    if (!queue.TryTake(out QueueItem item)) continue;
                    item.Completion.SetResult(item.Data + 1);
                }
            });

            await Task.WhenAll(producers);
            await consumer;
            // Assert
            Assert.Equal(55, sum);
        }
        [Fact]
        public async Task BlockingCollection_with_enqueueCommand_test_async()
        {
            // Arrange
            var stockMarket = new TestStockMarketProccessor();
            var commandQueue = new BlockingCollection<ICommand>();
            var enqueueCommandProducers = new Task[10];
            var j = -1;
            // Act
            for (int i = 0; i < 5; i++)
            {
                enqueueCommandProducers[i] = Task.Run(async () =>
                {
                    var command = new EnqueueCommand(stockMarket, TradeSide.Buy, 1500M, 1M);
                    commandQueue.Add(command);
                    Interlocked.Increment(ref j);

                    await command.WaitForCompletionAsync();
                });
            }

            for (int i = 5; i < 10; i++)
            {
                enqueueCommandProducers[i] = Task.Run(async () =>
                {
                    var command = new EnqueueCommand(stockMarket, TradeSide.Sell, 1500M, 1M);
                    commandQueue.Add(command);
                    Interlocked.Increment(ref j);

                    if (j == 9) commandQueue.CompleteAdding();

                    await command.WaitForCompletionAsync();
                });
            }

            var commandConsumer = Task.Run(() =>
            {
                while (!commandQueue.IsAddingCompleted || commandQueue.Count > 0)
                {
                    if (!commandQueue.TryTake(out ICommand? item)) continue;
                    item.Execute();
                }
            });

            await Task.WhenAll(enqueueCommandProducers);
            await commandConsumer;
            // Assert
            Assert.Equal(10, stockMarket.Orders.Count());
            Assert.Equal(5, stockMarket.Trades.Count());
        }
        [Fact]
        public async Task BlockingCollection_with_enqueueCommand_and_cancelCommand_test_async()
        {
            // Arrange
            var stockMarket = new TestStockMarketProccessor();
            var commandQueue = new BlockingCollection<ICommand>();
            var enqueuedOrdersIdQueue = new BlockingCollection<long>();
            var enqueueCommandProducers = new Task[10];
            var cancelCommandProducers = new Task[10];
            var j = -1;
            // Act
            for (int i = 0; i < 10; i++)
            {
                enqueueCommandProducers[i] = Task.Run(async () =>
                {
                    var command = new EnqueueCommand(stockMarket, TradeSide.Buy, 1500M, 1M);
                    commandQueue.Add(command);
                    Interlocked.Increment(ref j);

                    var orderId = await command.WaitForCompletionAsync();
                    enqueuedOrdersIdQueue.Add(orderId);
                });
            }

            for (int i = 0; i < 10; i++)
            {
                cancelCommandProducers[i] = Task.Run(async () =>
                {
                    var orderId = enqueuedOrdersIdQueue.Take();
                    var command = new CancelCommand(stockMarket, orderId);
                    commandQueue.Add(command);
                    Interlocked.Increment(ref j);

                    if (j == 19)
                    {
                        commandQueue.CompleteAdding();
                        enqueuedOrdersIdQueue.CompleteAdding();
                    }

                    await command.WaitForCompletionAsync();
                });
            }

            var commandConsumer = Task.Run(() =>
            {
                while (!commandQueue.IsAddingCompleted || commandQueue.Count > 0)
                {
                    if (!commandQueue.TryTake(out ICommand? item)) continue;
                    item.Execute();
                }
            });

            await Task.WhenAll(enqueueCommandProducers);
            await Task.WhenAll(cancelCommandProducers);
            await commandConsumer;
            // Assert
            Assert.Equal(10, stockMarket.Orders.Count(o => o.IsCanceled));
        }
    }
}