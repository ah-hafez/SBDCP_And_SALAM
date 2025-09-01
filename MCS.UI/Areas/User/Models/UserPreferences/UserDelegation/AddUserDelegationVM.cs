using System;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.UserPreferences.UserDelegation
{
    public class AddUserDelegationVM
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [CustomDateTimeCompareAttribute("ToDate", Operation.LessThan, "User.UserDelegation.DateCompare")]
        public DateTime FromDate { get; set; }

        [CustomDateTimeCompareAttribute("FromDate", Operation.GreaterThan)]
        public DateTime ToDate { get; set; }
        public int OrgUnitId { get; set; }
        public int DirectedToId { get; set; }
        public int PriorityId { get; set; }
        public int ConfidentialityId { get; set; }
        public int SourceTypesId { get; set; }
        //public TransactionSourceDTO sourceType { get; set; }
        public string FromDateH { get; set; }
        public string ToDateH { get; set; }
    }
}