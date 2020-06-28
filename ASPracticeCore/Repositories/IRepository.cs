using System.Linq;
using System.Threading.Tasks;

namespace ASPracticeCore.Models
{
    public interface IRepository
    {
        //This interfaces uses IEntity interface as type constraint
        string Create<T>(T entity) where T : class, IEntity;
        string Update<T>(T entity) where T : class, IEntity;
        Task<string> Delete<T>(int id) where T : class, IEntity;
        Task<T> GetById<T>(int id) where T : class, IEntity;

    }
}
