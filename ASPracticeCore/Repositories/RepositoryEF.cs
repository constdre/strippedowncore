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
    public class RepositoryEF : 
    IRepository
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
            catch (Exception)
            {
                statusMessage = Util.AttachStatusToMessage(Constants.FAILED, Constants.INTERNAL_ERROR);
            }
            return statusMessage;
        }
        public string Update<T>(T entity) where T : class, IEntity
        {
            string statusMessage = Constants.SUCCESS;

            //cast IDbContext to DbContext for the Update method
            // var context = dbContext as DbContext;
            try
            {
                //delete then update for now
                Delete<T>(entity.Id);
                entity.Id = default;
                Create<T>(entity);

                //updates all props
                // context.Update(entity);
                
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                statusMessage = Util.AttachStatusToMessage(Constants.FAILED, Constants.INTERNAL_ERROR);
            }
            return statusMessage;


        }

        public string Delete<T>(int id) where T : class, IEntity
        {
            string statusMessage = Constants.SUCCESS;
            try
            {
                dbContext.GetEntitySet<T>().Remove(GetById<T>(id));
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                statusMessage = Util.AttachStatusToMessage(Constants.FAILED,  Constants.INTERNAL_ERROR);
            }
            return statusMessage;
        }

        public T GetById<T>(int id) where T : class, IEntity
        {
            T entity = dbContext.GetEntitySet<T>().Find(id);
            return entity;
        }


        public IQueryable<T> Get<T>() where T : class, IEntity
        {

            throw new NotImplementedException();
        }
    }
}
