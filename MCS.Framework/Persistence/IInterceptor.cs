using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Persistence
{
    public interface IInterceptor
    {
        void DoWork(IDbContext dbContext, List<Tuple<EntityState, DbEntityEntry, DbPropertyValues>> entities);        
    }
}
