using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class UserGroupVM
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string GroupName { get; internal set; }
        public bool IsPrint { get; internal set; }
        public List<int> ColumnsToGrid { get; set; }

        public string Name { get; set; }
        public string OrgUnitName { get; internal set; }
        public string AdminUserName { get; set; }
        public List<string> OrgUnitNames { get; internal set; }
    }
}