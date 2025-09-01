using System;
using System.Collections.Generic;

namespace MobileApi.Domain
{

    public class EntityAccomplishmentReportInfoResult
    {
        public int TRANSACTIONS { get; set; }
        public int DELAYED { get; set; }
        public int DECISION { get; set; }
        public int WITH_APPOITMENT { get; set; }
        public int TRANS_PARTIES { get; set; }
        public DateTime FROM_DATE { get; set; }
        public DateTime TO_DATE { get; set; }
    }
    public class EntityAccomplishmentReportInfo
    {
        public DateTime PeriodFrom { get; set; }
        public DateTime PeriodTo { get; set; }
        public List<ReportItem> Counts { get; set; }
    }

    public class ReportItem
    {
        public string Text { get; set; }
        public int Value { get; set; }
        public int TrayId { get; set; }
    }
}