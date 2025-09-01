using MCS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Models
{
    public class TransactionVM
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OrgUnitId { get; set; }
        public DateTime RecordDate { get; set; }
        public String HijriRecordDate { get; set; }
        public int StatusId { get; set; }
        public bool IsSigned { get; set; }
        public virtual TransactionCategory Type { get; }
        public DocumentVM DocumentVM { get; set; }
        //public TransactionAssignmentVM TransactionAssignmentVM { get; set; } = new TransactionAssignmentVM();
        //public List<TransactionNameVM> Names { get; set; } = (AjaxGrid<TransactionNameVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionNameVM>(), 1, 0, false);
        //public List<TransactionLinkVM> Links { get; set; } = (AjaxGrid<TransactionLinkVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionLinkVM>(), 1, 0, false);
        //public List<TransactionAttachmentVM> Attachments { get; set; } = (AjaxGrid<TransactionAttachmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAttachmentVM>(), 1, 0, false);
        //public List<TransactionCopyVM> Copies { get; set; } = (AjaxGrid<TransactionCopyVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionCopyVM>(), 1, 0, false);
        //public List<TransactionExternalCopyVM> ExternalCopies { get; set; } = (AjaxGrid<TransactionExternalCopyVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionExternalCopyVM>(), 1, 0, false);
        //public List<TransactionArchiveVM> Archives { get; set; } = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionArchiveVM>(), 1, 0, false);
        public UserProfileVM AssignedFromUser { get; set; }
        public UserProfileVM AssignedToUser { get; set; }
        public int? ProcessPeriodTransaction { get; set; }
        public int? SideContactExternalEntityID { get; set; }
        public string NumberContact { get; set; }
        public bool IsEnableAssignBack { get; set; }

    }
}