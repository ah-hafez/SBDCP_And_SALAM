using System;
using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.Domain;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class VIPTransactionFollowUpVM : EntityBase
    {
        public int Id { get; set; }

        [CustomDisplayName("User.Transaction.FollowUp.Employee")]
        public int? UserId { get; set; }

        [CustomDisplayName("User.Transaction.FollowUp.Entity")]
        public int ToEntityId { get; set; }

        [CustomDisplayName("User.Transaction.FollowUp.Entity")]
        public string ToEntityName { get; set; }

        [CustomDisplayName("User.Transaction.FollowUp.Employee")]
        public string EmployeeName { get; set; }
        public string EmployeeEntityName { get; set; }

        public int TransactionId { get; set; }

        [CustomDisplayName("FollowUp.EndDate")]
        public DateTime? DateTo { get; set; }

        [CustomDisplayName("FollowUp.EndDate")]
        public string DateToH { get; set; }
        public bool IsDeleted { get; set; }

        public int FollowUpId { get; set; }
        public int CreatingUserId { get; set; }
        public string CreatingUserName { get; set; }
        public int CreatingEntityId { get; set; }
        public string CreatingEntityName { get; set; }
        [CustomDisplayName("User.Transaction.FollowUp.Entity")]
        public int FollowUpEntityId { get; set; }
        public string FollowUpEntityName { get; set; }
        [CustomDisplayName("User.Transaction.FollowUp.Employee")]
        public int? FollowUpUserId { get; set; }

        public string FollowUpUserName { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationDateHj { get; set; }
        public DateTime FollowUpExpireDate { get; set; }
        public string FollowUpExpireDateHj { get; set; }
        public string Notes { get; set; }
        public bool Active { get; set; }
        public int? ProccessPeriod { get; set; }
        public DateTime? ProccessPeriodDate { get; set; }

        public string FollowUpProccessNote { get; set; }

        public DateTime? FollowUpCompletionDate { get; set; }
        public DateTime? FollowUpReceiveDate { get; set; }
        public string FollowUpReceiveDateHj { get; set; }

        public string FollowUpReason { get; set; }

        public int FollowUpTypeId { get; set; }
        public string FollowUpType { get; set; }
        public int FollowUpStatusId { get; set; }
        public string FollowUpStatus { get; set; }
        public string FollowUpMethod { get; set; }
        public int FollowUpMethodId { get; set; }
        public int FollowUpPriortyId { get; set; }
        public int? FollowUpProccessId { get; set; }
        public int FollowUpSourceId { get; set; }
        public int? FollowUpProgressId { get; set; }
        public bool IsCopy { get; set; }
        public bool IsReminder { get; set; }
        public bool IsEscalated { get; set; }

        public bool IsImportant { get; set; }
        public bool HasChild { get; set; }
        public int? ParentId { get; set; }
        public List<TransactionFollowUpVM> FollowUps { get; set; } = (AjaxGrid<TransactionFollowUpVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionFollowUpVM>(), 1, 0, false);
    }
}