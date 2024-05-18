using Microsoft.Extensions.Logging;

namespace Predictor.Testing.Mocks
{
    internal class MockLogger<T> : ILogger where T : class
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            // Do nothing.
        }
    }
}
