using System;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class EditUserDelegationDTO
    {
        public int Id { get; set; }
        public int PreferenceId { get; set; }
        [CustomRequired("User.UserDelegation.FromDateRequired")]
        [CustomDateTimeCompareAttribute("ToDate", Operation.LessThan, "User.UserDelegation.DateCompare")]
        public DateTime FromDate { get; set; }

        [CustomRequired("User.UserDelegation.ToDateRequired")]
        public DateTime ToDate { get; set; }
        //[CustomDisplayName("User.UserDelegation.Unit")]
        [CustomRequired("User.UserDelegation.OrgUnitRequired")]
        public int OrgUnitId { get; set; }
        [CustomRequired("User.UserDelegation.DirectToRequired")]
        public int DirectedToId { get; set; }
        [CustomRequired("User.UserDelegation.PriorityRequired")]
        public int PriorityId { get; set; }

        [CustomRequired("User.UserDelegation.ConfidentialityRequired")]
        public int ConfidentialityId { get; set; }

        [CustomRequired("User.UserDelegation.SourceTypeRequired")]
        public int SourceTypesId { get; set; }

        public string FromDateH { get; set; }
        public string ToDateH { get; set; }

        public int StatusId { get; set; }
        public string Status { get; set; }
        public string RejectionReason { get; set; }
    }
}
