using System.Collections.Generic;
using System.Linq;
using MCS.Business;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class DeliveryReportMapper
    {
        public static DeliveryReportDTO Map(DeliveryReportInfoDTO deliveryReport)
        {
            if (deliveryReport == null)
            {
                return null;
            }
            DeliveryReportDTO deliveryReportDTO = new DeliveryReportDTO()
            {
                OrgUnitName = deliveryReport.OrgUnitName,
                ReportNumber = deliveryReport.ReportNumber,
                DateH = deliveryReport.DateH,
                RootName = deliveryReport.RootOrgUnitName,
                UserName = deliveryReport.UserName,
                DeliveryReportTransactions = DeliveryReportMapper.Map(deliveryReport.DeliveryReportTransactions),
            };

            return deliveryReportDTO;
        }
        public static List<DeliveryReportDTO> Map(IList<DeliveryReportInfoDTO> deliveryReports)
        {
            if (deliveryReports == null || !deliveryReports.Any())
            {
                return null;
            }
            List<DeliveryReportDTO> deliveryReportDTOs = deliveryReports.Select(deliveryReportDTO => new DeliveryReportDTO()
            {
                OrgUnitName = deliveryReportDTO.OrgUnitName,
                ReportNumber = deliveryReportDTO.ReportNumber,
                DateH = deliveryReportDTO.DateH,
                RootName = deliveryReportDTO.RootOrgUnitName,
                UserName = deliveryReportDTO.UserName,
                DeliveryReportTransactions = DeliveryReportMapper.Map(deliveryReportDTO.DeliveryReportTransactions),
                ConfidentialityName = deliveryReportDTO.ConfidentialityName,
                TransactionTypeName = deliveryReportDTO.TransactionTypeName
            }).ToList();


            return deliveryReportDTOs;
        }

        public static List<DeliveryReportTransactionDTO> Map(IList<DeliveryReportTransactionInfoDTO> deliveryReportTransactions)
        {
            if (deliveryReportTransactions == null || !deliveryReportTransactions.Any())
            {
                return null;
            }
            List<DeliveryReportTransactionDTO> deliveryReportTransactionDTOs = deliveryReportTransactions.Select(deliveryReportTransactionDTO => new DeliveryReportTransactionDTO()
            {
                TransactionNumber = deliveryReportTransactionDTO.TransactionNumber,
                AttachmentCount = deliveryReportTransactionDTO.AttachmentCount,
                FromEntity = deliveryReportTransactionDTO.FromEntity,
                ToEntity = deliveryReportTransactionDTO.ToEntity,
                DateH = deliveryReportTransactionDTO.DateH,
                TransactionType = deliveryReportTransactionDTO.TransactionCategory,
                AttachmentTotal = deliveryReportTransactionDTO.AttachmentTotal,
                Receiver = deliveryReportTransactionDTO.Receiver,
                TransactionTypeId = deliveryReportTransactionDTO.TransactionCategoryId,
                IsCopy = deliveryReportTransactionDTO.IsCopy,
                ExternalParty = deliveryReportTransactionDTO.ExternalParty,
                Subject = deliveryReportTransactionDTO.Subject
            }).ToList();



            return deliveryReportTransactionDTOs;
        }

        public static List<TransactionReportInfoDTO> Map(IList<TransactionReportInfo> transactionReportInfos)
        {
            if (transactionReportInfos == null || !transactionReportInfos.Any())
            {
                return null;
            }
            List<TransactionReportInfoDTO> transactionReportInfoDTOs = transactionReportInfos
                .Select(transactionReportInfoDTO => new TransactionReportInfoDTO()
                {
                    TransactionId = transactionReportInfoDTO.TransactionId,
                    ReportsIds = transactionReportInfoDTO.ReportsIds != null ? transactionReportInfoDTO.ReportsIds : null
                }).ToList();


            return transactionReportInfoDTOs;
        }

        public static List<TransactionReportInfo> Map(IList<TransactionReportInfoDTO> transactionReportInfoDTOs)
        {
            if (transactionReportInfoDTOs == null || !transactionReportInfoDTOs.Any())
            {
                return null;
            }
            List<TransactionReportInfo> transactionReportInfos = transactionReportInfoDTOs
                .Select(transactionReportInfo => new TransactionReportInfo()
                {
                    TransactionId = transactionReportInfo.TransactionId,
                    RejectReportId = transactionReportInfo.RejectReportId,
                    ReportsIds = transactionReportInfo.ReportsIds != null ? transactionReportInfo.ReportsIds : null
                }).ToList();



            return transactionReportInfos;
        }
    }
}