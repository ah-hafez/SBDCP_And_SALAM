using System;

namespace MCS.UI.Areas.User.Models.Report
{
    public class TransactionDeliveryReportVM
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Number { get; set; }
        public string Confidentiality { get; set; }
        public string Priority { get; set; }
        public string TransactionTypeName { get; set; }
        public string DeliveryMethod { get; set; }
        public string ToEntity { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public int TransactionId { get; set; }
        public bool PrintedDeliveryReport { get; set; }
        public string Subject { get; set; }
        public string TransactionCategoryName { get; set; }
        public string TransactionNumber { get; set; }
        public int TotalCount { get; set; }
        public bool? IsPrint { get; set; }
        public int TransactionCategoryId { get; set; }
        public bool IsForIndividual { get; set; }
        public string ExternalPartyName { get; set; }
        public int ExternalPartyId { get; set; }
        public bool IsCopy { get; set; }
        public int ToEntityId { get; set; }
        public int InternalPartyId { get; set; }
        public string InternalPartyName { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public AdditionalFieldsInboundVM AdditionalFieldsInboundVM { get; set; } = new AdditionalFieldsInboundVM();
        public AdditionalFieldsOutboundVM AdditionalFieldsOutboundVM { get; set; } = new AdditionalFieldsOutboundVM();

    }
}