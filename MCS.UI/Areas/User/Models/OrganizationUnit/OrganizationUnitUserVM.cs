using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.OrgUnit
{
    public class OrgUnitUserVM
    {
        [CustomRequired("Admin.OrgUnitUsers.UserName")]
        [CustomDisplayName("Admin.OrgUnitUsers.UserName")]
        public int Id { get; set; }

        public string UserName { get; set; }
    }
}