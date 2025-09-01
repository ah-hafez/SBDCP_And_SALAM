using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class DeliveryReportMapper
    {
        public static List<DeliveryReportVM> Map(IList<DeliveryReportDTO> deliveryReportDTOs)
        {
            if (deliveryReportDTOs == null || !deliveryReportDTOs.Any())
            {
                return new List<DeliveryReportVM>();
            }
            List<DeliveryReportVM> deliveryReportVMs = deliveryReportDTOs
                .Select(deliveryReportDTO => new DeliveryReportVM()
                { 
                    DateH = deliveryReportDTO.DateH,
                    DeliveryReportTransactions = DeliveryReportTransactionMapper.Map(deliveryReportDTO.DeliveryReportTransactions),
                    OrgUnitName = deliveryReportDTO.OrgUnitName,
                    ReportNumber = deliveryReportDTO.ReportNumber,
                    RootName = deliveryReportDTO.RootName,
                    UserName = deliveryReportDTO.UserName,
                    ConfidentialityName = deliveryReportDTO.ConfidentialityName,
                    TransactionTypeName = deliveryReportDTO.TransactionTypeName
                }).ToList();

            return deliveryReportVMs;
        }
        public static List<DeliveryReportDTO> Map(IList<DeliveryReportVM> deliveryReportVMs)
        {
            if (deliveryReportVMs == null || !deliveryReportVMs.Any())
            {
                return new List<DeliveryReportDTO>();
            }
            List<DeliveryReportDTO> deliveryReportDTOs = deliveryReportVMs
                .Select(deliveryReportVM => new DeliveryReportDTO()
                {
                    DateH = deliveryReportVM.DateH,
                    DeliveryReportTransactions = DeliveryReportTransactionMapper.Map(deliveryReportVM.DeliveryReportTransactions),
                    OrgUnitName = deliveryReportVM.OrgUnitName,
                    ReportNumber = deliveryReportVM.ReportNumber,
                    RootName = deliveryReportVM.RootName,
                    UserName = deliveryReportVM.UserName
                }).ToList();

            return deliveryReportDTOs;
        }


    }
}