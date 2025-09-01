using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class OrgUnitLinkDTO
    {
        [CustomRequired("Admin.OrgUnitLinks.OrgUnitName")]
        //[CustomDisplayName("Admin.OrgUnitLinks.OrgUnitName")]
        public int Key { get; set; }

        public string OrgUnitName { get; set; }
    }
}
