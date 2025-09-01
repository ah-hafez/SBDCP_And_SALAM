using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.OrgUnit;

namespace MCS.UI.Areas.User.Mappers.OrgUnit
{
    public static class TransactionsCountMapper
    {
        public static List<TransactionsCountVM> Map(IList<TransactionsCountDTO> transactionsCountDTOs)
        {
            if (transactionsCountDTOs == null || !transactionsCountDTOs.Any())
            {
                return new List<TransactionsCountVM>();
            }
            List<TransactionsCountVM> transactionsCountVMs = transactionsCountDTOs
                .Select(transactionsCountDTO => new TransactionsCountVM()
                {
                    Count = transactionsCountDTO.Count,
                    TransactionCategoryId = transactionsCountDTO.TransactionCategoryId
                }).ToList();

            return transactionsCountVMs;
        }
        public static List<TransactionsCountDTO> Map(IList<TransactionsCountVM> transactionsCountVMs)
        {
            if (transactionsCountVMs == null || !transactionsCountVMs.Any())
            {
                return new List<TransactionsCountDTO>();
            }
            List<TransactionsCountDTO> transactionsCountDTOs = transactionsCountVMs
                .Select(transactionsCountVM => new TransactionsCountDTO()
                {
                    Count = transactionsCountVM.Count,
                    TransactionCategoryId = transactionsCountVM.TransactionCategoryId
                }).ToList();
            return transactionsCountDTOs;
        }
    }
}