using Autofac;
using Propo.Framework.Aggregates;
using Propo.Framework.Events;

namespace Propo.Framework.Commands
{
    public interface ICommandSender
    {
        void Send<TCommand>(TCommand command) where TCommand : ICommand;

        void Send<TAggregate, TCommand>(TCommand command)
            where TAggregate : IAggregateRoot
            where TCommand : class, IAggregateCommand<TAggregate>;
    }

    public class CommandSender : ICommandSender
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly ICommandPersister _persister;
        private readonly IEventDispatcher _eventDispatcher;

        public CommandSender(ILifetimeScope lifetimeScope, ICommandPersister persister, IEventDispatcher eventDispatcher)
        {
            _lifetimeScope = lifetimeScope;
            _persister = persister;
            _eventDispatcher = eventDispatcher;
        }

        public void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null) return;
            _persister.Save(command);
            var handler = _lifetimeScope.Resolve<ICommandHandler<TCommand>>();
            var newEvents = handler.Handle(command);
            _eventDispatcher.Publish(newEvents, false);
        }

        public void Send<TAggregate, TCommand>(TCommand command) 
            where TAggregate : IAggregateRoot
            where TCommand : class, IAggregateCommand<TAggregate>
        {
            if (command == null) return;
            _persister.Save(command);
            var resolver = _lifetimeScope.Resolve<IAggregateResolver<TAggregate>>();
            var aggregateRoot = resolver.Resolve(command.Id);
            command.Id = aggregateRoot.Id;
            var accepts = aggregateRoot as IAcceptCommand<TAggregate, TCommand>;
            if (accepts == null) return;
            var newEvents = accepts.Accept(_lifetimeScope, command);
            _eventDispatcher.Publish(newEvents, false);
        }
    }
}
