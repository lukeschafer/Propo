using System.Threading.Tasks;

namespace Propo.Framework.ReadModels
{
    public interface IReadModelRepository
    {
        Task<T> Get<T>(string id) where T : class;
        Task Set<T>(string id, T model) where T : class;
    }
}
