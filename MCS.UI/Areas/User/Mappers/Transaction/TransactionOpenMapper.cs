using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class TransactionOpenMapper
    {
        public static List<TransactionOpenVM> Map(IList<TransactionOpenDTO> transactionOpenDTOs)
        {
            if (transactionOpenDTOs == null || !transactionOpenDTOs.Any())
            {
                return new List<TransactionOpenVM>();
            }
            List<TransactionOpenVM> transactionOpenVMs = transactionOpenDTOs
                .Select(transactionOpenDTO => new TransactionOpenVM()
                {
                    OrgUnitId = transactionOpenDTO.OrgUnitId,
                    TransactionTypeId = transactionOpenDTO.TransactionTypeId,
                    TransactionNumber = transactionOpenDTO.TransactionNumber,
                    TransactionCategory = transactionOpenDTO.TransactionCategory,
                    Year = transactionOpenDTO.Year
                }).ToList();

            return transactionOpenVMs;
        }
        public static List<TransactionOpenDTO> Map(IList<TransactionOpenVM> transactionOpenVMs)
        {
            if (transactionOpenVMs == null || !transactionOpenVMs.Any())
            {
                return new List<TransactionOpenDTO>();
            }
            List<TransactionOpenDTO> transactionOpenDTOs = transactionOpenVMs
                .Select(transactionOpenVM => new TransactionOpenDTO()
                {
                    OrgUnitId = transactionOpenVM.OrgUnitId,
                    TransactionTypeId = transactionOpenVM.TransactionTypeId,
                    TransactionNumber = transactionOpenVM.TransactionNumber,
                    TransactionCategory = transactionOpenVM.TransactionCategory,
                    Year = transactionOpenVM.Year
                }).ToList();

            return transactionOpenDTOs;
        }


    }
}