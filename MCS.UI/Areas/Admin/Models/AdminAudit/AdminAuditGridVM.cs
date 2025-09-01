using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.Admin.Models.AdminAudit
{   
    public class AdminAuditGridVM
    {
        public string UserName { get; set; }
        public string Table { get; set; } 
        public string Action { get; set; }
        public string OriginalValue { get; set; }
        public string NewValue { get; set; }
        public DateTime AuditDate { get; set; }
       
    }

 
}