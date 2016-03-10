using System;
using Propo.Framework.Aggregates;

namespace Propo.Framework.Events
{
    public interface IAggregateEvent<TAggregate> : IEvent where TAggregate : IAggregateRoot
    {
        Guid Id { get; set; }
    }
}
