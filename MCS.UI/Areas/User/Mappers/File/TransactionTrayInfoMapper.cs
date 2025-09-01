using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Transaction;
using MCS.UI.Areas.User.Models.File;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.File
{
    public static class TransactionTrayInfoMapper
    {
        public static List<TransactionTrayInfoVM> Map(IList<TransactionTrayInfoDTO> transactionTrayInfoDTOs)
        {
            if (transactionTrayInfoDTOs == null || !transactionTrayInfoDTOs.Any())
            {
                return new List<TransactionTrayInfoVM>();
            }
            List<TransactionTrayInfoVM> transactionTrayInfoVMs = transactionTrayInfoDTOs
                .Select(transactionTrayInfoDTO => new TransactionTrayInfoVM()
                { 
                    TransactionDetailsInfoVM = TransactionDetailsInfoMapper.Map(transactionTrayInfoDTO.TransactionDetailsInfoDTOs),
                    TransactionAssignmentInfoVMs = TransactionAssignmentInfoMapper.Map(transactionTrayInfoDTO.TransactionAssignmentInfoDTOs),
                    SentTaskVM = SentTaskMapper.Map(transactionTrayInfoDTO.sentTasDTO),
                    IsVIPUser = (bool)SessionInfo.GetObjectFromSession(Constants.IsVIPUser)
                }).ToList();

            return transactionTrayInfoVMs;
        }
        public static List<TransactionTrayInfoDTO> Map(IList<TransactionTrayInfoVM> transactionTrayInfoVMs)
        {
            if (transactionTrayInfoVMs == null || !transactionTrayInfoVMs.Any())
            {
                return new List<TransactionTrayInfoDTO>();
            }
            List<TransactionTrayInfoDTO> transactionTrayInfoDTOs = transactionTrayInfoVMs
                .Select(transactionTrayInfoVM => new TransactionTrayInfoDTO()
                { 
                    TransactionDetailsInfoDTOs = TransactionDetailsInfoMapper.Map(transactionTrayInfoVM.TransactionDetailsInfoVM),
                    TransactionAssignmentInfoDTOs = TransactionAssignmentInfoMapper.Map(transactionTrayInfoVM.TransactionAssignmentInfoVMs),
                    sentTasDTO = SentTaskMapper.Map(transactionTrayInfoVM.SentTaskVM),
                }).ToList();

            return transactionTrayInfoDTOs;
        }
    }
}