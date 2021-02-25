using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;

namespace RSK.IdentityServer4.AuditEventSink
{
    public class EventSinkAggregator : IEventSink
    {
        private readonly ILogger logger;
        public List<IEventSink> EventSinks { get; set; } = new List<IEventSink>();

        public EventSinkAggregator(ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task PersistAsync(Event evt)
        {
            var eventSinkTasks = new List<Task>();

            foreach (var eventSink in EventSinks)
            {
                eventSinkTasks.Add(ProtectedExecution(() => eventSink.PersistAsync(evt)));    
            }

            return Task.WhenAll(eventSinkTasks);
        }

        private async Task ProtectedExecution(Func<Task> persistAsync)
        {
            try
            {
                await persistAsync();
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e.Message);
            }
        }
    }
}
