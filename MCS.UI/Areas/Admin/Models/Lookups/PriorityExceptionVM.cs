using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class PriorityExceptionVM
    {
        public int Id { get; set; }
        public int Key { get; set; }
        public int PriorityId { get; set; }
        [CustomDisplayName("Admin.Lookups.PriorityExceptions.OrgUnit")]
        public int OrgUnitId { get; set; }
        public int? UserId { get; set; }
        public int LateOnUsersAfter { get; set; }
        public string UserName { get; set; }
        public string OrgUnitName { get; set; }
    }
}