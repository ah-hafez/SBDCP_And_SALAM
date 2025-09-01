using System;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class TransactionIndexLog : EntityBase
    {
        public int TransId { get; set; }
        public int TransactionCategoryId { get; set; }
        public int TransactionTypeId { get; set; }
        public long Number { get; set; }
        public string Barcode { get; set; }
        public string DateH { get; set; }
        public DateTime Date { get; set; }
        public int Year { get; set; }
        public int YearH { get; set; }
        public string PermissionCode { get; set; }
        public int PriorityId { get; set; }
        public int? PartyId { get; set; }
        public int OrgUnitId { get; set; }
        public int SignedByUserId { get; set; }
        public int? DirectedToUserId { get; set; }
        public int StatusId { get; set; }
        public int LetterTypeId { get; set; }
        public string OrgUnitNameAr { get; set; }
        public string OrgUnitNameEn { get; set; }
        public string TypeNameAr { get; set; }
        public string TypeNameEn { get; set; }
        public string PartyNameAr { get; set; }
        public string PartyNameEn { get; set; }
        public string SignedByNameAr { get; set; }
        public string SignedByNameEn { get; set; }
        public string ConfidentialityNameAr { get; set; }
        public string ConfidentialityNameEn { get; set; }
        public string PriorityNameAr { get; set; }
        public string PriorityNameEn { get; set; }
        public string StatusNameAr { get; set; }
        public string StatusNameEn { get; set; }
        public string TransactionTypeNameAr { get; set; }
        public string TransactionTypeNameEn { get; set; }
        public string Subject { get; set; }
        public string Assignments { get; set; }
        public bool IsIndexed { get; set; }
        public bool IsUpdated { get; set; }
        public bool WithArchiving { get; set; }
        public string Color { get; set; }
        public string SubjectClassifications { get; set; }
    }
}
