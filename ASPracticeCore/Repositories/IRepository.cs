using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPracticeCore.Models;

namespace ASPracticeCore.Repositories
{
    public interface IRepository
    {
        string Add<T>(T entity) where T : EntityBase; //could do without type specif, but I want the arguments to just be EntityBase
        string Update<T>(T entity) where T : EntityBase;
        string Delete();
        List<T> GetAll<T>();

        List<T> GetFiltered<T>(dynamic filters);
        T GetById<T>(int id) where T : EntityBase;
        
    }
}

