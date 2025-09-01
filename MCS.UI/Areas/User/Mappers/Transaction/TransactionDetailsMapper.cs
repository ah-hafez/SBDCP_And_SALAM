using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class TransactionDetailsMapper
    {
        public static List<TransactionDetailsVM> Map(IList<TransactionDetailsDTO> transactionDetailsDTOs)
        {
            if (transactionDetailsDTOs == null || !transactionDetailsDTOs.Any())
            {
                return new List<TransactionDetailsVM>();
            }
            List<TransactionDetailsVM> transactionDetailsVMs = transactionDetailsDTOs
                .Select(transactionDetailsDTO => new TransactionDetailsVM()
                {
                    Id = transactionDetailsDTO.Id,
                    Number = transactionDetailsDTO.Number,
                    Date = transactionDetailsDTO.Date,
                    HijriDate = transactionDetailsDTO.HijriDate,
                    LetterTypeId = transactionDetailsDTO.LetterTypeId,
                    LetterType = transactionDetailsDTO.LetterType,
                    PriorityId = transactionDetailsDTO.PriorityId,
                    Priority = transactionDetailsDTO.Priority,
                    ConfidentialityId = transactionDetailsDTO.ConfidentialityId,
                    Confidentiality = transactionDetailsDTO.Confidentiality,
                    Subject = transactionDetailsDTO.Subject,
                    SourceType = transactionDetailsDTO.TransactionType,
                    CreatedOn = transactionDetailsDTO.CreatedOn,
                    Creator = transactionDetailsDTO.Creator,
                    Year=transactionDetailsDTO.Year,
                    TransactionTypeId = transactionDetailsDTO.TransactionCategoryId,
                    CurrentUser = transactionDetailsDTO.CurrentUser,
                    LetterNumber = transactionDetailsDTO.LetterNumber
                }).ToList();
            return transactionDetailsVMs;
        }
        public static List<TransactionDetailsDTO> Map(IList<TransactionDetailsVM> transactionDetailsVMs)
        {
            if (transactionDetailsVMs == null || !transactionDetailsVMs.Any())
            {
                return new List<TransactionDetailsDTO>();
            }
            List<TransactionDetailsDTO> transactionDetailsDTOs = transactionDetailsVMs
                .Select(transactionDetailsVM => new TransactionDetailsDTO()
                {
                    Id = transactionDetailsVM.Id,
                    Date = transactionDetailsVM.Date,
                    HijriDate = transactionDetailsVM.HijriDate,
                    Year = transactionDetailsVM.Year,
                    Number = transactionDetailsVM.Number,
                    TransactionsTypes = transactionDetailsVM.TransactionSources,
                    TransactionCategoryId = transactionDetailsVM. TransactionTypeId
                }).ToList();
            return transactionDetailsDTOs;
        }
        public static TransactionDetailsDTO Map(TransactionDetailsVM transactionDetailsVM)
        {
            if (transactionDetailsVM != null)
            {
                TransactionDetailsDTO transactionDetailsDTOs = new TransactionDetailsDTO()
                {
                    Id = transactionDetailsVM.Id,
                    Date = transactionDetailsVM.Date,
                    HijriDate = transactionDetailsVM.HijriDate,
                    Number = transactionDetailsVM.Number,
                    TransactionsTypes = transactionDetailsVM.TransactionSources,
                    TransactionCategoryId = transactionDetailsVM.TransactionTypeId
                };
                return transactionDetailsDTOs;
            }
            return new TransactionDetailsDTO();
        }
        public static TransactionDetailsVM Map(TransactionDetailsDTO transactionDetailsDTO)
        {
            if (transactionDetailsDTO != null)
            {
                TransactionDetailsVM transactionDetailsVM = new TransactionDetailsVM()
                {
                    Id = transactionDetailsDTO.Id,
                    Date = transactionDetailsDTO.Date,
                    HijriDate = transactionDetailsDTO.HijriDate,
                    Number = transactionDetailsDTO.Number,
                    TransactionSources = transactionDetailsDTO.TransactionsTypes,
                    TransactionTypeId = transactionDetailsDTO.TransactionCategoryId
                };
                return transactionDetailsVM;
            }
            return new TransactionDetailsVM();
        }
    }
}