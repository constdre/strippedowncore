using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPracticeCore.Models;

namespace ASPracticeCore.Repositories
{
    public interface IRepositoryA
    {
        //This version of interface uses abstact class EntityBase as the type parameter for the generic methods
        
        string Add<T>(T entity) where T : EntityBase; //could do w/o type specif, but I want the arguments to be of type EntityBase
        string Update<T>(T entity) where T : EntityBase;
        string Delete();
        List<T> GetAll<T>();
        List<T> GetFiltered<T>(dynamic filters);
        T GetById<T>(int id) where T : EntityBase;
        
    }
}

