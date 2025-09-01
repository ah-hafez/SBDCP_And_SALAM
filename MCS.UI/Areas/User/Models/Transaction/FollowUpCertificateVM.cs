using System;
using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.Domain;
using MCS.DTO;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class FollowUpCertificateVM  
    {
        public int FollowUpId { get; set; }
        public int TransactionId { get; set; }
        public int CreatingUserId { get; set; }
        public string CreatingUserName { get; set; }
        public int CreatingEntityId { get; set; } 
        public string CreatingEntityName { get; set; }
        [CustomDisplayName("User.FollowUpCertificate.FollowUp.FollowUpUser")]
        public int? FollowUpUserId { get; set; }
        public string FollowUpUserName { get; set; }
        public string FollowUpUserAsignName { get; set; }
        [CustomDisplayName("User.FollowUpCertificate.FollowUp.FollowUpEntity")]
        public int FollowUpEntityId { get; set; }
        public string FollowUpEntityName { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationDateHj { get; set; }
        public DateTime FollowUpExpireDate { get; set; }
        public string FollowUpExpireDateHj { get; set; }
        public string FollowUpReason { get; set; }
        public string FollowUpType { get; set; }
        public int FollowUpTypeId { get; set; }
        public int FollowUpStatusId { get; set; }
        public int FollowUpMethodId { get; set; }
        public int FollowUpPriortyId { get; set; }
        public int FollowUpProccessId { get; set; }
        public int FollowUpSourceId { get; set; }
        public int? FollowUpProgressId { get; set; } 
        public string FollowUpProccessNote { get; set; } 
        public string Notes { get; set; }
        public bool Active { get; set; }
        public int ProccessPeriod { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsFinalCompleted { get; set; }
        public bool IsUnderFollowup { get; set; }
        public bool IsCanceld { get; set; }
        public bool ReadOnly { get; set; }
        public DateTime? ProccessPeriodDate { get; set; }
        public DateTime? FollowUpCompletionDate { get; set; }
        public string FollowUpCompletionDateHj { get; set; }
        public DateTime? FollowUpReceiveDate { get; set; }
        public string FollowUpReceiveDateHj { get; set; }
        public bool IsCopy { get; set; }
        public bool IsReminder { get; set; }
        public bool IsEscalated { get; set; }
        public bool IsImportant { get; set; }
        public bool HasChild { get; set; }
        public int? ParentId { get; set; }
        public List<TransactionFollowUpVM> FollowUps { get; set; } = (AjaxGrid<TransactionFollowUpVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionFollowUpVM>(), 1, 0, false);
        public List<FollowUpAuditTrailVM> FollowUpAuditTrails { get; set; } = (AjaxGrid<FollowUpAuditTrailVM>)new AjaxGridFactory().CreateAjaxGrid(new List<FollowUpAuditTrailVM>(), 1, 0, false);
        public TransactionLinkVM transactionLinkVM { get; set; }
    }
}