using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MCS.Framework.Entities;

namespace MCS.Framework.Persistence
{
    public class EFRepository<T> : IRepository<T> where T : EntityBase
    {
        private readonly IDbContext _dbContext;
        private readonly IDbSet<T> _dbSet;

        public EFRepository(IDbContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Added;
        }

        public T Get(int id)
        {
            return _dbSet.Find(id);
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
        }

        public IQueryable<T> GetAll(IList<Filter> filters = null, string orderBy = null, bool ascending = true,
              int? pageNo = null, int? pageSize = null, params Expression<Func<T, object>>[] navigationProperties)
        {
            IQueryable<T> query = _dbSet;

            if (filters != null)
            {
                foreach (Filter filter in filters)
                {
                    query = WhereQuery(query, filter.ColumnName, filter.Value, filter.Type);
                }
            }

            if (orderBy != null)
                query = OrderQuery(query, orderBy, ascending);

            if (pageNo != null && pageSize != null)
                query = query
                    .Skip((pageNo.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);

            if (navigationProperties.Length > 0)
            {
                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    query = query.Include<T, object>(navigationProperty);
            }

            return query;
        }

        public T FindBy(Expression<Func<T, bool>> @where)
        {
            return _dbSet.Where(@where).FirstOrDefault();
        }

        public IQueryable<T> OrderQuery(IQueryable<T> source, string ordering = null, bool ascending = true,
            params object[] values)
        {
            try
            {
                string orderByType = (ascending) ? "OrderBy" : "OrderByDescending";

                ordering = (!string.IsNullOrEmpty(ordering)) ? ordering : "Id";

                Type type = typeof(T);

                PropertyInfo propertyInfo = type.GetProperty(ordering);

                ParameterExpression parameterExpression = Expression.Parameter(type, "p");

                MemberExpression memberExpression = Expression.MakeMemberAccess(parameterExpression, propertyInfo);

                LambdaExpression orderByExp = Expression.Lambda(memberExpression, parameterExpression);

                MethodCallExpression resultExp = Expression.Call(typeof(Queryable), orderByType,
                    new Type[] { type, propertyInfo.PropertyType }, source.Expression, Expression.Quote(orderByExp));

                return source.Provider.CreateQuery<T>(resultExp);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<T> OrderQuery(IQueryable<T> source, string ordering, bool ascending = true,
            int? pageNo = null, int? pageSize = null, params object[] values)
        {
            IQueryable<T> query = OrderQuery(source, ordering, ascending, values);

            if (pageNo != null && pageSize != null)
            {
                query = query
                    .Skip((pageNo.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }

            return query;
        }

        public IQueryable<T> WhereQuery(IEnumerable<T> source, string columnName, string propertyValue, FilterType filterType)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(s =>
                    {
                        return s.GetType().GetProperty(columnName).GetValue(s, null).ToString().Contains(propertyValue);
                    }).AsQueryable();

                case FilterType.EndsWidth:
                    return source.Where(s =>
                    {
                        return s.GetType().GetProperty(columnName).GetValue(s, null).ToString().EndsWith(propertyValue);
                    }).AsQueryable();

                case FilterType.StartsWith:
                    return source.Where(s =>
                    {
                        return s.GetType().GetProperty(columnName).GetValue(s, null).ToString().StartsWith(propertyValue);
                    }).AsQueryable();

                case FilterType.GreaterThan:
                    return source.Where(s =>
                    {
                        return Convert.ToInt32(s.GetType().GetProperty(columnName).GetValue(s, null)) > Convert.ToInt32(propertyValue);
                    }).AsQueryable();

                case FilterType.GreaterThanOrEquals:
                    return source.Where(s =>
                    {
                        return Convert.ToInt32(s.GetType().GetProperty(columnName).GetValue(s, null)) >= Convert.ToInt32(propertyValue);
                    }).AsQueryable();

                case FilterType.LessThan:
                    return source.Where(s =>
                    {
                        return Convert.ToInt32(s.GetType().GetProperty(columnName).GetValue(s, null)) < Convert.ToInt32(propertyValue);
                    }).AsQueryable();

                case FilterType.LessThanOrEquals:
                    return source.Where(s =>
                    {
                        return Convert.ToInt32(s.GetType().GetProperty(columnName).GetValue(s, null)) <= Convert.ToInt32(propertyValue);
                    }).AsQueryable();
            }

            return source.Where(s => { return s.GetType().GetProperty(columnName).GetValue(s, null).ToString().Equals(propertyValue); }).AsQueryable();
        }

        //public IList<AuditInfo> GetAuditInfo()
        //{
        //    DbContext context = _dbContext as DbContext;

        //    AuditTrailFactory auditTrail = new AuditTrailFactory(context);

        //    IEnumerable<DbEntityEntry> entityList = context.ChangeTracker.Entries().Where(p => p.State == EntityState.Added ||
        //       p.State == EntityState.Deleted || p.State == EntityState.Modified);

        //    IList<AuditInfo> auditInfoList = new List<AuditInfo>();

        //    foreach (DbEntityEntry entity in entityList)
        //    {
        //        if (entity.Entity.GetType().GetInterfaces().Contains(typeof(IAuditable)))
        //        {
        //            AuditInfo auditInfo = auditTrail.GetAudit(entity);

        //            auditInfoList.Add(auditInfo);
        //        }
        //    }

        //    return auditInfoList;
        //}
    }
}
