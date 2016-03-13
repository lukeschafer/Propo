using System;
using System.Collections.Generic;
using Propo.Framework.Aggregates;
using Propo.Framework.Events;

namespace Propo.Framework.InMemory.Persisters
{
    public class InMemoryEventPersister : IEventPersister
    {
        private static readonly List<IEvent> Events = new List<IEvent>();
        private static readonly object Lock = new {};

        public void Save(IEvent evt)
        {
            lock (Lock)
            {
                evt.EventId = Events.Count;
                Events.Add(evt);
            }
        }

        public IEnumerable<IEvent> StreamFrom(long eventid)
        {
            var currentIndex = eventid;
            while (true)
            {
                lock (Lock)
                {
                    if (Events.Count <= currentIndex) yield break;
                }
                yield return Events[Convert.ToInt32(currentIndex)];
                currentIndex++;
            }
        }
        public IEnumerable<IAggregateEvent<TAggregate>> StreamFromAggregate<TAggregate>(Guid aggregateId) where TAggregate : IAggregateRoot
        {
            var currentIndex = 0;
            while (true)
            {
                lock (Lock)
                {
                    if (Events.Count <= currentIndex) yield break;
                }
                var evt = Events[currentIndex] as IAggregateEvent<TAggregate>;
                if (evt != null && evt.Id == aggregateId)
                    yield return evt;

                currentIndex++;
            }
        }
    }
}