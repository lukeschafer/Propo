using System.Collections.Generic;
using Autofac;
using Propo.Framework.Commands;
using Propo.Framework.Events;

namespace Propo.Framework.Aggregates
{
    public interface IAcceptCommand<TAggregateRoot, in TCommand> 
        where TCommand : IAggregateCommand<TAggregateRoot> 
        where TAggregateRoot : IAggregateRoot
    {
        IEnumerable<IEvent> Accept(ILifetimeScope lifetimeScope, TCommand command);
    }
}
