using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.OrgUnit;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class TransactionsCountMapper
    {
        public static List<TransactionsCountVM> Map(IList<TransactionsCountDTO> transactionsCountDTOs)
        {
            if (transactionsCountDTOs == null || !transactionsCountDTOs.Any())
            { return null; }
            List<TransactionsCountVM> transactionsCountVMs = transactionsCountDTOs
                .Select(b => new TransactionsCountVM
                {
                      
                    Count = b.Count,
                    TransactionCategoryId = b.TransactionCategoryId

                }).ToList();
            return transactionsCountVMs;
        }
        public static List<TransactionsCountDTO> Map(IList<TransactionsCountVM> transactionsCountVMs)
        {
            if (transactionsCountVMs == null || !transactionsCountVMs.Any())
            { return null; }
            List<TransactionsCountDTO> transactionsCountDTOs = transactionsCountVMs
                .Select(b => new TransactionsCountDTO 
                {
                     
                    Count = b.Count,  
                    TransactionCategoryId = b.TransactionCategoryId

                }).ToList();
            return transactionsCountDTOs;
        }
    }
}