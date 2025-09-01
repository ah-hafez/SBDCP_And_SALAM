using System.Collections.Generic;
using MCS.UI.Areas.Admin.Models.Permission;
using MCS.UI.Areas.Admin.Models.Tray;

namespace MCS.UI.Areas.Admin.Models.UserCategories
{
    public class UsersTraysAndPermissionsViewModel
    {
        public List<PermissionVM> Permissions { get; set; }
        public List<TrayVM> Trays { get; set; }
        public List<UserProfileVM> Users { get; set; }

        public UsersTraysAndPermissionsViewModel()
        {
            Permissions = new List<PermissionVM>();
            Trays = new List<TrayVM>();
            Users = new List<UserProfileVM>();
        }
    }
}