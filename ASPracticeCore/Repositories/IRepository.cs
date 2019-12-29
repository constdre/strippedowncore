using System.Linq;
namespace ASPracticeCore.Models
{
    public interface IRepository
    {
        
        string Create<T>(T entity) where T:class, IEntity; //"class" specif for non-EF implementors
        string Update<T>(T entity) where T : class, IEntity;
        string Delete<T>(int id) where T : class, IEntity;
        IQueryable<T> Get<T>(dynamic filters) where T : class, IEntity;

    }
}
