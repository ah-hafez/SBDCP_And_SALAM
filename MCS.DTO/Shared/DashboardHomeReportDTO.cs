using System.Collections.Generic;

namespace MCS.DTO.Shared
{
    public class DashboardHomeReportDTO
    {
        public int TotalTransactions { set; get; }
        public int TotalAssignments { set; get; }
        public int TotalInbound { set; get; }
        public int TotalOutbound { set; get; }
        public int TotalInternal { set; get; }
        public decimal LateAVG { set; get; }
        public decimal TotalCompleted { set; get; }

        public List<DashboardReportBottomDTO> DashboardReportBottomList { set; get; }


    }
}
