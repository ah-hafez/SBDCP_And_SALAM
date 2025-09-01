using System.Collections.Generic;
using System.Linq;
using MCS.DTO.File;
using MCS.UI.Areas.User.Models.File;

namespace MCS.UI.Areas.User.Mappers.File
{
    public static class TransactionsBatchMapper
    {
        public static List<TransactionsBatchVM> Map(IList<TransactionsBatchDTO> transactionsBatchDTOs)
        {
            if (transactionsBatchDTOs == null || !transactionsBatchDTOs.Any())
            {
                return new List<TransactionsBatchVM>();
            }
            List<TransactionsBatchVM> transactionsBatchVMs = transactionsBatchDTOs
                .Select(transactionsBatchDTO => new TransactionsBatchVM()
                { 
                    TransIds = transactionsBatchDTO.TransIds,
                    IsLinked = transactionsBatchDTO.IsLinked,
                    BatchName = transactionsBatchDTO.BatchName
                }).ToList();

            return transactionsBatchVMs;
        }
        public static List<TransactionsBatchDTO> Map(IList<TransactionsBatchVM> transactionsBatchVMs)
        {
            if (transactionsBatchVMs == null || !transactionsBatchVMs.Any())
            {
                return new List<TransactionsBatchDTO>();
            }
            List<TransactionsBatchDTO> transactionsBatchDTOs = transactionsBatchVMs
                .Select(transactionsBatchVM => new TransactionsBatchDTO()
                { 
                    TransIds = transactionsBatchVM.TransIds,
                    IsLinked = transactionsBatchVM.IsLinked,
                    BatchName = transactionsBatchVM.BatchName
                }).ToList();

            return transactionsBatchDTOs;
        }
    }
}