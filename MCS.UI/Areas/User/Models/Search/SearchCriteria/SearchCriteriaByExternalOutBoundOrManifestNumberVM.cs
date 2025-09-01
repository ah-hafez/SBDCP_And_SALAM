using System;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Search
{
    public class SearchCriteriaByExternalOutBoundOrManifestNumberVM
    {
        public SearchCriteriaByExternalOutBoundOrManifestNumberVM()
        {
            AdvancedSearch = new OutboundAdvancedVM();
        }

        [CustomDisplayName("User.OutboundSearch.OutboundNumber")]
        [CustomStringLength("User.OutboundSearch.OutboundNumber", 20, 0)]
        public int? Number { get; set; }

        [CustomDisplayName("User.OutboundSearch.Year")]
        public int? Year { get; set; }

        [CustomRequired("User.Inbound.BasicInfo.InboundTypeRequired")]
        [CustomDisplayName("User.Inbound.BasicInfo.InboundType")]
        public int TransactionTypeId { get; set; }

        public OutboundAdvancedVM AdvancedSearch { get; set; }

        [CustomDateTimeCompareAttribute("DateTo", Operation.LessThanOrEqual)]
        public DateTime? DateFrom { get; set; }

        [CustomDateTimeCompareAttribute("DateFrom", Operation.GreaterThanOrEqual)]
        public DateTime? DateTo { get; set; }

        public int? HourFrom { get; set; }
        public int? HourTo { get; set; }
        public int? MinuteFrom { get; set; }
        public int? MinuteTo { get; set; }

        [CustomTimeSpanCompareAttribute("TimeTo", Operation.LessThanOrEqual, "User.OutboundSearch.TimeCompare")]
        public TimeSpan? TimeFrom { get; set; }

        [CustomTimeSpanCompareAttribute("TimeFrom", Operation.GreaterThanOrEqual)]
        public TimeSpan? TimeTo { get; set; }
    }
}