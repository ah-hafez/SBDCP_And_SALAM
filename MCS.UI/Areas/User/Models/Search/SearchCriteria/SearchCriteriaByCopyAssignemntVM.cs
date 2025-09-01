using System;
using MCS.Common.CustomAttributes;
namespace MCS.UI.Areas.User.Models.Search
{
    public class SearchCriteriaByCopyAssignemntVM
    {
        public SearchCriteriaByCopyAssignemntVM()
        {
            AdvancedSearch = new InboundAdvancedVM(); 
        }

        [CustomDisplayName("User.Search.FromEntity")]
        [CustomRequired("User.Search.FromEntityRequired")]
        public int FromEntityId { get; set; }
        [CustomDisplayName("User.Search.ToEntity")]
        [CustomRequired("User.Search.ToEntityRequired")]
        public int ToEntityId { get; set; }
         
        [CustomDateTimeCompareAttribute("DateTo", Operation.LessThanOrEqual, "User.InboundSearch.DateCompare")]
        public DateTime? DateFrom { get; set; }

        [CustomDateTimeCompareAttribute("DateFrom", Operation.GreaterThanOrEqual)]
        public DateTime? DateTo { get; set; }
        public int? Number { get; set; }//رقم القيد       
        public int? HourFrom { get; set; }
        public int? HourTo { get; set; }
        public int? MinuteFrom { get; set; }
        public int? MinuteTo { get; set; }

        [CustomTimeSpanCompareAttribute("TimeTo", Operation.LessThanOrEqual, "User.InboundSearch.TimeCompare")]
        public TimeSpan? TimeFrom { get; set; }

        [CustomTimeSpanCompareAttribute("TimeFrom", Operation.GreaterThanOrEqual)]
        public TimeSpan? TimeTo { get; set; }

        public InboundAdvancedVM AdvancedSearch { get; set; }

    }
}