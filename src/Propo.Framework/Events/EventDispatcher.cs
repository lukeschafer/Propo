using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;

namespace Propo.Framework.Events
{
    public interface IEventDispatcher
    {
        void Publish(IEnumerable<IEvent> events, bool isReplay);
        void Publish(IEvent evt, bool isReplay);
        Task PublishAsync(IEnumerable<IEvent> events, bool isReplay);
        Task PublishAsync(IEvent evt, bool isReplay);
    }

    public class EventDispatcher : IEventDispatcher
    {
        private readonly ILifetimeScope _scope;
        private readonly IEventPersister _persister;

        public EventDispatcher(ILifetimeScope scope, IEventPersister persister)
        {
            _scope = scope;
            _persister = persister;
        }

        public void Publish(IEnumerable<IEvent> events, bool isReplay)
        {
            if (events == null) return;
            foreach(var evt in events) Publish(evt, isReplay);
        }

        public void Publish(IEvent evt, bool isReplay)
        {
            if (evt == null) return;
            if (!isReplay) _persister.Save(evt);

            dynamic handlers = _scope.Resolve(typeof(IEnumerable<>).MakeGenericType(typeof(IEventHandler<>).MakeGenericType(evt.GetType())));
            foreach (var handler in handlers)
            {
                var newEvents = (IEnumerable<IEvent>)handler.Handle((dynamic)evt, isReplay);
                Publish(newEvents, isReplay);
            }
        }

        public async Task PublishAsync(IEnumerable<IEvent> events, bool isReplay)
        {
            var tf = new TaskFactory();
            await Task.WhenAll(events.Select(evt => tf.StartNew(e => Publish(e as IEvent, isReplay), evt)).ToArray());
        }

        public async Task PublishAsync(IEvent evt, bool isReplay)
        {
            var tf = new TaskFactory();
            await tf.StartNew(e => Publish(e as IEvent, isReplay), evt);
        }
    }
}
