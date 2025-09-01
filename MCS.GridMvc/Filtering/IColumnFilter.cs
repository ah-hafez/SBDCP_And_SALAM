using System.Linq;

namespace MCS.GridMvc.Filtering
{
    public interface IColumnFilter<T>
    {
        IQueryable<T> ApplyFilter(IQueryable<T> items, ColumnFilterValue value);
    }
}