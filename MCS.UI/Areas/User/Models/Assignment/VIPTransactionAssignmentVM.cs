using System;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Assignment
{

    public class VIPTransactionAssignmentVM
    {
        public int Id { get; set; }

        [CustomDisplayName("User.Transaction.AssignmentDetail.UserProfileId")]
        public int? ToUserId { get; set; }

        [CustomDisplayName("User.Transaction.Assignment.ActionId")]
        [CustomRequired("User.Transaction.Assignment.ActionIdRequired")]
        public int ActionId { get; set; }
        public string ToUserName { get; set; }
        public string ToOrgUnitName { get; set; }
        public int GroupId { get; set; }

        public string GroupName { get; set; }
        public int GroupOrderNo { get; set; }

        [CustomDisplayName("User.Transaction.Assignment.ToOrgUnit")]
        [CustomRequired("User.Transaction.Assignment.ToOrgUnitIdRequired")]
        public int ToOrgUnitId { get; set; }

        [CustomDisplayName("User.Transaction.Assignment.Remarks")]
        [CustomStringLength("User.Transaction.Assignment.RemarksLength", 500)]
        public string Remarks { get; set; }


        private bool? _isAssigned = false;
        public bool? IsAssigned
        {
            get { return _isAssigned; }
            set { _isAssigned = value; }
        }
        public bool? IsCopy { get; set; } = false;
        public bool ChkConstant { get; set; }
        public string ActionName { get; set; }
        public int Key { get; set; }
        public int DeliveryMethodId { get; set; }
        [CustomDisplayNameAttribute("User.Transaction.Copies.SpecialExplanation")]
        public string SpecialExplanation { get; set; }

        public bool IsBcc { get; set; }
        public int OrderNo { get; set; }
        public int OrderNoGroup { get; set; }

    }
}