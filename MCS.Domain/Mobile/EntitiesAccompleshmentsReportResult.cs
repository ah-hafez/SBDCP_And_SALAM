using System;

namespace MCS.Domain
{
    public class EntitiesAccompleshmentsReportResult
    {
        public int TRANSACTIONS { get; set; }
        public int DELAYED { get; set; }
        public int WITH_APPOITMENT { get; set; }
        public int TRANS_PARTIES { get; set; }
        public DateTime FROM_DATE { get; set; }
        public DateTime TO_DATE { get; set; }
    }
}
