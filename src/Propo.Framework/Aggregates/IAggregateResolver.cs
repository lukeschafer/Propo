using System;

namespace Propo.Framework.Aggregates
{
    public interface IAggregateResolver<out T> where T : IAggregateRoot
    {
        T Resolve(Guid aggregateId);
    }
}
