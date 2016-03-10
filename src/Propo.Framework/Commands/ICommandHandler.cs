using System.Collections.Generic;
using Propo.Framework.Events;

namespace Propo.Framework.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        IEnumerable<IEvent> Handle(TCommand command);
    }
}
