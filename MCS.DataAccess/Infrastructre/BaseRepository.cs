using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MCS.Framework.Entities;
using MCS.Framework.Persistence;
using MCS.Common.TransactionContext;

namespace MCS.DataAccess
{
    public class BaseRepository<T> : IRepository<T> where T : EntityBase
    {
        private readonly IAmbienTTransactionContextLocator _ambienTTransactionContextLocator;
        public readonly MCSDbContext _oMCSDbContext;
        private readonly IDbSet<T> _dbSet;
        private ISystemDefaultValuesRepository ambienTTransactionContextLocator;

        public BaseRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
        {
            _ambienTTransactionContextLocator = ambienTTransactionContextLocator;
            _oMCSDbContext = _ambienTTransactionContextLocator.Get<MCSDbContext>();
            _dbSet = _oMCSDbContext.Set<T>();
        }

        public BaseRepository(ISystemDefaultValuesRepository ambienTTransactionContextLocator)
        {
            this.ambienTTransactionContextLocator = ambienTTransactionContextLocator;
        }

        public int Add(T entity)
        {
            try
            {
                _dbSet.Add(entity);
                _oMCSDbContext.SaveChanges();
                return entity.Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public T Get(int id)
        {
            return _dbSet.FirstOrDefault(e => e.Id == id);
        }
        public virtual void Update(T entity)
        {
            try
            {
                T obj = this.FindBy(a => a.Id == entity.Id);
                _oMCSDbContext.Entry(obj).CurrentValues.SetValues(entity);
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public virtual void Delete(int id)
        {
            try
            {
                T deletedEntity = _dbSet.FirstOrDefault(d => d.Id == id);
                _dbSet.Remove(deletedEntity);
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public T FindBy(Expression<Func<T, bool>> @where)
        {
            return _dbSet.Where(@where).FirstOrDefault();
        }
        public IQueryable<T> OrderQuery(IQueryable<T> source, string ordering = null, bool ascending = true, params object[] values)
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
        public IQueryable<T> OrderQuery(IQueryable<T> source, string ordering, bool ascending = true, int? pageNo = null, int? pageSize = null, params object[] values)
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
                        return s.GetType().GetProperty(columnName).GetValue(s, null).ToString().ToLower().Contains(propertyValue.ToLower());
                    }).AsQueryable();

                case FilterType.EndsWidth:
                    return source.Where(s =>
                    {
                        return s.GetType().GetProperty(columnName).GetValue(s, null).ToString().ToLower().EndsWith(propertyValue.ToLower());
                    }).AsQueryable();

                case FilterType.StartsWith:
                    return source.Where(s =>
                    {
                        return s.GetType().GetProperty(columnName).GetValue(s, null).ToString().ToLower().StartsWith(propertyValue.ToLower());
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

            return source.Where(s => { return s.GetType().GetProperty(columnName).GetValue(s, null).ToString().ToLower().Equals(propertyValue.ToLower()); }).AsQueryable();
        }
    }
}