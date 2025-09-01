using MCS.DTO;
using MCS.UI.Areas.User.Models.Report;

namespace MCS.UI.Areas.User.Mappers.Report
{
    public static class TransactionsDeliveryReportMapper
    {
        public static TransactionsDeliveryReportVM Map(TransactionsDeliveryReportDTO transactionsDeliveryReportDTO)
        {
            if (transactionsDeliveryReportDTO != null)
            {
                TransactionsDeliveryReportVM transactionsDeliveryReportVM = new TransactionsDeliveryReportVM()
                {
                    ToOrgUnit = transactionsDeliveryReportDTO.AssignedOrgUnitId,
                    ConfidentialityLevelId = transactionsDeliveryReportDTO.ConfidentialityLevelId,
                    DateFrom = transactionsDeliveryReportDTO.DateFrom,
                    DateTo = transactionsDeliveryReportDTO.DateTo,
                    FromTransactionNumber = transactionsDeliveryReportDTO.FromTransactionNumber,
                    HourFrom = transactionsDeliveryReportDTO.HourFrom,
                    HourTo = transactionsDeliveryReportDTO.HourTo,
                    LetterTypeId = transactionsDeliveryReportDTO.LetterTypeId,
                    MinuteFrom = transactionsDeliveryReportDTO.MinuteFrom,
                    MinuteTo = transactionsDeliveryReportDTO.MinuteTo,
                    PriorityLevelId = transactionsDeliveryReportDTO.PriorityLevelId,
                    RePrint = transactionsDeliveryReportDTO.RePrint,
                    TimeFrom = transactionsDeliveryReportDTO.TimeFrom,
                    TimeTo = transactionsDeliveryReportDTO.TimeTo,
                    ToTransactionNumber = transactionsDeliveryReportDTO.ToTransactionNumber,
                    TransactionCategoryId = transactionsDeliveryReportDTO.TransactionCategoryId,
                    UserId = transactionsDeliveryReportDTO.UserId,
                    DeliveryReportNumber = transactionsDeliveryReportDTO.DeliveryReportNumber
                };
                return transactionsDeliveryReportVM;
            }
            return new TransactionsDeliveryReportVM();
        }
        public static TransactionsDeliveryReportDTO Map(TransactionsDeliveryReportVM transactionsDeliveryReportVM)
        {
            if (transactionsDeliveryReportVM != null)
            {
                TransactionsDeliveryReportDTO transactionsDeliveryReportDTO = new TransactionsDeliveryReportDTO()
                {
                    AssignedOrgUnitId = transactionsDeliveryReportVM.ToOrgUnit,
                    ConfidentialityLevelId = transactionsDeliveryReportVM.ConfidentialityLevelId,
                    DateFrom = transactionsDeliveryReportVM.DateFrom,
                    DateTo = transactionsDeliveryReportVM.DateTo,
                    FromTransactionNumber = transactionsDeliveryReportVM.FromTransactionNumber,
                    HourFrom = transactionsDeliveryReportVM.HourFrom,
                    HourTo = transactionsDeliveryReportVM.HourTo,
                    LetterTypeId = transactionsDeliveryReportVM.LetterTypeId,
                    MinuteFrom = transactionsDeliveryReportVM.MinuteFrom,
                    MinuteTo = transactionsDeliveryReportVM.MinuteTo,
                    PriorityLevelId = transactionsDeliveryReportVM.PriorityLevelId,
                    RePrint = transactionsDeliveryReportVM.RePrint,
                    TimeFrom = transactionsDeliveryReportVM.TimeFrom,
                    TimeTo = transactionsDeliveryReportVM.TimeTo,
                    ToTransactionNumber = transactionsDeliveryReportVM.ToTransactionNumber,
                    TransactionCategoryId = transactionsDeliveryReportVM.TransactionCategoryId,
                    UserId = transactionsDeliveryReportVM.UserId,
                    DeliveryReportNumber = transactionsDeliveryReportVM.DeliveryReportNumber
                };
                return transactionsDeliveryReportDTO;
            }
            return new TransactionsDeliveryReportDTO();
        }
    }
}