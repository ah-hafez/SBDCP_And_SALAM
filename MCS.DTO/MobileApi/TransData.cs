using MCS.DTO;
using MCS.DTO.MobileApi;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using System;
using System.Collections.Generic;

namespace MobileApi.Domain
{
    public class TransData
    {
        public int TransId { get; set; }

        public string TransNo { get; set; }

        public int TransCategory { get; set; }

        public int TransSource { get; set; }

        public string TransSourceDesc { get; set; }

        public int ConfidId { get; set; }

        public string ConfidDesc { get; set; }

        public int PriorityId { get; set; }

        public string PriorityDesc { get; set; }

        public int TypeId { get; set; }

        public string TypeDesc { get; set; }

        public DateTime? PriorityDate { get; set; }

        public string PriorityDateHJ { get; set; }

        public string FormattedPriorityDate { get; set; }

        public int Status { get; set; }

        public string StatusDesc { get; set; }

        public int Year { get; set; }

        public string Remarks { get; set; }

        public bool IsInternalOutbound { get; set; }

        public bool OutboundDraft { get; set; }

        public string Subject { get; set; }

        public DateTime TransDate { get; set; }

        public string TransDateHJ { get; set; }

        public string FormattedTransDate { get; set; }

        public int InitialAssignToPersonId { get; set; }

        public string InitialAssignToPersonName { get; set; }

        public int MainParty { get; set; }
        public int? ExternalPartyId { get; set; }

        public string MainPartyDesc { get; set; }

        public int ConcernedEntityId { get; set; }

        public string ConcernedEntityDesc { get; set; }

        public int UserId { get; set; }

        public string CreatorUserName { get; set; }

        public int EntityId { get; set; }

        public string CreatingEntityName { get; set; }

        public string BarcodeRand { get; set; }

        public string ExtTransNo { get; set; }

        public string ProcessFinishDateHJ { get; set; }

        public DateTime ProcessFinishDate { get; set; }

        public TransAssign AssignEntity { get; set; }

        public TransAssignTrack AssignTrack { get; set; }

        public List<TransPartiy> TransCopies { get; set; }
        public List<TransLink> TransLinks { get; set; }

        public List<IncludedItem> IncludedItems { get; set; }

        public List<ArchiveRecord> archiveRecords { get; set; }
        public virtual IList<TransactionName> Names { get; set; }

        public List<PredefinedAssignee> predefinedAssignees { get; set; }
        public bool IsInternalParty { get { return false; } }
        public bool IsEditable { get; set; }
        public bool IsAssign { get; set; }
        public int TrayId { get; set; }

        public int? StatusLevel { get; set; }
        public bool IsDecisionNumber { get; set; } = false;
        public long? DecisionNumber { get; set; }
        public byte[] Barcode { get; set; }
        public string CivilID { get; set; }
        public string ErpReferenceNumber { get; set; }
        public TransactionBarcodesDTO BarcodeData { get; set; }
        public string TransactionCode { get; set; }
        public string CopiesBranch { get; set; }
        public bool IsSigned { get; set; }




    }
    public class TransactionName : EntityBase, IAuditable
    {
        public int TransactionId { get; set; }
        public int NameId { get; set; }
        public virtual Name Name { get; set; }
    }
    public class Name : EntityBase, IAuditable
    {
        public string CivilID { get; set; }
        public int? NationalityId { get; set; }
        public virtual Lookup Nationality { get; set; }
        public string FirstName { get; set; }
        //public string SecondName { get; set; }
        //public string ThirdName { get; set; }
        //public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        //public decimal? DueAmount { get; set; }
        //public string POBox { get; set; }
        //public string Fax { get; set; }
        public string OtherInformation { get; set; } // معلومات أخرى //
                                                     //public string FourthName { get; set; }   //الاسم الرابع//
        public int? TitleId { get; set; }//اللقب//
        public virtual Lookup Title { get; set; }
        public string RelativeRelation { get; set; }
        //public string PostCode { get; set; }
        public string City { get; set; }
        public int Gender { get; set; }
    }
}