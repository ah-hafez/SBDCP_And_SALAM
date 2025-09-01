using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models
{
    public class EditUserCategoryVM
    {
        public int Id { get; set; }

        public List<LocalizationVM> Categories { get; set; }

        [CustomDisplayName("Admin.UserCategory.PermissionId")]
        [CustomRequired("Admin.UserCategory.PermissionId")]
        public int PermissionId { get; set; }
    }
}