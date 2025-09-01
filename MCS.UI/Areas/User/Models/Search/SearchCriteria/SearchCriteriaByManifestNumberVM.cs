using MCS.Common.CustomAttributes;
using System;

namespace MCS.UI.Areas.User.Models.Search
{
    public class SearchCriteriaByManifestNumberVM
    {
        public SearchCriteriaByManifestNumberVM()
        {
            AdvancedSearch = new InboundAdvancedVM(); 
        }
         
        [CustomDateTimeCompareAttribute("DateTo", Operation.LessThanOrEqual, "User.InboundSearch.DateCompare")]
        public DateTime? DateFrom { get; set; } 
        [CustomDateTimeCompareAttribute("DateFrom", Operation.GreaterThanOrEqual)]
        public DateTime? DateTo { get; set; } 
        public int? HourFrom { get; set; }
        public int? HourTo { get; set; }
        public int? MinuteFrom { get; set; }
        public int? MinuteTo { get; set; } 
        [CustomDisplayName("User.ManifestNumber.ManifestNumber")]
        [CustomRequired("User.ManifestNumber.ManifestNumberRequired")]
        public int ManifestNumber { get; set; } 
        public InboundAdvancedVM AdvancedSearch { get; set; }  
    }
}