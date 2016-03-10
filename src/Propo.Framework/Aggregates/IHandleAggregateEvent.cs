using System.Collections.Generic;
using Autofac;
using Propo.Framework.Events;

namespace Propo.Framework.Aggregates
{
    public interface IHandleAggregateEvent<TAggregate, in TEvent> 
        where TEvent : IAggregateEvent<TAggregate> 
        where TAggregate : IAggregateRoot
    {
        IEnumerable<IEvent> Handle(ILifetimeScope lifetimeScope, TEvent evt, bool isReplay);
    }
}
