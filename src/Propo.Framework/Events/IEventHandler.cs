using System.Collections.Generic;

namespace Propo.Framework.Events
{
    public interface IEventHandler<in TEventType> where TEventType : IEvent
    {
        IEnumerable<IEvent> Handle(TEventType evt, bool isReplay);
    }
}
