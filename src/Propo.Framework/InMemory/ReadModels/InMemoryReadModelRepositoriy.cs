using System.Collections.Concurrent;
using System.Threading.Tasks;
using Propo.Framework.ReadModels;

namespace Propo.Framework.InMemory.ReadModels
{
    public class InMemoryReadModelRepositoriy : IReadModelRepository
    {
        private readonly ConcurrentDictionary<string, object> _models = new ConcurrentDictionary<string, object>(); 
        public Task<T> Get<T>(string id) where T : class
        {
            object obj;
            if (!_models.TryGetValue(id, out obj)) return null;
            return Task.FromResult((T)obj);
        }

        public Task Set<T>(string id, T model) where T : class
        {
            _models.AddOrUpdate(id, model, (k, m) => model);
            return Task.FromResult(0);
        }
    }
}