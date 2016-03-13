using System;
using System.Collections.Concurrent;
using Propo.Framework.Aggregates;

namespace Propo.Framework.InMemory.Resolvers
{
    public abstract class BaseInMemoryResolver<TAggregate> : IAggregateResolver<TAggregate> where TAggregate : IAggregateRoot
    {
        protected static readonly ConcurrentDictionary<Guid, TAggregate> AggregateRoots = new ConcurrentDictionary<Guid, TAggregate>();
        
        public TAggregate Resolve(Guid aggregateId)
        {
            if (aggregateId != Guid.Empty && AggregateRoots.ContainsKey(aggregateId))
                return AggregateRoots[aggregateId];

            var aggregate = Activator.CreateInstance<TAggregate>();
            aggregate.Id = Guid.NewGuid();

            AggregateRoots.AddOrUpdate(aggregate.Id, aggregate, (k, a) => aggregate);
            return aggregate;
        }
    }
}
