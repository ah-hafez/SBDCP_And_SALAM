using System.Reflection;

namespace MCS.Framework.Controls.Mvc
{
    internal interface IGridAnnotaionsProvider
    {
        GridColumnAttribute GetAnnotationForColumn<T>(PropertyInfo pi);
        GridHiddenColumnAttribute GetAnnotationForHiddenColumn<T>(PropertyInfo pi);

        bool IsColumnMapped(PropertyInfo pi);

        GridTableAttribute GetAnnotationForTable<T>();
    }
}