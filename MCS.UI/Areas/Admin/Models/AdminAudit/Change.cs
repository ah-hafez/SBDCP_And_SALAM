using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.Admin.Models.AdminAudit
{
    public class Change
    {
        public string ColumnName { get; set; }
        public object OriginalValue { get; set; }
        public object NewValue { get; set; }
    }
}