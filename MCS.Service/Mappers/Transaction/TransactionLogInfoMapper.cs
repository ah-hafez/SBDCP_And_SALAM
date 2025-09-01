using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class TransactionLogInfoMapper
    {
        public static List<TransactionLogInfoDTO> Map(IList<TransactionLogInfo> transactionLogInfos)
        {
            if (transactionLogInfos.Any())
            {
                List<TransactionLogInfoDTO> transactionLogInfoDTOs = transactionLogInfos.Select(log =>
                {
                    TransactionLogInfoDTO transactionLogInfo = new TransactionLogInfoDTO
                    {
                        UserId = log.UserId,
                        UserName = log.UserName,
                        TransactionLogDetails = Map(log.TransactionLogDetails)
                    };
                    return transactionLogInfo;
                }).ToList();
                return transactionLogInfoDTOs;
            }
            return new List<TransactionLogInfoDTO>();
        }

        public static List<TransactionLogDetailInfoDTO> Map(IList<TransactionLogDetailInfo> transactionLogDetailInfos)
        {
            if (transactionLogDetailInfos.Any())
            {
                List<TransactionLogDetailInfoDTO> transactionLogInfoDTOs = transactionLogDetailInfos.Select(log =>
                {
                    TransactionLogDetailInfoDTO transactionLogDetailInfo = new TransactionLogDetailInfoDTO
                    {
                        UserId = log.UserId,
                        UserName = log.UserName,
                        Date = log.Date,
                        Description = log.Description
                    };
                    return transactionLogDetailInfo;
                }).ToList();
                return transactionLogInfoDTOs;
            }
            return new List<TransactionLogDetailInfoDTO>();
        }
    }
}