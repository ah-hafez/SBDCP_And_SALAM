using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.Admin.Models.UserCategories
{
    public class AddUserCategoryVM
    {

        public List<LocalizationVM> Categories { get; set; }

        [CustomDisplayName("Admin.UserCategory.PermissionId")]
        [CustomRequired("Admin.UserCategory.PermissionId")]
        public int PermissionId { get; set; }
    }
}