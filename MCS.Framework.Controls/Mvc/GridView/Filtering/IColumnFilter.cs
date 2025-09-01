using System.Linq;

namespace MCS.Framework.Controls.Mvc
{
    public interface IColumnFilter<T>
    {
        IQueryable<T> ApplyFilter(IQueryable<T> items, ColumnFilterValue value);
    }
}