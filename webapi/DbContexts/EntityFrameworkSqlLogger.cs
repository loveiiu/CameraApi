using Microsoft.Extensions.Logging;
using System;

namespace WebApi.DbContexts
{
    public class EntityFrameworkSqlLoggerFactory : ILoggerFactory
    {
        private ILoggerProvider provider;
        private ILogger logger;
        public void AddProvider(ILoggerProvider provider)
        {
            this.provider = provider;
        }

        public ILogger CreateLogger(string categoryName)
        {
            if (this.logger == null)
            {
                this.logger = this.provider.CreateLogger(categoryName);
            }
            return this.logger;
        }

        public void Dispose()
        {

        }
    }
    public class EntityFrameworkSqlLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new EntityFrameworkSqlLogger();
        }

        public void Dispose()
        {

        }
    }
    public class EntityFrameworkSqlLogger : ILogger
    {

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (eventId.Name == null || eventId.Name == "Microsoft.EntityFrameworkCore.Database.Command.CommandExecuted" == false)
            {
                return;
            }
        }
    }
}
