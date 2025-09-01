using System;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models
{
    public class UserDelegationClientEditViewModel
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public int UserId { get; set; }
        [CustomRequired("User.UserDelegation.FromDateRequired")]
        [CustomDateTimeCompareAttribute("ToDate", Operation.LessThan, "User.UserDelegation.DateCompare")]
        public DateTime FromDate { get; set; }

        [CustomRequired("User.UserDelegation.ToDateRequired")]
        public DateTime ToDate { get; set; }

        [CustomDisplayName("User.UserDelegation.Unit")]
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
        public string OrgUnitName { get; set; }
        public string DirectedToName { get; set; }
        public string PriorityName { get; set; }
        public string ConfidentialityName { get; set; }
        public string TransactionTypeName { get; set; }
    }
}

