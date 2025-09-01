using System.Collections.Generic;
using System.Linq;
using MCS.DTO;

namespace MCS.UI.Mappers
{
    public class TransactionLogInfoMapper
    {
        public static List<TransactionLogInfoVM> Map(IList<TransactionLogInfoDTO> transactionLogInfoDTOs)
        {
            if (transactionLogInfoDTOs.Any())
            {
                List<TransactionLogInfoVM> transactionLogInfoVMs = transactionLogInfoDTOs.Select(log =>
                {
                    TransactionLogInfoVM transactionLogInfo = new TransactionLogInfoVM
                    {
                        UserId = log.UserId,
                        UserName = log.UserName,
                        TransactionLogDetails = Map(log.TransactionLogDetails)
                    };
                    return transactionLogInfo;
                }).ToList();
                return transactionLogInfoVMs;
            }
            return new List<TransactionLogInfoVM>();
        }

        public static List<TransactionLogDetailInfoVM> Map(IList<TransactionLogDetailInfoDTO> transactionLogDetailInfoDTOs)
        {
            if (transactionLogDetailInfoDTOs.Any())
            {
                List<TransactionLogDetailInfoVM> transactionLogDetailInfoVMs = transactionLogDetailInfoDTOs.Select(log =>
                {
                    TransactionLogDetailInfoVM transactionLogDetailInfo = new TransactionLogDetailInfoVM
                    {
                        UserId = log.UserId,
                        UserName = log.UserName,
                        Date = log.Date,
                        Description = log.Description
                    };
                    return transactionLogDetailInfo;
                }).ToList();
                return transactionLogDetailInfoVMs;
            }
            return new List<TransactionLogDetailInfoVM>();
        }
    }
}