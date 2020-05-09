using ASPracticeCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPracticeCore.DAL
{
    public interface IDbContext:IDisposable
    {
        DbSet<T> GetEntitySet<T>() where T : class, IEntity;
        int SaveChanges();
    }
}
