using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Report;

namespace MCS.UI.Areas.User.Mappers.Report
{
    public class DeliveryReportTransactionInfoMapper
    {
        public static DeliveryReportTransactionInfoVM Map(DeliveryReportTransactionInfoDTO deliveryReportTransactionInfoDTO)
        {
            if (deliveryReportTransactionInfoDTO != null)
            {
                DeliveryReportTransactionInfoVM deliveryReportTransactionInfoVM = new DeliveryReportTransactionInfoVM()
                { 
                    AttachmentCount = deliveryReportTransactionInfoDTO.AttachmentCount,
                    FromEntity = deliveryReportTransactionInfoDTO.FromEntity,
                    ToEntity = deliveryReportTransactionInfoDTO.ToEntity,
                    TransactionNumber = deliveryReportTransactionInfoDTO.TransactionNumber
                };
                return deliveryReportTransactionInfoVM;
            }
            return new DeliveryReportTransactionInfoVM();
        }
        public static DeliveryReportTransactionInfoDTO Map(DeliveryReportTransactionInfoVM deliveryReportTransactionInfoVM)
        {
            if (deliveryReportTransactionInfoVM != null)
            {
                DeliveryReportTransactionInfoDTO deliveryReportTransactionInfoDTO = new DeliveryReportTransactionInfoDTO()
                { 
                    AttachmentCount = deliveryReportTransactionInfoVM.AttachmentCount,
                    FromEntity = deliveryReportTransactionInfoVM.FromEntity,
                    ToEntity = deliveryReportTransactionInfoVM.ToEntity,
                    TransactionNumber = deliveryReportTransactionInfoVM.TransactionNumber
                };
                return deliveryReportTransactionInfoDTO;
            }
            return new DeliveryReportTransactionInfoDTO();
        }
        public static List<DeliveryReportTransactionInfoVM> Map(IList<DeliveryReportTransactionInfoDTO> deliveryReportTransactionInfoDTOs)
        {
            if (deliveryReportTransactionInfoDTOs == null || !deliveryReportTransactionInfoDTOs.Any())
            {
                return new List<DeliveryReportTransactionInfoVM>();
            }
            List<DeliveryReportTransactionInfoVM> deliveryReportTransactionInfoVMs = deliveryReportTransactionInfoDTOs
                .Select(deliveryReportTransactionInfoDTO => new DeliveryReportTransactionInfoVM
                { 
                    AttachmentCount = deliveryReportTransactionInfoDTO.AttachmentCount,
                    FromEntity = deliveryReportTransactionInfoDTO.FromEntity,
                    ToEntity = deliveryReportTransactionInfoDTO.ToEntity,
                    TransactionNumber = deliveryReportTransactionInfoDTO.TransactionNumber
                }).ToList();
            return deliveryReportTransactionInfoVMs;
        }
        public static List<DeliveryReportTransactionInfoDTO> Map(IList<DeliveryReportTransactionInfoVM> deliveryReportTransactionInfoVMs)
        {
            if (deliveryReportTransactionInfoVMs == null || !deliveryReportTransactionInfoVMs.Any())
            {
                return new List<DeliveryReportTransactionInfoDTO>();
            }
            List<DeliveryReportTransactionInfoDTO> deliveryReportTransactionInfoDTOs = deliveryReportTransactionInfoVMs
                .Select(deliveryReportTransactionInfoVM => new DeliveryReportTransactionInfoDTO
                {
                    AttachmentCount = deliveryReportTransactionInfoVM.AttachmentCount,
                    FromEntity = deliveryReportTransactionInfoVM.FromEntity,
                    ToEntity = deliveryReportTransactionInfoVM.ToEntity,
                    TransactionNumber = deliveryReportTransactionInfoVM.TransactionNumber
                }).ToList();
            return deliveryReportTransactionInfoDTOs;
        }
    }
}