using System.Collections.Generic;
using Propo.Framework.Aggregates;
using Propo.Framework.Commands;

namespace Propo.Framework.InMemory.Persisters
{
    public class InMemoryCommandPersister : ICommandPersister
    {
        protected static readonly List<object> Commands = new List<object>();
        private static readonly object Lock = new { };
        
        public void Save(ICommand command)
        {
            lock (Lock)
            {
                Commands.Add(command);
            }
        }

        public void Save<TAggregate>(IAggregateCommand<TAggregate> command) where TAggregate : IAggregateRoot
        {
            lock (Lock)
            {
                Commands.Add(command);
            }
        }
    }
}
