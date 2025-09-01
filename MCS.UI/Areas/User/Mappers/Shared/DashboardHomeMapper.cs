using MCS.DTO;
using MCS.DTO.Shared;
using MCS.UI.Areas.User.Models.Shared;

namespace MCS.UI.Areas.User.Mappers.Shared
{
    public class DashboardHomeMapper
    {
        public static DashboardHomeVM Map(DashboardHomeDTO dashboardHomeDTO)
        {
            if (dashboardHomeDTO == null)
            {
                return null;
            }
            DashboardHomeVM dashboardHomeVM = new DashboardHomeVM
            {
                OutboundCount = dashboardHomeDTO.OutboundCount,
                OutboundDraftCountCreated = dashboardHomeDTO.OutboundDraftCountCreated,
                OutboundDraftCountAssigned = dashboardHomeDTO.OutboundDraftCountAssigned,
                InboundCountCreated = dashboardHomeDTO.InboundCountCreated,
                InboundCountAssigned = dashboardHomeDTO.InboundCountAssigned,
                InternalOutboundCountCreated = dashboardHomeDTO.InternalOutboundCountCreated,
                InternalOutboundCountAssigned = dashboardHomeDTO.InternalOutboundCountAssigned,
                DelayedCount = dashboardHomeDTO.DelayedCount
            };
            return dashboardHomeVM;
        }

        internal static DashboardHomeVM Map(DashboardHomeReportDTO result)
        {
            if (result == null)
            {
                return null;
            }

            return new DashboardHomeVM
            {
                TotalTransactions = result.TotalTransactions,
                TotalAssignments = result.TotalAssignments,
                TotalInbound = result.TotalInbound,
                TotalOutbound = result.TotalOutbound,
                TotalInternal = result.TotalInternal,
                LateAVG = result.LateAVG,
                TotalCompleted = result.TotalCompleted,
                DashboardReportBottomList = result.DashboardReportBottomList
            };
        }
    }
}