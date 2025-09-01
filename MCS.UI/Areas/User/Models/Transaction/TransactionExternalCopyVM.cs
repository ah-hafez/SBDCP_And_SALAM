using System;
using System.Collections.Generic;
using System.Web;
using MCS.Common.CustomAttributes;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TransactionExternalCopyVM : EntityBase
    {
        public int Id { get; set; }
        public string ActionName { get; set; }
        [CustomDisplayName("User.Transaction.Copies.ToUser")]
        public int? UserId { get; set; }
        public string UserName { get; set; }
        [CustomDisplayName("User.Transaction.Copies.ExternalParty")]
        [CustomRequired("User.Transaction.Copies.OrgUnitRequired")]
        public int OrgUnitId { get; set; }
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
        public List<TransactionExternalCopyVM> ExternalCopies { get; set; } = (AjaxGrid<TransactionExternalCopyVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionExternalCopyVM>(), 1, 0, false);
        public List<ExternalPartyAttachmentVM> externalPartyAttachmentVMs { get; set; }
        public int AttachmentCount { get; set; }
        public string attachmentNames { get; set; }
        public int Status { get; internal set; }
        public int TransactionId { get; set; }
        public bool IsNewFile { get; set; }       
        public bool SendEmail { get; set; }
        public string Email { get; set; }
        public string ExternalOrgSelectedList { get; set; }
     


    }
}