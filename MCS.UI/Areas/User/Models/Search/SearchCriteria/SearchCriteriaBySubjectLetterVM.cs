using MCS.Common.CustomAttributes;
using System;

namespace MCS.UI.Areas.User.Models.Search
{
    public class SearchCriteriaBySubjectLetterVM
    {
        public SearchCriteriaBySubjectLetterVM()
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
        [CustomDisplayName("User.SubjectLetter.FirstLetter")]
        [CustomRequired("User.SubjectSearch.FirstLetterTypeRequired")]
        public string FirstLetter { get; set; } 
        [CustomDisplayName("User.SubjectLetter.SecondLetter")] 
        public string SecondLetter { get; set; } 
     
        [CustomDisplayName("User.SubjectLetter.ThirdLetter")] 
        public string ThirdLetter { get; set; }
        [CustomDisplayName("User.SubjectLetter.FourthLetter")] 
        public string FourthLetter { get; set; }
        public int TransactionTypeId { get; set; }
        public int SearchTypeForFiltersId { get; set; }
        public int TransactionCategory { get; set; }
        public InboundAdvancedVM InboundAdvanced { get; set; } 
        public OutboundAdvancedVM OutboundAdvanced { get; set; }
    }
}