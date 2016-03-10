using System.Collections.Generic;
using Autofac;
using Propo.Framework.Aggregates;

namespace Propo.Framework.Events
{
    public class AggregateEventHandler<TAggregate, TEvent> : IEventHandler<TEvent> 
        where TAggregate : IAggregateRoot 
        where TEvent : IAggregateEvent<TAggregate>
    {
        private readonly IAggregateResolver<TAggregate> _resolver;
        private readonly ILifetimeScope _lifetimeScope;

        public AggregateEventHandler(IAggregateResolver<TAggregate> resolver, ILifetimeScope lifetimeScope)
        {
            _resolver = resolver;
            _lifetimeScope = lifetimeScope;
        }

        public IEnumerable<IEvent> Handle(TEvent evt, bool isReplay)
        {
            var aggregate = ((dynamic)_resolver.Resolve(evt.Id));
            if (!(aggregate is IHandleAggregateEvent<TAggregate, TEvent>)) return null;
            var newEvents = (IEnumerable<IEvent>)aggregate.Handle(_lifetimeScope, evt, isReplay);
            return newEvents;
        }
    }
}
