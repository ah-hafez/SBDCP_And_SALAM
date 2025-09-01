
using System;
using Audit.EntityFramework;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    [AuditIgnore]
    public class TransactionHistory : EntityBase 
    {
        public int UserId { get; set; }
        public virtual UserProfile User { get; set; }
        public int? SignedByUserId { get; set; }
        public virtual UserProfile SignedByUser { get; set; }
        public int? SignedByOrgUnitId { get; set; }
        public virtual OrgUnit SignedByOrgUnit { get; set; }
        public int StatusId { get; set; }
        public virtual Lookup Status { get; set; }
        public int? DestinationId { get; set; }
        public virtual OrgUnit Destination { get; set; }
        public int? ExplanationId { get; set; }
        public virtual Lookup Explanation { get; set; }
        public int DeliveryMethodId { get; set; }
        public virtual Lookup DeliveryMethod { get; set; }
        public int PriorityId { get; set; }
        public virtual Priority Priority { get; set; }
        public int ConfidentialityId { get; set; }
        public virtual Permission Confidentiality { get; set; }
        //public virtual string DirectedTo { get; set; }
        public string Remarks { get; set; }
        public string Subject { get; set; }
        public int TransactionCategoryId { get; set; }
        public virtual Lookup TransactionCategory { get; set; }
        public int? TransactionTypeId { get; set; }
        public virtual TransactionType TransactionType { get; set; }
        public int? LetterTypeId { get; set; }
        public virtual LetterType LetterType { get; set; }
        public int? ExternalPartyId { get; set; }
        public virtual ExternalParty ExternalParty { get; set; }
        public int? ExternalPartyManagerId { get; set; }
        public virtual ExternalPartyManager ExternalPartyManager { get; set; }
        public int TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }
        public bool PrintedDeliveryReport { get; set; }
        public string  DeliveryReportNumber { get; set; }
        public int AttchmentCount { get; set; }
        public int? ToEntityId { get; set; }
        public virtual OrgUnit ToEntity { get; set; }
        public int? ToUserId { get; set; }
        public virtual UserProfile ToUser { get; set; }
        //public int? FromEntityId { get; set; }
        //public virtual OrgUnit FromEntity { get; set; }
        //public int? FromUserId { get; set; }
        //public virtual UserProfile FromUser { get; set; }
        public DateTime? RemindDate { get; set; }
        public string RemindDateH { get; set; }
        public int? OutboundDraftId { get; set; }
        public string LetterNumber { get; set; }
    }
}
