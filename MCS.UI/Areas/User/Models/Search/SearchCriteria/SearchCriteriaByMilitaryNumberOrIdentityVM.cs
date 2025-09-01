using MCS.Common.CustomAttributes;
using System;

namespace MCS.UI.Areas.User.Models.Search
{
    public class SearchCriteriaByMilitaryNumberOrIdentityVM
    {
        public SearchCriteriaByMilitaryNumberOrIdentityVM()
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
        [CustomDisplayName("User.MilitaryNumberOrIdentitySearch.IdentificationNumber")]
        [CustomRequired("User.Transaction.Name.CivilIDRequired")]
        [CustomStringLength("User.Transaction.Name.CivilIDLength", 10, 10)]
        public string IdentificationNumber { get; set; } 
        public int TransactionTypeId { get; set; }
        public InboundAdvancedVM InboundAdvanced { get; set; } 
        public OutboundAdvancedVM OutboundAdvanced { get; set; }
        public int TransactionCategory { get; set; }
    }
}