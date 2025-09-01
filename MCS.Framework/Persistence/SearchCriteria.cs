using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Persistence
{
    public class SearchCriteria
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }        
        public bool Ascending { get; set; }
        public string CultureName { get; set; }
        public List<Filter> Filters { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<SearchColunm> SearchColunms { get; set; }

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

        public bool isDeleted { get; set; } = false;
        public int?  OrgUnitId { get; set; }
        public int? UserId { get; set; }

    }
}
