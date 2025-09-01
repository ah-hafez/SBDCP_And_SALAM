using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Report;

namespace MCS.UI.Areas.User.Mappers.Report
{
    public static class TransactionReportInfoMapper
    {
        public static TransactionReportInfoVM Map(TransactionReportInfoDTO transactionReportInfoDTO)
        {
            if (transactionReportInfoDTO != null)
            {
                TransactionReportInfoVM transactionReportInfoVM = new TransactionReportInfoVM()
                { 
                    ReportsIds = transactionReportInfoDTO.ReportsIds,
                    TransactionId = transactionReportInfoDTO.TransactionId
                };
                return transactionReportInfoVM;
            }
            return new TransactionReportInfoVM();
        }
        public static TransactionReportInfoDTO Map(TransactionReportInfoVM transactionReportInfoVM)
        {
            if (transactionReportInfoVM != null)
            {
                TransactionReportInfoDTO transactionReportInfoDTO = new TransactionReportInfoDTO()
                { 
                    ReportsIds = transactionReportInfoVM.ReportsIds,
                    TransactionId = transactionReportInfoVM.TransactionId
                };
                return transactionReportInfoDTO;
            }
            return new TransactionReportInfoDTO();
        }
        public static List<TransactionReportInfoDTO> Map(IList<TransactionReportInfoVM> transactionReportInfoVMs)
        {
            if (transactionReportInfoVMs == null || !transactionReportInfoVMs.Any())
            {
                return null;
            }
                List<TransactionReportInfoDTO> transactionReportInfoDTOs = transactionReportInfoVMs
                    .Select(transactionReportInfoVM => new TransactionReportInfoDTO()
                    {
                        ReportsIds = transactionReportInfoVM.ReportsIds,
                        TransactionId = transactionReportInfoVM.TransactionId
                    }).ToList();
                return transactionReportInfoDTOs;
            
            
        }
        public static List<TransactionReportInfoVM> Map(IList<TransactionReportInfoDTO> transactionReportInfoDTOs)
        {
            if (transactionReportInfoDTOs == null || !transactionReportInfoDTOs.Any())
            {
                return null;
            }
            List<TransactionReportInfoVM> transactionReportInfoVMs = transactionReportInfoDTOs
                .Select(transactionReportInfoDTO => new TransactionReportInfoVM()
                {
                    ReportsIds = transactionReportInfoDTO.ReportsIds,
                    TransactionId = transactionReportInfoDTO.TransactionId
                }).ToList();
            return transactionReportInfoVMs;


        }
    }
}