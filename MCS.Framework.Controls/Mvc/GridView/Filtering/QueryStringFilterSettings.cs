using System;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MCS.Framework.Controls.Mvc
{
    /// <summary>
    ///     Object gets filter settings from query string
    /// </summary>
    public class QueryStringFilterSettings : IGridFilterSettings
    {
        private static readonly object _transaction = new object();
        public const string DefaultTypeQueryParameter = "grid-filter";
        public const string PageSizeQueryParameter = "pageSize";
        private const string FilterDataDelimeter = "__";
        public const string DefaultFilterInitQueryParameter = "gridinit";
        public readonly HttpContext Context;
        private readonly DefaultFilterColumnCollection _filterValues = new DefaultFilterColumnCollection();

        #region Ctor's

        public QueryStringFilterSettings()
            : this(HttpContext.Current)
        {
        }

        public QueryStringFilterSettings(HttpContext context)
        {
            lock (_transaction)
            {
                Context = context ?? throw new ArgumentException("No http context here!");

                int pageSize = 10; //default page size

                if (!string.IsNullOrEmpty(Context.Request.QueryString[PageSizeQueryParameter]))
                {
                    pageSize = Convert.ToInt32(Context.Request.QueryString[PageSizeQueryParameter]);
                }
                else if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["GridPageSize"]))
                {
                    pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
                }

                GridHelper.PageSize = pageSize;

                string[] filters = Context.Request.QueryString.GetValues(DefaultTypeQueryParameter);

                if (filters != null)
                {
                    foreach (string filter in filters)
                    {
                        ColumnFilterValue column = CreateColumnData(filter);
                        if (column != ColumnFilterValue.Null)
                            _filterValues.Add(column);
                    }
                }
            }
        }

        #endregion

        private ColumnFilterValue CreateColumnData(string queryParameterValue)
        {
            if (string.IsNullOrEmpty(queryParameterValue))
                return ColumnFilterValue.Null;

            string[] data = queryParameterValue.Split(new[] { FilterDataDelimeter }, StringSplitOptions.RemoveEmptyEntries);
            if (data.Length != 3)
                return ColumnFilterValue.Null;
            GridFilterType type;
            if (!Enum.TryParse(data[1], true, out type))
                type = GridFilterType.Equals;

            return new ColumnFilterValue { ColumnName = data[0], FilterType = type, FilterValue = data[2] };
        }

        #region IGridFilterSettings Members

        public IFilterColumnCollection FilteredColumns
        {
            get { return _filterValues; }
        }

        public bool IsInitState
        {
            get
            {
                if (FilteredColumns.Any()) return false;
                return Context.Request.QueryString[DefaultFilterInitQueryParameter] != null;
            }
        }

        #endregion
    }
}