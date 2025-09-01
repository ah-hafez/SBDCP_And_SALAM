using System.Collections.Generic;

namespace MCS.UI.Areas.Admin.Models.Permission
{
    public class PermissionGroupVM
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<PermissionVM> Permissions { get; set; }
        public bool IsUserDefined { get; set; }
    }
}