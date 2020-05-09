using ASPracticeCore.Models;
using ASPracticeCore.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASPracticeCore.Utils;

namespace ASPracticeCore.Repositories
{
    public class RepositoryEF : IRepository
    {
        readonly IDbContext dbContext;
        
        public RepositoryEF(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public string Create<T>(T entity) where T : class, IEntity
        {
            DbSet<T> dbSet = dbContext.GetEntitySet<T>();
            string statusMessage = Constants.SUCCESS;
            dbSet.Add(entity);
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                statusMessage = Util.AttachStatusToMessage(Constants.FAILED, "Server error");
            }
            return statusMessage;
        }

        public string Delete<T>(int id) where T : class, IEntity
        {
            dbContext.GetEntitySet<T>().Remove(GetById<T>(id));
            string statusMessage = Constants.SUCCESS;
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                statusMessage = Util.AttachStatusToMessage(Constants.FAILED,ex.ToString());
            }
            return statusMessage;
        }

        public T GetById<T>(int id) where T : class, IEntity
        {
            T entity = dbContext.GetEntitySet<T>().Find(id);
            return entity;
        }

        public string Update<T>(T entity) where T : class, IEntity
        {
            //Delete then Create New approach:
            string statusMessage = Delete<T>(entity.Id);
            if(statusMessage == Constants.SUCCESS)
            {
                statusMessage = Create(entity); 
            }
            return statusMessage;
            
        }

        public IQueryable<T> Get<T>() where T:class,IEntity
        {
             
            throw new NotImplementedException();
        }
    }
}
