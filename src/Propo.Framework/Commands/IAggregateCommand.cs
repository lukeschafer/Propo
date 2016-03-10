using System;
using Propo.Framework.Aggregates;

namespace Propo.Framework.Commands
{
    public interface IAggregateCommand<TAggregate> where TAggregate : IAggregateRoot
    {
        Guid Id { get; set; }
    }
}
