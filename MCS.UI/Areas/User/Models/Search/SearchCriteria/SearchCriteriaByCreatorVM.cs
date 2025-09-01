using System;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Search
{
    public class SearchCriteriaByCreatorVM
    {
        public SearchCriteriaByCreatorVM()
        {
            InboundAdvanced = new InboundAdvancedVM();
            OutboundAdvanced = new OutboundAdvancedVM();
        }
        public int? Number { get; set; }//رقم القيد
        //[CustomDisplayName("User.Inbound.BasicInfo.DirectedTo")]
        //[CustomRequired("User.Inbound.BasicInfo.DirectedToRequired")]
        [CustomDisplayName("User.Search.BasicInfo.DirectedTo")]
        [CustomRequired("User.Search.BasicInfo.DirectedToRequired")]
        public int UserId { get; set; }

        [CustomDisplayName("User.Transaction.Link.TransactionType")]
        [CustomRequired("User.Transaction.Link.TransactionTypeRequired")]
        public int TransactionCategory { get; set; }

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
        public InboundAdvancedVM InboundAdvanced { get; set; }

        public OutboundAdvancedVM OutboundAdvanced { get; set; }
    }
}