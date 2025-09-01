using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using MCS.Framework.Persistence;

namespace MCS.GridMvc.Helpers
{
    public class GridHelper
    {
        private const string filterDataDelimeter = "__";
        private static readonly object _transaction = new object();

        public static string GetGridParameters()
        {
            lock (_transaction)
            {
                StringBuilder result = new StringBuilder();

                string filter = HttpContext.Current.Request.QueryString["grid-filter"];
                string sortColumnName = HttpContext.Current.Request.QueryString["gridColumn"];
                string dir = HttpContext.Current.Request.QueryString["dir"];
                string pageIndex = HttpContext.Current.Request.QueryString["page"];
                string pageSizeText = HttpContext.Current.Request.QueryString["pageSize"];


                result.Append("CultureName=").Append(System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);

                FilterType filterType;

                if (filter != null)
                {
                    string[] filterData = filter.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < filterData.Length; i++)
                    {
                        string[] data = filterData[i].Split(new[] { filterDataDelimeter },
                        StringSplitOptions.RemoveEmptyEntries);

                        string filterValue = data.Count() == 3 ? data[2] : string.Empty;

                        string[] columnName = data[0].Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);

                        if (!Enum.TryParse(data[1], true, out filterType))
                            filterType = FilterType.Equals;

                        result.Append("&Filters[").Append(i).Append("].ColumnName=")
                              .Append(columnName[0]).Append("&Filters[").Append(i)
                              .Append("].Type=").Append(filterType).Append("&Filters[")
                              .Append(i).Append("].Value=").Append(filterValue);
                    }
                }

                if (sortColumnName != null)
                {
                    string[] sortData = sortColumnName.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);

                    if (sortData.Length > 1)
                    {
                        result.Append("&OrderBy=").Append(sortData[0]);
                    }
                    else
                    {
                        result.Append("&OrderBy=").Append(sortData[0]);
                    }
                }

                if (!string.IsNullOrEmpty(pageSizeText))
                {
                    PageSize = Convert.ToInt32(pageSizeText);
                }

                result.Append("&PageSize=").Append(PageSize);

                if (dir == "1")
                {
                    result.Append("&Ascending=").Append(true);
                }
                else
                {
                    result.Append("&Ascending=").Append(false);

                }

                if (!string.IsNullOrEmpty(pageIndex))
                {
                    int page = Convert.ToInt32(pageIndex);

                    result.Append("&PageIndex=").Append(page);
                }
                else
                {
                    result.Append("&PageIndex=").Append(1);

                }
                return result.ToString();
            }
        }
        private static int pageSize = 0;
        public static int PageSize
        {
            get
            {
                if (pageSize == 0 && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["GridPageSize"]))
                {
                    pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
                }

                return pageSize;
            }
            set { pageSize = value; }
        }

        public static void ResetPageSize()
        {
            PageSize = 0;
        }

        public static int PagePartitionSize
        {
            get
            {

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["GridPagePartitionSize"]))
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["GridPagePartitionSize"]);
                }

                throw new Exception("GridPagePartitionSize not configured in the web config file");
            }

        }
    }
}
