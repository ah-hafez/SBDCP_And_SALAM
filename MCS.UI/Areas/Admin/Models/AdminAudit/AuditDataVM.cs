using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Models.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.Admin.Models.AdminAudit
{   
    public class AdminAuditVM
    {
        public string Schema { get; set; }
        public string Table { get; set; }
        public PrimaryKey PrimaryKey { get; set; }
        public string Action { get; set; }
        public List<Change> Changes { get; set; }
        public bool Valid { get; set; }
        public List<AdminAuditGridVM> AdminAuditGridVMs { get; set; } = (AjaxGrid<AdminAuditGridVM>)new AjaxGridFactory().CreateAjaxGrid(new List<AdminAuditGridVM>(), 1, 0, false);

    }


}