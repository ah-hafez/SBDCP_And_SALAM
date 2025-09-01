using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class EditGroupDTO
    {
        public int Id { get; set; }
        public LookupDTO Name { get; set; }
        //[CustomDisplayName("Admin.User.Permissions")]
        [CustomRequired("Admin.Permissions.PermissionsRequired")]
        public List<int> Permissions { get; set; }
    }
}
