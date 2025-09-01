using MCS.Common.CustomAttributes;
using System;

namespace MCS.UI.Areas.User.Models.Search
{
    public class SearchCriteriaByNamesVM
    {
        public SearchCriteriaByNamesVM()
        {
            InboundAdvanced = new InboundAdvancedVM();
            OutboundAdvanced = new OutboundAdvancedVM();
        }
         
        [CustomDateTimeCompareAttribute("DateTo", Operation.LessThanOrEqual, "User.InboundSearch.DateCompare")]
        public DateTime? DateFrom { get; set; } 
        [CustomDateTimeCompareAttribute("DateFrom", Operation.GreaterThanOrEqual)]
        public DateTime? DateTo { get; set; } 
        public int? HourFrom { get; set; }
        public int? HourTo { get; set; }
        public int? MinuteFrom { get; set; }
        public int? MinuteTo { get; set; } 
        [CustomDisplayName("User.NamesSearch.FirstName")]
        [CustomRequired("User.NamesSearch.FirstNameRequired")]
        public string FirstName { get; set; } 
        [CustomDisplayName("User.NamesSearch.SecondName")] 
        public string SecondName { get; set; } 
        public int SearchNamesType { get; set; } 
        [CustomDisplayName("User.NamesSearch.ThirdName")] 
        public string ThirdName{ get; set; }
        [CustomDisplayName("User.NamesSearch.FamilyName")] 
        public string FamilyName { get; set; }
        public int TransactionTypeId { get; set; } 
        public int TransactionCategory { get; set; }
        public InboundAdvancedVM InboundAdvanced { get; set; } 
        public OutboundAdvancedVM OutboundAdvanced { get; set; }
    }
}