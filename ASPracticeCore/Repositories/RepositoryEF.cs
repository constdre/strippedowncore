using ASPracticeCore.Models;
using ASPracticeCore.DAL;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASPracticeCore.Utils;

namespace ASPracticeCore.Repositories
{
    public class RepositoryEF :
    IRepository
    {
        readonly IDbContext context;

        public RepositoryEF(IDbContext dbContext)
        {
            this.context = dbContext;
        }
        public string Create<T>(T entity) where T : class, IEntity
        {
            DbSet<T> dbSet = context.GetEntitySet<T>();
            string statusMessage = Constants.SUCCESS;
            dbSet.Add(entity);
            try
            {
                context.SaveChanges();
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

            var dbContext = context as DbContext;
            try
            {
                //updates all props
                dbContext.Update(entity);
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                statusMessage = Util.AttachStatusToMessage(Constants.FAILED, Constants.INTERNAL_ERROR);
            }
            return statusMessage;


        }

        public async Task<string> Delete<T>(int id) where T : class, IEntity
        {
            string statusMessage = Constants.SUCCESS;

            try
            {
                T entity = await GetById<T>(id);
                context.GetEntitySet<T>().Remove(entity);
                
                var dbContext = context as DbContext;
                await dbContext.SaveChangesAsync(); 
            }
            catch (Exception)
            {
                statusMessage = Util.AttachStatusToMessage(Constants.FAILED, Constants.INTERNAL_ERROR);
            }
            return statusMessage;
        }

        public async Task<T> GetById<T>(int id) where T : class, IEntity
        {
            T entity = await context.GetEntitySet<T>().FindAsync(id);
            return entity;
        }


        public IQueryable<T> Get<T>() where T : class, IEntity
        {

            throw new NotImplementedException();
        }
    }
}
