using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Common;

namespace MCS.UI.Areas.User.Models.Hub
{
    public class HubTransactionVM
    {
        public int Id { get; set; }
        public string TransactionNumber { get; set; }
        public int OrgUnitId { get; set; }
        public int PriorityLevelId { get; set; }
        public int ConfidentialityLevelId { get; set; }
        public int DestinationId { get; set; }
        public DateTime RecordDate { get; set; }
        public string HijriRecordDate { get; set; }
        public string Remarks { get; set; }
        public string Subject { get; set; }
        public DocumentInfoVM MainDocument { get; set; }
        public List<HubAttachmentVM> HubAttachments { get; set; }
        public List<HubRelatedPersonVM> HubRelatedPersons { get; set; }
        public Guid RQUID { get; set; }
        public string PriorityText { get; set; }
        public string ConfidentialityName { get; set; }
        public string ExternalPartyName { get; set; }
        public string TransactionTypeName { get; set; }
        public string TransactionCategory { get; set; }
        public HubTransactionStatus Status { get; set; }
        public DateTime? ReminderGDate { get; internal set; }
        public string ReminderHDate { get; internal set; }

        public string TransactionType { get; set; }

        public string SourceTypeName { get; set; }

        public HubDeliveryType DeliveryType { get; set; }
        public string DeliveryTypeName { get; set; }


    }
}