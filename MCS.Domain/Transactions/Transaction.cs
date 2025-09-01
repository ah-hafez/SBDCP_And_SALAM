using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using MCS.Common;
using Audit.EntityFramework;


namespace MCS.Domain
{
    [AuditInclude]
    public class Transaction : EntityBase, IAuditable
    {
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public long Number { get; set; }
        public int Year { get; set; }
        public int YearH { get; set; }
        public string DocumentNumber { get; set; }
        public string Remarks { get; set; }
        public string Subject { get; set; }
        public bool PrintedDeliveryReport { get; set; }
        public string DeliveryReportNumber { get; set; }
        public int? SignedByUserId { get; set; }
        public virtual UserProfile SignedByUser { get; set; }
        public int StatusId { get; set; }
        public virtual Lookup Status { get; set; }
        public string RejectionReason { get; set; }
        public int TransactionCategoryId { get; set; }
        public virtual Lookup TransactionCategory { get; set; }
        public int UserId { get; set; }
        public virtual UserProfile User { get; set; }
        public int OrgUnitId { get; set; }
        public virtual OrgUnit OrgUnit { get; set; }
        public virtual IList<TransactionAssignment> Assignments { get; set; }
        public virtual IList<TransactionCopy> Copies { get; set; }
        public virtual IList<TransactionExternalCopy> ExternalCopies { get; set; }
        public virtual IList<TransactionName> Names { get; set; }
        public virtual IList<TransactionLink> Links { get; set; }
        public virtual IList<Explanation> Explanations { get; set; }
        public virtual IList<Attachment> Attachments { get; set; }
        public virtual IList<TransactionFollowUp> FollowUp { get; set; }
        public virtual IList<TransactionSubjectClassification> SubjectClassifications { get; set; }
        public int? SuggestedTopicId { get; set; }
        public virtual SuggestedTopic SuggestedTopic { get; set; }
        public int? EntityId { get; set; }
        public virtual OrgUnit Entity { get; set; }
        public int? ToUserId { get; set; }
        public virtual UserProfile ToUser { get; set; }
        public int PriorityId { get; set; }
        public virtual Priority Priority { get; set; }
        public int ConfidentialityId { get; set; }
        public virtual Permission Confidentiality { get; set; }
        public int? SourceTypeId { get; set; }
        public int? TransactionTypeId { get; set; }
        public virtual TransactionType TransactionType { get; set; }
        public int? LetterTypeId { get; set; }
        public virtual LetterType LetterType { get; set; }
        public int? ExternalPartyId { get; set; }
        public virtual ExternalParty ExternalParty { get; set; }
        public int? ExternalPartyManagerId { get; set; }
        public virtual ExternalPartyManager ExternalPartyManager { get; set; }
        public int? MainDocumentId { get; set; }
        public virtual DocumentInfo MainDocument { get; set; }
        public DateTime? RemindDate { get; set; }
        public string RemindDateH { get; set; }
        public int? OutboundDraftId { get; set; }
        public int? OutboundDraftEditorType { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSigned { get; set; }
        public int? GroupId { get; set; }
        public int DeliveryMethodId { get; set; }
        public virtual Lookup DeliveryMethod { get; set; }
        public string InboundDateH { get; set; }
        public string PostCode { get; set; }
        public string POBox { get; set; }
        public bool IsDraft { get; set; }
        public bool IsForIndividual { get; set; }
        public string SavedReason { get; set; }
        public bool HasPermission { get; set; }
        public string DeliveryNumber { get; set; }
        public int? ReporterId { get; set; }
        public string InboundIntendedPerson { get; set; }
        public int? ReservationId { get; set; }
        public virtual TransactionReservation Reservation { get; set; }
        public int? TransactionPathId { get; set; }
        public bool HasLinks { get; set; }
        public int? ProcessPeriodTransaction { get; set; }
        [NotMapped]
        public TransactionDateType SourceTray { get; set; }
        [NotMapped]
        public bool IsDelayed { get; set; }
        [NotMapped]
        public bool IsAppointment { get; set; }
        [NotMapped]
        public bool IsCopy { get; set; }

        [NotMapped]
        public bool IsImportant { get; set; }

        public int? SubjectClassificationsId { get; set; }

        public int? RecordNumber { get; set; }
        public string NumberContact { get; set; }
        public int? SideContactExternalEntityID { get; set; }
        [ForeignKey("SideContactExternalEntityID")]
        public virtual ExternalParty Sidecontactexternalentity { get; set; }
        public virtual List<SavedTransactionAssignment> SavedTransactionAssignments { get; set; }
        public string ContactDateH { get; set; }
        public string ComplaintNumber { get; set; }
        public int? PrivecyId { get; set; }
        public virtual SpecificLevel Privecy { get; set; }
        public string LetterNumber { get; set; }
        public string Summary { get; set; }
        public bool IsPresentationDraft { get; set; }
        public long? PresentationDraftNumber { get; set; }
        public long? OutBoundDraftNumber { get; set; }
        public bool IsElcOutBound { get; set; }
        public bool NeedAcknowled { get; set; }
        public int? OldWordDocumntId { get; set; }
        public virtual DocumentInfo OldWordDocumnt { get; set; }
        public bool IsDecisionDraft { get; set; }
        [NotMapped]
        public virtual Name Name { get; set; }
        public int? CityId { get; set; }
        public virtual City City { get; set; }

        [NotMapped]
        public bool IsMultiExternal { get; set; }
        public virtual IList<TransactionSpecialAuthorize> SpecialAuthorizations { get; set; }
        public bool Encrypted { get; set; }
        public virtual IList<IC_SUBJECTS_TRANSACTION> SubjectTransactions { get; set; }



    }
}
