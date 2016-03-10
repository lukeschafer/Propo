using Propo.Framework.Aggregates;

namespace Propo.Framework.Commands
{
    public interface ICommandPersister
    {
        void Save(ICommand command);
        void Save<TAggregate>(IAggregateCommand<TAggregate> command) where TAggregate : IAggregateRoot;
    }
}
