using System;

namespace Propo.Framework.Aggregates
{
    public interface IAggregateRoot
    {
        Guid Id { get; set; }
    }
}
