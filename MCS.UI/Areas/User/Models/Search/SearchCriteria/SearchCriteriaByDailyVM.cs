using MCS.Common.CustomAttributes;
using System;

namespace MCS.UI.Areas.User.Models.Search
{
    public class SearchCriteriaByDailyVM
    {
        public SearchCriteriaByDailyVM()
        {
            InboundAdvanced = new InboundAdvancedVM();
            OutboundAdvanced = new OutboundAdvancedVM();
        }
        public DateTime? TodayDate { get; set; }

        public InboundAdvancedVM InboundAdvanced { get; set; } 
        public OutboundAdvancedVM OutboundAdvanced { get; set; }
    }
}