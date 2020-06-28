using ASPracticeCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPracticeCore.DAL
{
    public interface IDbContext : IDisposable
    {
        //Will turn this into abstract class so it inherits directly from DbContext

        //we provide this common generic method to the implementors
        DbSet<T> GetEntitySet<T>() where T : class, IEntity;

        //======already implemented by DbContext, made accessible to concrete classes:
        int SaveChanges();

    }
}
