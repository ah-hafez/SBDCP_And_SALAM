using System;
using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TransactionCopyVM : EntityBase
    {
        public int Id { get; set; }
        public string ActionName { get; set; }

        [CustomDisplayName("User.Transaction.Copies.Employee")]

        public int? UserId { get; set; }
        public string UserName { get; set; }
        [CustomDisplayName("User.Transaction.Copies.OrgUnit")]
        [CustomRequired("User.Transaction.Copies.OrgUnitRequired")]
        public int OrgUnitId { get; set; }
        public string OrgSelectedList { get; set; }
        public string OrgUnitName { get; set; }
        public int? FromUserId { get; set; }
        public string FromUserName { get; set; }
        public int FromOrgUnitId { get; set; }
        public string FromOrgUnitName { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        [CustomDisplayName("User.Transaction.Assignment.Reason")]
        [CustomRequired("User.Transaction.Copies.ReasonRequired")]

        public int ActionId { get; set; }
        public object[] ActionTypeId { get; set; }
        public int? IsSent { get; set; }
        public List<TransactionCopyVM> Copies { get; set; } = (AjaxGrid<TransactionCopyVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionCopyVM>(), 1, 0, false);

        public bool SendEmail { get; set; }
        public int Status { get; set; }
        public string UserList { get; set; }

        public DateTime? SentDate { get; set; }
        public string SpecialExplanation { get; set; }
        public string GeneralExplanation { get; set; }
        public bool SpecialCopy { get; set; }
        public bool IsBcc { get; set; }
        public bool IsOpr { get; set; }

        public int? OprEntityId { get; set; }
        public string OprEntityName { get; set; }
        public string ViewedOnDateH { get; set; }
        public string ViewedBy { get; set; }

    }
}