using SolrNet.Attributes;
using System;
using System.Collections.Generic;

namespace MCS.Common
{
    public class TransactionInfo
    {
        [SolrUniqueKey(SearchFields.DocId)]
        public int DocId { get; set; }

        [SolrField(SearchFields.TransactionCategoryId)]
        public int TransactionCategoryId { get; set; }

        [SolrField(SearchFields.TransactionTypeId)]
        public int TransactionTypeId { get; set; }

        [SolrField(SearchFields.Number)]
        public long Number { get; set; }

        [SolrField(SearchFields.Barcode)]
        public string Barcode { get; set; }

        [SolrField(SearchFields.DateH)]
        public string DateH { get; set; }

        [SolrField(SearchFields.Date)]
        public DateTime Date { get; set; }

        [SolrField(SearchFields.Year)]
        public int Year { get; set; }

        [SolrField(SearchFields.YearH)]
        public int YearH { get; set; }

        [SolrField(SearchFields.PermissionCode)]
        public string PermissionCode { get; set; }

        [SolrField(SearchFields.PriorityId)]
        public int PriorityId { get; set; }

        [SolrField(SearchFields.PartyId)]
        public int? PartyId { get; set; }

        [SolrField(SearchFields.OrgUnitId)]
        public int OrgUnitId { get; set; }

        [SolrField(SearchFields.SignedByUserId)]
        public int SignedByUserId { get; set; }

        [SolrField(SearchFields.DirectedToUserId)]
        public int? DirectedToUserId { get; set; }

        [SolrField(SearchFields.StatusId)]
        public int StatusId { get; set; }

        [SolrField(SearchFields.LetterTypeId)]
        public int LetterTypeId { get; set; }

        [SolrField(SearchFields.OrgUnitNameAr)]
        public string OrgUnitNameAr { get; set; }

        [SolrField(SearchFields.OrgUnitNameEn)]
        public string OrgUnitNameEn { get; set; }

        [SolrField(SearchFields.TypeNameAr)]
        public string TypeNameAr { get; set; }

        [SolrField(SearchFields.TypeNameEn)]
        public string TypeNameEn { get; set; }

        [SolrField(SearchFields.PartyNameAr)]
        public string PartyNameAr { get; set; }

        [SolrField(SearchFields.PartyNameEn)]
        public string PartyNameEn { get; set; }

        [SolrField(SearchFields.SignedByNameAr)]
        public string SignedByNameAr { get; set; }

        [SolrField(SearchFields.SignedByNameEn)]
        public string SignedByNameEn { get; set; }

        [SolrField(SearchFields.ConfidentialityNameAr)]
        public string ConfidentialityNameAr { get; set; }

        [SolrField(SearchFields.ConfidentialityNameEn)]
        public string ConfidentialityNameEn { get; set; }

        [SolrField(SearchFields.PriorityNameAr)]
        public string PriorityNameAr { get; set; }

        [SolrField(SearchFields.PriorityNameEn)]
        public string PriorityNameEn { get; set; }

        [SolrField(SearchFields.StatusNameAr)]
        public string StatusNameAr { get; set; }

        [SolrField(SearchFields.StatusNameEn)]
        public string StatusNameEn { get; set; }

        [SolrField(SearchFields.TransactionTypeNameAr)]
        public string TransactionTypeNameAr { get; set; }

        [SolrField(SearchFields.TransactionTypeNameEn)]
        public string TransactionTypeNameEn { get; set; }

        [SolrField(SearchFields.Subject)]
        public string Subject { get; set; }

        [SolrField(SearchFields.Assignments)]
        public ICollection<string> Assignments { get; set; }

        [SolrField(SearchFields.WithArchiving)]
        public bool WithArchiving { get; set; }

        [SolrField(SearchFields.Color)]
        public string Color { get; set; }

        [SolrField(SearchFields.SubjectClassifications)]
        public ICollection<string> SubjectClassifications { get; set; }

        [SolrField(SearchFields.AllFields)]
        public ICollection<string> AllFields { get; set; }
        public string LetterNumber { get; set; }
    }
}
