using System;
using System.Linq.Expressions;

namespace MCS.DataAccess
{
    public interface IRepository<T>
    {
        int Add(T entity);

        T Get(int id);

        void Update(T entity);

        void Delete(int id);

        T FindBy(Expression<Func<T, bool>> @where);
    }
}
