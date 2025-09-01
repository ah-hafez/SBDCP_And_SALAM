using System;
using System.Collections.Generic;

namespace MCS.Domain
{
    public class TransactionCertificateInfo
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public long Number { get; set; }
        public string DocumentNumber { get; set; }
        public string Subject { get; set; }
        public string Manager { get; set; }
        public string Status { get; set; }
        public string UserCreatedBy { get; set; }
        public string OrgUnitCreatedBy { get; set; }
        public IList<TransactionAssignmentHistory> AssignmentsHistory { get; set; }
        public IList<TransactionCopy> Copies { get; set; }
        public IList<TransactionExternalCopy> ExternalCopies { get; set; }
        public IList<TransactionName> Names { get; set; }
        public IList<TransactionLink> Links { get; set; }
        public IList<Attachment> Attachments { get; set; }
        public IList<Explanation> Explanations { get; set; }

        public IList<TransactionLogInfo> TransactionLog { get; set; }
        public TransactionAssignment CurrentAssignment { get; set; }
        public bool IsMultiOwnership { get; set; }
        public string ToUser { get; set; }
        public string Priority { get; set; }
        public string Confidentiality { get; set; }
        public int ConfidentialityId { get; set; }
        public string TransactionType { get; set; }
        public string LetterType { get; set; }
        public string ExternalParty { get; set; }
        public string ExternalPartyManager { get; set; }
        public DocumentInfo MainDocument { get; set; }
        public string RemindDateH { get; set; }
        public string RemindTime { get; set; }
        public string InboundIntendedPerson { get; set; }
        public bool IsForIndividual { get; set; }
        public string DeliveryMethod { get; set; }
        public bool HasDate { get; set; }
        public string Remarks { get; set; }
        public string SignedBy { get; set; }
        public string ToEntity { get; set; }
        public int ProcessPeriodTransaction { get; set; }
        public string SideContactExternalEntityName { get; set; }
        public string NumberContact { get; set; }
        public int? RecordNumber { get; set; }
        public string LetterNumber { get; set; }
        public bool Encrypted { get; set; }
        public string ClassificationName { get; set; }
        public string FileDescription { get; set; }

        public int FileNumber { get; set; }
    }
}
