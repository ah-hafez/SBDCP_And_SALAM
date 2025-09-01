using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class TransactionSubTaskMapper
    {
        public static List<TransactionSubTaskVM> Map(IList<TransactionSubTaskDTO> transactionSubTaskDTOs)
        {
            if (transactionSubTaskDTOs == null || !transactionSubTaskDTOs.Any())
            {
                return new List<TransactionSubTaskVM>();
            }
            List<TransactionSubTaskVM> transactionSubTaskVMs = transactionSubTaskDTOs
                .Select(transactionSubTaskDTO => new TransactionSubTaskVM()
                {
                    ParentId = transactionSubTaskDTO.ParentId,
                    SubTasks = SubTaskAddMapper.Map(transactionSubTaskDTO.SubTasks),
                    TransactionId = transactionSubTaskDTO.TransactionId
                }).ToList();

            return transactionSubTaskVMs;
        }
        public static List<TransactionSubTaskDTO> Map(IList<TransactionSubTaskVM> transactionSubTaskVMs)
        {
            if (transactionSubTaskVMs == null || !transactionSubTaskVMs.Any())
            {
                return new List<TransactionSubTaskDTO>();
            }
            List<TransactionSubTaskDTO> transactionSubTaskDTOs = transactionSubTaskVMs
                .Select(transactionSubTaskVM => new TransactionSubTaskDTO()
                {
                    ParentId = transactionSubTaskVM.ParentId,
                    SubTasks = SubTaskAddMapper.Map(transactionSubTaskVM.SubTasks),
                    TransactionId = transactionSubTaskVM.TransactionId
                }).ToList();

            return transactionSubTaskDTOs;
        }


    }
}