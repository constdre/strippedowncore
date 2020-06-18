using System.Linq;
using System.Threading.Tasks;

namespace ASPracticeCore.Models
{
    public interface IRepository
    {
        
        string Create<T>(T entity) where T:class, IEntity; //"class" specif for non-EF implementors
        // Task<string> Update<T>(T entity) where T : class, IEntity;
        string Update<T>(T entity) where T : class, IEntity;
        string Delete<T>(int id) where T : class, IEntity;
        IQueryable<T> Get<T>() where T : class, IEntity;
        T GetById<T>(int id) where T : class, IEntity;
      

    }
}
