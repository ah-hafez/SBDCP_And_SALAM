using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Report
{
    public class TransactionReportVM
    {
        #region Basic Search
        [CustomDisplayName("User.InboundSearch.InboundNumber")]
        //[CustomStringLength("User.InboundSearch.InboundNumber", 20, 0)]
        public int? Number { get; set; }

        [CustomDisplayName("User.SubjectSearch.Subject")]
        public string Subject { get; set; }

        [CustomRequired("User.Transaction.Link.TransactionTypeRequired")]
        public int TransactionCategory { get; set; }

        [CustomRequired("User.DateRequired")]
        [CustomDateTimeCompareAttribute("To", Operation.LessThanOrEqual, "User.InboundSearch.DateCompare")]
        public DateTime From { get; set; }


        [CustomRequired("User.DateRequired")]
        [CustomDateTimeCompareAttribute("From", Operation.GreaterThanOrEqual)]
        public DateTime To { get; set; }
        public List<int> ColumnsToGrid { get; set; }
        public int TotalCount { get; set; }
        public bool? IsPrint { get; set; }
        [CustomRequired("User.Reports.TransactionReportTypeRequired")]
        public int TransactionReportType { get; set; }

        #endregion

        #region Names
        public NamesVM NamesVM { get; set; } = new NamesVM();
        #endregion

        #region Common
        public CommonVM CommonVM { get; set; } = new CommonVM();
        #endregion

        #region Assignment
        public SearchAssignmentVM SearchAssignmentVM { get; set; } = new SearchAssignmentVM();
        #endregion

        #region Additional Fields Inbound
        public AdditionalFieldsInboundVM AdditionalFieldsInboundVM { get; set; } = new AdditionalFieldsInboundVM();
        #endregion

        #region Additional Fields Outbound
        public AdditionalFieldsOutboundVM AdditionalFieldsOutboundVM { get; set; } = new AdditionalFieldsOutboundVM();
        #endregion
        public string ummalqura { get; set; }
        public string gregorian { get; set; }
        [CustomDisplayName("User.Copy.OrgUnitName")]
        public int OrgUnitId { get; set; }
    }
}