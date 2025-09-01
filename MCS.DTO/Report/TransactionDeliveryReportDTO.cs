using System;

namespace MCS.DTO
{
    public class TransactionDeliveryReportDTO
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Number { get; set; }
        public string Confidentiality { get; set; }
        public string Priority { get; set; }
        public string TransactionTypeName { get; set; }
        public string DeliveryMethod { get; set; }
        public string ToEntity { get; set; }
        public int ToEntityId { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public int TransactionId { get; set; }
        public bool PrintedDeliveryReport { get; set; }
        public string Subject { get; set; }
        public string TransactionCategoryName { get; set; }
        public string TransactionNumber { get; set; }
        public int TransactionCategoryId { get; set; }
        public bool IsForIndividual { get; set; }
        public string ExternalPartyName { get; set; }
        public int ExternalPartyId { get; set; }
        public DocumentDTO Document { get; set; }
        public bool IsCopy { get; set; }
        public string InternalPartyName { get; set; }
        public int InternalPartyId { get; set; }
    }
}
