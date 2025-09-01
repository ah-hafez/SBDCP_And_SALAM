using System;

namespace MCS.UI
{
    public interface ISearchResult
    {
        int DocId { get; set; }
        string Type { get; set; }
        long Number { get; set; }
        string Subject { get; set; }
        string DateH { get; set; }
        DateTime Date { get; set; }
        string ConfidentialityName { get; set; }
        string PriorityName { get; set; }
        string PartyName { get; set; }
        string OrgUnitName { get; set; }
        string SignedByUserName { get; set; }
        string StatusName { get; set; }
        bool WithArchiving { get; set; }
        int ColorCode { get; set; }
        string TransactionTypeName { get; set; }
        int TransactionCategoryId { get; set; }
    }
}
