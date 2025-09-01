using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class TransactionTaskMapper
    {
        public static List<TransactionTaskVM> Map(IList<TransactionTaskDTO> transactionTaskDTOs)
        {
            if (transactionTaskDTOs == null || !transactionTaskDTOs.Any())
            {
                return new List<TransactionTaskVM>();
            }
            List<TransactionTaskVM> transactionTaskVMs = transactionTaskDTOs
                .Select(transactionTaskDTO => new TransactionTaskVM()
                {
                    TaskVMs = TaskAddMapper.Map(transactionTaskDTO.TaskDTOs),
                    TransactionId = transactionTaskDTO.TransactionId,
                }).ToList();

            return transactionTaskVMs;
        }
        public static List<TransactionTaskDTO> Map(IList<TransactionTaskVM> transactionTaskVMs)
        {
            if (transactionTaskVMs == null || !transactionTaskVMs.Any())
            {
                return new List<TransactionTaskDTO>();
            }
            List<TransactionTaskDTO> transactionTaskDTOs = transactionTaskVMs
                .Select(transactionTaskVM => new TransactionTaskDTO()
                {
                    TaskDTOs = TaskAddMapper.Map(transactionTaskVM.TaskVMs),
                    TransactionId = transactionTaskVM.TransactionId
                }).ToList();

            return transactionTaskDTOs;
        }
        public static TransactionTaskVM Map(TransactionTaskDTO transactionTaskDTO)
        {
            if (transactionTaskDTO != null)
            {
                TransactionTaskVM transactionTaskVM = new TransactionTaskVM()
                {
                    TaskVMs = TaskAddMapper.Map(transactionTaskDTO.TaskDTOs),
                    TransactionId = transactionTaskDTO.TransactionId,
                };

                return transactionTaskVM;
            }
            return new TransactionTaskVM();
        }
        public static TransactionTaskDTO Map(TransactionTaskVM transactionTaskVM)
        {
            if (transactionTaskVM != null)
            {
                TransactionTaskDTO transactionTaskDTO = new TransactionTaskDTO()
                {
                    TaskDTOs = TaskAddMapper.Map(transactionTaskVM.TaskVMs),
                    TransactionId = transactionTaskVM.TransactionId,
                };

                return transactionTaskDTO;
            }
            return new TransactionTaskDTO();
        }
    }
}