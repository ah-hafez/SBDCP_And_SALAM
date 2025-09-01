using System;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Assignment
{

    public class VipCopyTransactionAssignmentVM
    {
        public int Id { get; set; }
        public int TrayId { get; set; }
        public int FromOrgUnitId { get; set; }

        [CustomDisplayName("User.Transaction.AssignmentDetail.UserProfileId")]
        public int? ToUserId { get; set; }
        public int? FromUserId { get; set; }
        public string ToUserName { get; set; }

        public string FromUserName { get; set; }

        public string DateH { get; set; }

        public DateTime Date { get; set; }

        [CustomDisplayName("User.Transaction.Assignment.GroupId")]
        [CustomRequired("User.Transaction.Assignment.GroupIdRequired")]
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        [CustomDisplayName("User.Transaction.Assignment.ActionId")]
        [CustomRequired("User.Transaction.Assignment.ActionIdRequired")]
        public int ActionId { get; set; }
        [CustomDisplayName("User.Transaction.Assignment.Privatexplanation")]
        public string Privatexplanation { get; set; }
        public string ActionName { get; set; }

        public object[] ActionTypeId { get; set; }

        [CustomDisplayName("User.Transaction.Assignment.ActionForAllId")]
        public int? ActionForAllId { get; set; }

        public string ActionNameForAll { get; set; }

        public object[] ActionTypeForAllId { get; set; }

        [CustomDisplayName("User.Transaction.Assignment.ToOrgUnit")]
        public int? ToOrgUnitId { get; set; }

        public string ToOrgUnitName { get; set; }

        public string FromOrgUnitName { get; set; }

        [CustomDisplayName("User.Transaction.Assignment.Remarks")]
        [CustomStringLength("User.Transaction.Assignment.RemarksLength", int.MaxValue)]
        public string Remarks { get; set; }

        [CustomDisplayName("User.Transaction.Assignment.RemarksForAll")]
        [CustomStringLength("User.Transaction.Assignment.RemarksLength", int.MaxValue, 0)]
        public string RemarksForAll { get; set; }

        [CustomDisplayName("User.Transaction.Assignment.TrayName")]
        public string TrayName { get; set; }

        public int Count { get; set; }

        private bool _isAssigned = true;
        public bool IsAssigned
        {
            get { return _isAssigned; }
            set { _isAssigned = value; }
        }
        public bool IsCopy { get; set; }
       
        [CustomDisplayName("User.Transaction.Assignment.DeliveryMethod")]
        public string DeliveryMethod { get; set; }
        [CustomDisplayName("User.Transaction.Assignment.DeliveryMethod")]
        [CustomRequired("User.Transaction.DeliveryMethodIdRequired")]
        public int DeliveryMethodId { get; set; }

        public int Sequence { get; set; }

        [CustomDisplayName("User.Transaction.Outbound.Reporter")]
        //[CustomRequired("User.Transaction.ReporterIdRequired")]
        public int? ReporterId { get; set; }
        public int PhysicalUserId { get; set; }
        public string PhysicalUserName { get; set; }
        public int PhysicalEntityId { get; set; }
        public string PhysicalEntityName { get; set; }
        public string PhysicalDateH { get; set; }
        public DateTime PhysicalDate { get; set; }

        public string StringContent { get; set; }

        public int? UserImageId { get; set; }

        public int Key { get; set; }
        public string FollowupDateTo { get; set; }
        public int? FollowUpProccessId { get; set; }
        [CustomDisplayNameAttribute("User.Editor.ToFollowUp")]
        public bool ToFollowUp { get; set; }
        public int? ProccessPeriod { get; set; }
        public string FollowupDateToH { get; set; }
        public string Duration { get; set; }
        [CustomDisplayNameAttribute("User.Transaction.Copies.SpecialExplanation")]
        public string SpecialExplanation { get; set; }
        public bool ChkConstant { get; set; }

        public bool SpecialCopy { get; set; }
        public bool IsBcc { get; set; }
        public bool IsOpr { get; set; }

        public int? OprEntityId { get; set; }
        public string GeneralExplanation { get; set; }
    }
}