using System;
using MCS.Common;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Assignment
{

    public class TransactionRejectAssignmentVM
    {
        public TransactionRejectAssignmentVM()
        {
            TrayID = (int)TrayType.MyTransactions;
        }
        public TransactionCategory Type { get; set; }
        public TransactionRejectAssignmentVM(int trayId, bool returnToCreator = false)
        {
            TrayID = trayId;
            ReturnToCreator = returnToCreator;
        }
        public int Id { get; set; }

        public int FromOrgUnitId { get; set; }

        [CustomDisplayName("User.Transaction.Assignment.ToUserId")]
        [CustomRequired("User.Transaction.Assignment.ToUserIdRequired")]
        public int? ToUserId { get; set; }
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

        public string ActionName { get; set; }

        public object[] ActionTypeId { get; set; }

        [CustomDisplayName("User.Transaction.Assignment.ActionForAllId")]
        public int? ActionForAllId { get; set; }

        public string ActionNameForAll { get; set; }

        public object[] ActionTypeForAllId { get; set; }

        [CustomDisplayName("User.Transaction.Assignment.ToOrgUnitId")]
        [CustomRequired("User.Transaction.Assignment.ToOrgUnitIdRequired")]
        public int ToOrgUnitId { get; set; }

        public string ToOrgUnitName { get; set; }

        public string FromOrgUnitName { get; set; }

        [CustomDisplayName("User.Transaction.Assignment.Remarks")]
        [CustomStringLength("User.Transaction.Assignment.RemarksLength", 500)]
        [CustomRequired("User.Transaction.Assignment.Reason.Required")]
        public string Remarks { get; set; }

        [CustomDisplayName("User.Transaction.Assignment.RemarksForAll")]
        [CustomStringLength("User.Transaction.Assignment.RemarksLength", 500, 0)]
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
        public string DeliveryMethod { get; set; }
        [CustomDisplayName("User.Transaction.Assignment.DeliveryMethod")]
        [CustomRequired("User.Transaction.DeliveryMethodIdRequired")]
        public int DeliveryMethodId { get; set; }

        public int TrayID { get; set; }

        [CustomDisplayName("User.Transaction.Outbound.Reporter")]
        //[CustomRequired("User.Transaction.ReporterIdRequired")]
        public int? ReporterId { get; set; }
        public string Title { get; set; }
        public string URLAction { get; set; }

        public bool ReturnToCreator { get; set; } = false;
    }
}