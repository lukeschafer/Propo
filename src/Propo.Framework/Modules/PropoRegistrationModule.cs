using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Propo.Framework.Events;
using Module = Autofac.Module;

namespace Propo.Framework.Modules
{
    public class PropoRegistrationModule : Module
    {
        private readonly Assembly[] _messagingAssemblies;

        public PropoRegistrationModule(params Assembly[] messagingAssemblies)
        {
            _messagingAssemblies = messagingAssemblies;
        }
        protected override void Load(ContainerBuilder builder)
        {
            foreach (var evtType in _messagingAssemblies.SelectMany(GetAggregateEvents))
            {
                var d1 = typeof(AggregateEventHandler<,>);
                var aggregateType = evtType.GetInterfaces()
                       .First(t => t.IsGenericType && typeof (IAggregateEvent<>).IsAssignableFrom(t.GetGenericTypeDefinition()))
                       .GenericTypeArguments.First();
                Type[] typeArgs = { aggregateType, evtType };
                var makeme = d1.MakeGenericType(typeArgs);

                builder.RegisterType(makeme).AsImplementedInterfaces();
            }
        }

        public static IEnumerable<Type> GetAggregateEvents(Assembly assembly)
        {
            return from x in assembly.GetTypes()
                   from z in x.GetInterfaces()
                   where
                   (z.IsGenericType && typeof(IAggregateEvent<>).IsAssignableFrom(z.GetGenericTypeDefinition()))
                   && !x.IsAbstract && !x.IsInterface
                   select x;
        }
    }
}
