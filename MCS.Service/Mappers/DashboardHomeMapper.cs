using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.Domain.Search.SearchResult;
using MCS.DTO;
using MCS.DTO.Shared;

namespace MCS.Service.Mappers
{
    public class DashboardHomeMapper
    {
        public static DashboardHomeDTO Map(DashboardHome dashboardHome)
        {
            if (dashboardHome == null)
            {
                return null;
            }
            DashboardHomeDTO dashboardHomeDTO = new DashboardHomeDTO
            {
                OutboundCount = dashboardHome.OutboundCount,
                OutboundDraftCountCreated = dashboardHome.OutboundDraftCountCreated,
                OutboundDraftCountAssigned = dashboardHome.OutboundDraftCountAssigned,
                InboundCountCreated = dashboardHome.InboundCountCreated,
                InboundCountAssigned = dashboardHome.InboundCountAssigned,
                InternalOutboundCountCreated = dashboardHome.InternalOutboundCountCreated,
                InternalOutboundCountAssigned = dashboardHome.InternalOutboundCountAssigned,
                DelayedCount = dashboardHome.DelayedCount
            };
            return dashboardHomeDTO;
        }

        internal static DashboardHomeReportDTO Map(GetDashboardReportResult getDashboardReportResult)
        {
            if (getDashboardReportResult == null)
            {
                return null;
            }

            return new DashboardHomeReportDTO
            {
                LateAVG = getDashboardReportResult.LateAVG,
                TotalAssignments = getDashboardReportResult.TotalAssignments,
                TotalCompleted = getDashboardReportResult.TotalCompleted,
                TotalInbound = getDashboardReportResult.TotalInbound,
                TotalInternal = getDashboardReportResult.TotalInternal,
                TotalOutbound = getDashboardReportResult.TotalOutbound,
                TotalTransactions = getDashboardReportResult.TotalTransactions
            };
        }

        internal static List<DashboardReportBottomDTO> Map(List<GetDashboardReportBottomResult> getDashboardReportBottomResults)
        {
            if (getDashboardReportBottomResults == null)
            {
                return null;
            }

            return getDashboardReportBottomResults.Select(x => new DashboardReportBottomDTO
            {
                ReportType = x.ReportType,
                TotalCount = x.TotalCount,
                TypeId = x.TypeId,
                YEAR = x.YEAR
            }).ToList();
        }
    }
}