using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.OrgUnit
{
    public class OrgUnitLinkVM
    {
        [CustomRequired("Admin.OrgUnitLinks.OrgUnitName")]
        [CustomDisplayName("Admin.OrgUnitLinks.OrgUnitName")]
        public int Key { get; set; }

        public string OrgUnitName { get; set; }
    }
}