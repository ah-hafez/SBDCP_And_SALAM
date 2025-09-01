using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Common;

namespace MCS.DTO.HubTransaction
{
    public class HubTransactionDTO
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
        public DocumentInfoDTO MainDocument { get; set; }
        public  List<HubAttachmentDTO> HubAttachments { get; set; }
        public  List<HubRelatedPersonDTO> HubRelatedPersons { get; set; }
        public Guid RQUID { get; set; }
        public string PriorityText { get; set; }
        public string ConfidentialityName { get; set; }
        public string ExternalPartyName { get; set; }
        public string TransactionTypeName { get; set; }
        public string TransactionCategory { get; set; }
        public HubTransactionStatus Status { get; set; }
        public DateTime? ReminderGDate { get; set; }
        public string ReminderHDate { get; set; }

        public string TransactionType { get; set; }
        public HubDeliveryType DeliveryType { get; set; }
        public string DeliveryTypeName { get; set; }

        public string SourceTypeName { get; set; }


    }
}
