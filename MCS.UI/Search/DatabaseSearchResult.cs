using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI
{
    public class DatabaseSearchResult : ISearchResult
    {
        public int DocId { get; set;}        
        public string Type {get; set;}
        public long Number { get; set; }
        public string Barcode { get; set; }        
        public string Subject {get; set;}      
        public string DateH {get; set;}
        public DateTime Date {get; set;}
        public string ConfidentialityName { get; set; }
        public string PriorityName {get; set;}     
        public string PartyName {get; set;}
        public string OrgUnitName {get; set;}     
        public string SignedByUserName {get; set;}
        public string StatusName {get; set;}
        public bool WithArchiving { get; set; }
        public int ColorCode { get; set; }    
        public string TransactionTypeName { get; set; }
        public int TransactionCategoryId { get; set; }
    }
}