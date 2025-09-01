using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Persistence
{
    public interface IDbContext
    { 
        DbSet<T> Set<T>() where T : class;
        DbEntityEntry Entry(object obj);
        DbContextConfiguration Configuration { get; }
        //int SaveChanges();
        void Dispose();
    }
}
