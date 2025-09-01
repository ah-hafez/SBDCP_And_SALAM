using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework.Entities;

namespace MCS.Framework.Persistence
{
    public interface IRepository<T> where T : EntityBase
    {
        void Add(T entity);

        T Get(int id);

        void Update(T entity);

        void Delete(T entity);

        IQueryable<T> GetAll(IList<Filter> filters = null, string orderBy = null, bool ascending = false,
                int? pageNo = null, int? pageSize = null, params Expression<Func<T, object>>[] navigationProperties);

        T FindBy(Expression<Func<T, bool>> @where);
    }
}
