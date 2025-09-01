using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.Admin.Models.Roles
{
    public class RoleVM
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public List<LocalizationVM> Localizations { get; set; }
        public List<RolePermissionsVM> RolePermissions { get; set; }
        public List<int> Permissions { get; set; }
    }
}