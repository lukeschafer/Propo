using System;
using System.Collections.Generic;
using Propo.Framework.Aggregates;

namespace Propo.Framework.Events
{
    public interface IEventPersister
    {
        void Save(IEvent evt);
        IEnumerable<IEvent> StreamFrom(long eventid);
        IEnumerable<IAggregateEvent<TAggregate>> StreamFromAggregate<TAggregate>(Guid aggregateId) where TAggregate : IAggregateRoot;
    }
}
