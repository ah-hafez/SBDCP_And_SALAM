using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;
using MCS.DTO.Transaction.Vip;

namespace MCS.Service.Mappers
{
    public class TransactionCopyMapper
    {

        public static List<TransactionCopy> Map(List<TransactionCopyDTO> transactionCopyDTOs)
        {
            if (transactionCopyDTOs == null || !transactionCopyDTOs.Any())
            {
                return new List<TransactionCopy>();
            }
            List<TransactionCopy> transactionCopys = transactionCopyDTOs
                .Select(transactionCopyDTO => new TransactionCopy()
                {
                    Id = transactionCopyDTO.Id,
                    EntityId = transactionCopyDTO.OrgUnitId,
                    UserId = transactionCopyDTO?.UserId > 0 ? transactionCopyDTO.UserId : null,
                    FromUserId = transactionCopyDTO.FromUserId,
                    FromEntityId = transactionCopyDTO.FromOrgUnitId,
                    ActionId = transactionCopyDTO.ActionId,
                    Date = DateTime.Now,
                    DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                    IsSent = transactionCopyDTO.IsSent,
                    Status = TransCopyStatus.NotViewed.LookupIdentity(LookupCategory.TransCopyStatus, string.Empty),
                    SendEmail = transactionCopyDTO.SendEmail,
                    SpecialExplanation = transactionCopyDTO.SpecialExplanation,
                    GeneralExplanation = transactionCopyDTO.GeneralExplanation,
                    SpecialCopy = transactionCopyDTO.SpecialCopy,
                    IsOpr = transactionCopyDTO.IsOpr,
                    IsBcc = transactionCopyDTO.IsBcc,
                    OprEntityId = transactionCopyDTO.OprEntityId
                }).ToList();
            return transactionCopys;
        }

        public static List<TransactionCopyDTO> Map(IList<TransactionCopy> transactionCopys)
        {
            if (transactionCopys == null || !transactionCopys.Any())
            {
                return new List<TransactionCopyDTO>();
            }
            List<TransactionCopyDTO> transactionCopyDTOs = transactionCopys.Select(transactionCopy => new TransactionCopyDTO()
            {
                Id = transactionCopy.Id,
                Key = transactionCopy.Id,
                OrgUnitId = transactionCopy.Entity.Id,
                OrgUnitName = transactionCopy.Entity.LocalName,
                FromUserId = transactionCopy.FromUserId,
                FromOrgUnitId = transactionCopy.FromEntity != null ? transactionCopy.FromEntity.Id : 0,
                FromUserName = transactionCopy.FromUser?.LocalName,
                FromOrgUnitName = transactionCopy.FromEntity?.LocalName,
                UserId = transactionCopy.User?.Id,
                UserName = transactionCopy.User?.LocalName,
                ActionName = transactionCopy.Action.LocalName,
                ActionId = transactionCopy.Action.Id,
                IsSent = transactionCopy.IsSent,
                Date = transactionCopy.Date,
                DateH = transactionCopy.DateH + " " + transactionCopy.Date.ToShortTimeString(),
                Status = transactionCopy.Status,
                SentDate = transactionCopy.SentDate,
                SpecialExplanation = transactionCopy.SpecialExplanation,
                GeneralExplanation = transactionCopy.GeneralExplanation,
                IsOpr = transactionCopy.IsOpr,
                IsBcc = transactionCopy.IsBcc,
                SpecialCopy = transactionCopy.SpecialCopy,
                OprEntityId = transactionCopy.OprEntityId,
                OprEntityName = transactionCopy.OprEntity?.LocalName,
                ViewedOnDateH = transactionCopy?.ViewedOnDateH,
                ViewedBy = transactionCopy?.ViewedBy?.LocalName ?? transactionCopy.User?.LocalName



            }).ToList();

            return transactionCopyDTOs;
        }

        public static List<TransactionCopy> Map(List<VIPTransactionAssignmentDto> transactionCopyDTOs)
        {
            if (transactionCopyDTOs == null || !transactionCopyDTOs.Any())
            {
                return new List<TransactionCopy>();
            }
            var notViewedStatus = TransCopyStatus.NotViewed.LookupIdentity(LookupCategory.TransCopyStatus, string.Empty);
            List<TransactionCopy> transactionCopys = transactionCopyDTOs
                .Select(transactionCopyDTO => new TransactionCopy()
                {
                    Id = transactionCopyDTO.Id,
                    EntityId = transactionCopyDTO.ToOrgUnitId,
                    UserId = transactionCopyDTO?.ToUserId > 0 ? transactionCopyDTO.ToUserId : null,
                    FromUserId = transactionCopyDTO.FromUserId,
                    FromEntityId = transactionCopyDTO.FromEntityId,
                    ActionId = transactionCopyDTO.ActionId,
                    Date = DateTime.Now,
                    DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                    IsSent = 1,
                    SentDate = DateTime.Now,
                    Status = notViewedStatus,
                    SendEmail = false,
                    SpecialExplanation = transactionCopyDTO.SpecialExplanation,
                    GeneralExplanation = transactionCopyDTO.GeneralExplanation,
                    Viewed = false,
                    TransactionId = transactionCopyDTO.TransactionId,
                    IsBcc = transactionCopyDTO.IsBcc,


                }).ToList();
            return transactionCopys;
        }

    }
}
