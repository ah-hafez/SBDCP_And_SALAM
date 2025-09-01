using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class AddUserCategoryDTO
    {

        public List<LocalizationDTO> Categories { get; set; }

        //[CustomDisplayName("Admin.UserCategory.PermissionId")]
        [CustomRequired("Admin.UserCategory.PermissionId")]
        public int PermissionId { get; set; }
    }
}
