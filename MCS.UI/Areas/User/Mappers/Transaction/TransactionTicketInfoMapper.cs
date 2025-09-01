using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class TransactionTicketInfoMapper
    {
        public static List<TransactionTicketInfoVM> Map(IList<TransactionTicketInfoDTO> transactionTicketInfoDTOs)
        {
            if (transactionTicketInfoDTOs == null || !transactionTicketInfoDTOs.Any())
            {
                return new List<TransactionTicketInfoVM>();
            }
            List<TransactionTicketInfoVM> transactionTicketInfoVMs = transactionTicketInfoDTOs
                .Select(transactionTicketInfoDTO => new TransactionTicketInfoVM()
                { 
                    CultureId = transactionTicketInfoDTO.CultureId,
                    TransactionId = transactionTicketInfoDTO.TransactionId,
                    UserId = transactionTicketInfoDTO.UserId
                }).ToList();

            return transactionTicketInfoVMs;
        }
        public static List<TransactionTicketInfoDTO> Map(IList<TransactionTicketInfoVM> transactionTicketInfoVMs)
        {
            if (transactionTicketInfoVMs == null || !transactionTicketInfoVMs.Any())
            {
                return new List<TransactionTicketInfoDTO>();
            }
            List<TransactionTicketInfoDTO> transactionTicketInfoDTOs = transactionTicketInfoVMs
                .Select(transactionTicketInfoVM => new TransactionTicketInfoDTO()
                { 
                    CultureId = transactionTicketInfoVM.CultureId,
                    TransactionId = transactionTicketInfoVM.TransactionId,
                    UserId = transactionTicketInfoVM.UserId
                }).ToList();

            return transactionTicketInfoDTOs;
        }


    }
}