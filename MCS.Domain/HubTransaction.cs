using System;
using System.Collections.Generic;
using MCS.Framework.Entities;
using MCS.Common;

namespace MCS.Domain
{
    public class HubTransaction : EntityBase
    {
        public string TransactionNumber { get; set; }
        public int OrgUnitId { get; set; }
        public int PriorityLevelId { get; set; }
        public int ConfidentialityLevelId { get; set; }
        public int DestinationId { get; set; }
        public DateTime RecordDate { get; set; }
        public string HijriRecordDate { get; set; }
        public string Remarks { get; set; }
        public virtual DocumentInfo MainDocument { get; set; }
        public Guid RQUID { get; set; }
        public string Subject { get; set; }
        public virtual List<HubAttachment> HubAttachments { get; set; }
        public virtual List<HubRelatedPerson> HubRelatedPersons { get; set; }
        public DateTime? ReminderGDate { get; set; }
        public string ReminderHDate { get; set; }
        public HubTransactionStatus Status { get; set; }
        public OutboundClassification Classification { get; set; }
        public bool IsDeleted { get; set; }
        public long? NewTransactionId { get; set; }
        public DateTime? NewTransactionTimestamp { get; set; }
        public HubDeliveryType DeliveryType { get; set; }
    }
}
