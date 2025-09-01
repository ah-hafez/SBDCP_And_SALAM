using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Models.Groups
{
    public class AddGroupVM
    {
        public LookupVM Name { get; set; }

        [CustomDisplayName("Admin.User.Permissions")]
        [CustomRequired("Admin.Permissions.PermissionsRequired")]
        public List<int> Permissions { get; set; }
    }
}