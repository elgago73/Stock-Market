namespace StockMarket.Domain.Commands
{
    public abstract class BaseCommand<T> : ICommand
    {
        private readonly TaskCompletionSource<T> completion;

        public BaseCommand()
        {
            completion = new TaskCompletionSource<T>();
        }

        public Task<T> WaitForCompletionAsync()
        {
            return completion.Task;
        }

        public void Execute()
        {
            completion.SetResult(SpecificExecute());
        }

        protected abstract T SpecificExecute();
    }
}