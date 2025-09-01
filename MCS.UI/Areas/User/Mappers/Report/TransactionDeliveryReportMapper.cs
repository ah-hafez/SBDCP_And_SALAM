using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Report;

namespace MCS.UI.Areas.User.Mappers.Report
{
    public static class TransactionDeliveryReportMapper
    {
        public static TransactionDeliveryReportVM Map(TransactionDeliveryReportDTO transactionDeliveryReportDTO)
        {
            if (transactionDeliveryReportDTO != null)
            {
                TransactionDeliveryReportVM transactionDeliveryReportVM = new TransactionDeliveryReportVM()
                {
                    ToEntity = transactionDeliveryReportDTO.ToEntity,
                    Confidentiality = transactionDeliveryReportDTO.Confidentiality,
                    Date = transactionDeliveryReportDTO.Date,
                    DateH = transactionDeliveryReportDTO.DateH,
                    DeliveryMethod = transactionDeliveryReportDTO.DeliveryMethod,
                    Id = transactionDeliveryReportDTO.Id,
                    Number = transactionDeliveryReportDTO.Number,
                    PrintedDeliveryReport = transactionDeliveryReportDTO.PrintedDeliveryReport,
                    Priority = transactionDeliveryReportDTO.Priority,
                    TransactionTypeName = transactionDeliveryReportDTO.TransactionTypeName,
                    TransactionId = transactionDeliveryReportDTO.TransactionId,
                    User = transactionDeliveryReportDTO.User,
                    TransactionCategoryName = transactionDeliveryReportDTO.TransactionCategoryName,
                    TransactionCategoryId = transactionDeliveryReportDTO.TransactionCategoryId,
                    IsForIndividual = transactionDeliveryReportDTO.IsForIndividual,
                    ExternalPartyName = (transactionDeliveryReportDTO.ExternalPartyName != null) ? transactionDeliveryReportDTO.ExternalPartyName : null,
                    ExternalPartyId = transactionDeliveryReportDTO.ExternalPartyId,
                    InternalPartyName = (transactionDeliveryReportDTO.InternalPartyName != null) ? transactionDeliveryReportDTO.InternalPartyName : null,
                    InternalPartyId = transactionDeliveryReportDTO.InternalPartyId,
                    Subject = transactionDeliveryReportDTO.Subject
                };
                return transactionDeliveryReportVM;
            }
            return new TransactionDeliveryReportVM();
        }
        public static TransactionDeliveryReportDTO Map(TransactionDeliveryReportVM transactionDeliveryReportVM)
        {
            if (transactionDeliveryReportVM != null)
            {
                TransactionDeliveryReportDTO transactionDeliveryReportDTO = new TransactionDeliveryReportDTO()
                {
                    ToEntity = transactionDeliveryReportVM.ToEntity,
                    Confidentiality = transactionDeliveryReportVM.Confidentiality,
                    Date = transactionDeliveryReportVM.Date,
                    DateH = transactionDeliveryReportVM.DateH,
                    DeliveryMethod = transactionDeliveryReportVM.DeliveryMethod,
                    Id = transactionDeliveryReportVM.Id,
                    Number = transactionDeliveryReportVM.Number,
                    PrintedDeliveryReport = transactionDeliveryReportVM.PrintedDeliveryReport,
                    Priority = transactionDeliveryReportVM.Priority,
                    TransactionTypeName = transactionDeliveryReportVM.TransactionTypeName,
                    TransactionId = transactionDeliveryReportVM.TransactionId,
                    User = transactionDeliveryReportVM.User,
                    TransactionCategoryName = transactionDeliveryReportVM.TransactionCategoryName,
                    Subject = transactionDeliveryReportVM.Subject,
                    TransactionCategoryId = transactionDeliveryReportVM.TransactionCategoryId,
                    IsForIndividual = transactionDeliveryReportVM.IsForIndividual,
                    ExternalPartyName = (transactionDeliveryReportVM.ExternalPartyName != null) ? transactionDeliveryReportVM.ExternalPartyName : null,
                    ExternalPartyId = transactionDeliveryReportVM.ExternalPartyId,
                    InternalPartyName = (transactionDeliveryReportVM.InternalPartyName != null) ? transactionDeliveryReportVM.InternalPartyName : null,
                    InternalPartyId = transactionDeliveryReportVM.InternalPartyId,
                };
                return transactionDeliveryReportDTO;
            }
            return new TransactionDeliveryReportDTO();
        }
        public static List<TransactionDeliveryReportVM> Map(IList<TransactionDeliveryReportDTO> transactionDeliveryReportDTOs)
        {
            if (transactionDeliveryReportDTOs == null || !transactionDeliveryReportDTOs.Any())
            {
                return new List<TransactionDeliveryReportVM>();
            }
            List<TransactionDeliveryReportVM> transactionDeliveryReportVMs = transactionDeliveryReportDTOs
                .Select(b => new TransactionDeliveryReportVM
                {
                    ToEntity = b.ToEntity,
                    Confidentiality = b.Confidentiality,
                    Date = b.Date,
                    DateH = b.DateH,
                    DeliveryMethod = b.DeliveryMethod,
                    Id = b.Id,
                    Number = b.Number,
                    PrintedDeliveryReport = b.PrintedDeliveryReport,
                    Priority = b.Priority,
                    TransactionTypeName = b.TransactionTypeName,
                    TransactionId = b.TransactionId,
                    User = b.User,
                    Subject = b.Subject,
                    TransactionCategoryName = b.TransactionCategoryName,
                    TransactionNumber = b.TransactionNumber,
                    TransactionCategoryId = b.TransactionCategoryId,
                    IsForIndividual = b.IsForIndividual,
                    ExternalPartyId = b.ExternalPartyId,
                    ExternalPartyName = b.ExternalPartyName,
                    InternalPartyId = b.InternalPartyId,
                    InternalPartyName = b.InternalPartyName,
                    IsCopy = b.IsCopy,
                    ToEntityId = b.ToEntityId
                }).ToList();
            return transactionDeliveryReportVMs;
        }

    }
}