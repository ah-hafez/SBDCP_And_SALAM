using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Common.CustomAttributes;
using MCS.DTO.Shared;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Models.Shared
{
    public class DashboardHomeVM
    {
        public int OutboundCount { get; set; }
        public int OutboundDraftCountCreated { get; set; }
        public int OutboundDraftCountAssigned { get; set; }
        public int InboundCountCreated { get; set; }
        public int InboundCountAssigned { get; set; }
        public int InternalOutboundCountCreated { get; set; }
        public int InternalOutboundCountAssigned { get; set; }
        public int DelayedCount { get; set; }

        [CustomDateTimeCompareAttribute("ToDate", Operation.LessThanOrEqual, "User.InboundSearch.DateCompare")]
        public DateTime? FromDate { get; set; }
        public string FromDateH { get; set; }


        [CustomDateTimeCompareAttribute("FromDate", Operation.GreaterThanOrEqual)]
        public DateTime? ToDate { get; set; }
        public string ToDateH { get; set; }

        [CustomDisplayName("User.Copy.OrgUnitName")]
        public string OrgUnitName { get; set; }
        [CustomDisplayName("User.Copy.OrgUnitName")]
        [CustomRequired("User.UserDelegation.OrgUnitRequired")]
        public int OrgUnitId { get; set; }
        [CustomDisplayName("Admin.Lookups.PriorityExceptions.OrgUnit")]
        public int DirectedToOrgUnitId { set; get; }
        public int? DirectedToId { get; set; }    //موجهة إلى//
        public string DateFormateSetting { get; set; }
        public string userName { get; set; }

        public int TotalTransactions { set; get; }
        public int TotalAssignments { set; get; }
        public int TotalInbound { set; get; }
        public int TotalOutbound { set; get; }
        public int TotalInternal { set; get; }
        public decimal LateAVG { set; get; }
        public decimal TotalCompleted { set; get; }

        public List<DashboardReportBottomDTO> DashboardReportBottomList { set; get; }
        public List<TransactionTypesReport> transactionTypesReport { set; get; }
        public List<TransactionConfidentialityReport> transactionConfidentialityReports { set; get; }

    }
}