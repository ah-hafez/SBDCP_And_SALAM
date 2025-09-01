using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class DeliveryReportTransactionMapper
    {
        public static List<DeliveryReportTransactionVM> Map(IList<DeliveryReportTransactionDTO> deliveryReportTransactionDTOs)
        {
            if (deliveryReportTransactionDTOs == null || !deliveryReportTransactionDTOs.Any())
            {
                return new List<DeliveryReportTransactionVM>();
            }
            List<DeliveryReportTransactionVM> deliveryReportTransactionVMs = deliveryReportTransactionDTOs
                .Select(deliveryReportTransactionDTO => new DeliveryReportTransactionVM()
                {
                    AttachmentTotal = deliveryReportTransactionDTO.AttachmentTotal,
                    AttachmentCount = deliveryReportTransactionDTO.AttachmentCount,
                    DateAndSignature = deliveryReportTransactionDTO.DateAndSignature,
                    FromEntity = deliveryReportTransactionDTO.FromEntity,
                    Receiver = deliveryReportTransactionDTO.Receiver,
                    ToEntity = deliveryReportTransactionDTO.ToEntity,
                    TransactionNumber = deliveryReportTransactionDTO.TransactionNumber,
                    DateH = deliveryReportTransactionDTO.DateH,
                    TransactionType = deliveryReportTransactionDTO.TransactionType,
                    TransactionTypeId = deliveryReportTransactionDTO.TransactionTypeId,
                    IsCopy = deliveryReportTransactionDTO.IsCopy,
                    ExternalParty = deliveryReportTransactionDTO.ExternalParty,
                    Subject = deliveryReportTransactionDTO.Subject,
                    DateTime = DateTime.Now.ToString()
                }).ToList();

            return deliveryReportTransactionVMs;
        }
        public static List<DeliveryReportTransactionDTO> Map(IList<DeliveryReportTransactionVM> deliveryReportTransactionVMs)
        {
            if (deliveryReportTransactionVMs == null || !deliveryReportTransactionVMs.Any())
            {
                return new List<DeliveryReportTransactionDTO>();
            }
            List<DeliveryReportTransactionDTO> deliveryReportTransactionDTOs = deliveryReportTransactionVMs
                .Select(deliveryReportTransactionVM => new DeliveryReportTransactionDTO()
                {
                    AttachmentTotal = deliveryReportTransactionVM.AttachmentTotal,
                    AttachmentCount = deliveryReportTransactionVM.AttachmentCount,
                    DateAndSignature = deliveryReportTransactionVM.DateAndSignature,
                    FromEntity = deliveryReportTransactionVM.FromEntity,
                    Receiver = deliveryReportTransactionVM.Receiver,
                    ToEntity = deliveryReportTransactionVM.ToEntity,
                    TransactionNumber = deliveryReportTransactionVM.TransactionNumber,
                    DateH = deliveryReportTransactionVM.DateH,
                    Subject= deliveryReportTransactionVM.Subject,
                    DateTime = DateTime.Now.ToString()

        }).ToList();

            return deliveryReportTransactionDTOs;
        }


    }
}