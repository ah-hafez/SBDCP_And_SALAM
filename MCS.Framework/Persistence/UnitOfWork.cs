using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Persistence
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IDbContext dbContext = null;

        public UnitOfWork(IDbContext context)
        {
            this.dbContext = context;
        }

        public void Dispose()
        {
            dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        //public void SaveChanges()
        //{
        //    dbContext.SaveChanges();
        //}
    }
}
