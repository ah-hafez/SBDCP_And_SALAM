using System;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class TransactionAssignmentDTO
    {
        public int Id { get; set; }
        public int TrayId { get; set; }
        public int FromOrgUnitId { get; set; }

        public int? ToUserId { get; set; }

        public string ToUserName { get; set; }

        public int? FromUserId { get; set; }
        public string FromUserName { get; set; }

        public string DateH { get; set; }

        public DateTime Date { get; set; }
        public string PhysicalDateH { get; set; }

        public DateTime PhysicalDate { get; set; }

        [CustomRequired("User.Transaction.Assignment.GroupIdRequired")]
        public int GroupId { get; set; }

        public string GroupName { get; set; }
        public int GroupOrderNo { get; set; }

        [CustomRequired("User.Transaction.Assignment.ActionIdRequired")]
        public int ActionId { get; set; }

        public string ActionName { get; set; }
        public string DeliveryMethod { get; set; }
        [CustomRequired("User.Transaction.DeliveryMethodIdRequired")]
        public int DeliveryMethodId { get; set; }

        public object[] ActionTypeId { get; set; }

        public int? ActionForAllId { get; set; }

        public string ActionNameForAll { get; set; }

        public object[] ActionTypeForAllId { get; set; }

        [CustomRequired("User.Transaction.Assignment.ToOrgUnitIdRequired")]
        public int ToOrgUnitId { get; set; }

        public string ToOrgUnitName { get; set; }

        public string FromOrgUnitName { get; set; }

        [CustomStringLength("User.Transaction.Assignment.RemarksLength", 500, 0)]
        public string Remarks { get; set; }

        [CustomStringLength("User.Transaction.Assignment.RemarksLength", 500, 0)]
        public string RemarksForAll { get; set; }

        public string TrayName { get; set; }

        public int Count { get; set; }

        private bool _isAssigned = true;
        public bool IsAssigned
        {
            get { return _isAssigned; }
            set { _isAssigned = value; }
        }
        public int? ReporterId { get; set; }
        public int PhysicalUserId { get; set; }
        public string PhysicalUserName { get; set; }
        public int PhysicalEntityId { get; set; }
        public string PhysicalEntityName { get; set; }
        public string StringContent { get; set; }
        public string SpecialExplanation { get; set; }
        public string GeneralExplanation { get; set; }
        public string ReceivedDate { get; set; }
        public string FromUserInternalNumber { get; set; }
        public string ToUserInternalNumber { get; set; }

    }
}
