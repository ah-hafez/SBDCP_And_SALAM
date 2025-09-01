using System;
using System.Collections.Generic;
using System.Configuration;
using MCS.Framework.Persistence;

namespace MCS.Domain.Search.SearchCriteria
{
    public class SearchCriteriaCustom
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool Ascending { get; set; }
        public string CultureName { get; set; }
        public List<Filter> Filters { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<SearchColunm> SearchColunms { get; set; }
        public int SearchData { get; set; }
        public int UserId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? FromDateTime
        {
            get
            {
                if (!string.IsNullOrEmpty(FromDate))
                {
                    string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                    DateTime dateValue;

                    if (DateTime.TryParseExact(FromDate, dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                    {
                        return dateValue;
                    }
                }

                return null;
            }
        }

        public DateTime? ToDateTime
        {
            get
            {
                if (!string.IsNullOrEmpty(ToDate))
                {
                    string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                    DateTime dateValue;

                    if (DateTime.TryParseExact(ToDate, dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                    {
                        return dateValue;
                    }
                }

                return null;
            }
        }

        string _orderBy = string.Empty;
        public string OrderBy
        {
            get
            {
                if (!string.IsNullOrEmpty(_orderBy))
                {
                    return _orderBy;
                }

                return "Id";
            }
            set { _orderBy = value; }
        }
        public List<OrderingBy> MultipleOrderBy { get; set; }
    }


    public class OrderingBy
    {
        public int Index { get; set; }
        public bool IsAscending { get; set; }
        public string ColumnName { get; set; }
    }

}

