using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class DashboardMapper
    {
        public static List<DashboardDTO> Map(IList<TransactionCountReportInfo> transactionTypeReportInfos)
        {
            if (transactionTypeReportInfos == null || !transactionTypeReportInfos.Any())
            {
                return null;
            }
            List<DashboardDTO> dashboardDTOs = transactionTypeReportInfos
                .Select(transactionTypeReportInfo => new DashboardDTO
                {
                    TypeId = transactionTypeReportInfo.TypeId,
                    UserCategoryId = transactionTypeReportInfo.UserCategoryId,
                    Date = transactionTypeReportInfo.Date
                }).ToList();
            return dashboardDTOs;
        }

        public static List<TransactionCountReportInfo> Map(IList<DashboardDTO> transactionTypeReportInfoDTOs)
        {
            if (transactionTypeReportInfoDTOs == null || !transactionTypeReportInfoDTOs.Any())
            {
                return null;
            }
            List<TransactionCountReportInfo> dashboards = transactionTypeReportInfoDTOs
                .Select(transactionTypeReportInfoDTO => new TransactionCountReportInfo()
                {
                    TypeId = transactionTypeReportInfoDTO.TypeId,
                    UserCategoryId = transactionTypeReportInfoDTO.UserCategoryId,
                    Date = transactionTypeReportInfoDTO.Date
                }).ToList();

            return dashboards;
        }
    }
}