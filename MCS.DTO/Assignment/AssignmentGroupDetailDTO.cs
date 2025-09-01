using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class AssignmentGroupDetailDTO
    {
        public int Id { get; set; }

        //[CustomDisplayName("User.Transaction.AssignmentDetail.UserProfileId")]
        public int? UserProfileId { get; set; }

        public string UserProfileName { get; set; }

        //[CustomDisplayName("User.Transaction.AssignmentDetail.OrgUnitId")]
        [CustomRequired("User.Transaction.AssignmentDetail.OrgUnitIdRequired")]
        public int OrgUnitId { get; set; }

        public string OrgUnitName { get; set; }
    }
}
