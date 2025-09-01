using System;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Search
{
    public class SearchCriteriaByOutboundInternalVM
    {
        public SearchCriteriaByOutboundInternalVM()
        {
            AdvancedSearch = new OutboundInternalAdvancedVM();
        }

        [CustomDisplayName("User.InboundSearch.InboundNumber")]
        [CustomStringLength("User.InboundSearch.InboundNumber", 20, 0)]
        public int? Number { get; set; }//رقم القيد

        [CustomDisplayName("User.InboundSearch.Year")]
        public int? Year { get; set; }//السنة
        [CustomRequired("User.Inbound.BasicInfo.InboundTypeRequired")]
        [CustomDisplayName("User.Inbound.BasicInfo.InboundType")]
        public int TransactionTypeId { get; set; }

        public OutboundInternalAdvancedVM AdvancedSearch { get; set; }

        [CustomDateTimeCompareAttribute("DateTo", Operation.LessThanOrEqual, "User.InboundSearch.DateCompare")]
        public DateTime? DateFrom { get; set; }

        [CustomDateTimeCompareAttribute("DateFrom", Operation.GreaterThanOrEqual)]
        public DateTime? DateTo { get; set; }

        public int? HourFrom { get; set; }
        public int? HourTo { get; set; }
        public int? MinuteFrom { get; set; }
        public int? MinuteTo { get; set; }

        [CustomTimeSpanCompareAttribute("TimeTo", Operation.LessThanOrEqual, "User.InboundSearch.TimeCompare")]
        public TimeSpan? TimeFrom { get; set; }

        [CustomTimeSpanCompareAttribute("TimeFrom", Operation.GreaterThanOrEqual)]
        public TimeSpan? TimeTo { get; set; }
    }
}