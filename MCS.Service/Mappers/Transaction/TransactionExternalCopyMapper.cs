using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class TransactionExternalCopyMapper
    {

        public static List<TransactionExternalCopy> Map(List<TransactionExternalCopyDTO> transactionExternalCopyDTOs)
        {
            if (transactionExternalCopyDTOs == null || !transactionExternalCopyDTOs.Any())
            {
                return new List<TransactionExternalCopy>();
            }
            List<TransactionExternalCopy> transactionExternalCopys = transactionExternalCopyDTOs
                .Select(transactionExternalCopyDTO => new TransactionExternalCopy()
                {
                    Id = transactionExternalCopyDTO.Id,
                    EntityId = transactionExternalCopyDTO.OrgUnitId,
                    UserId = transactionExternalCopyDTO?.UserId,
                    FromUserId = transactionExternalCopyDTO.FromUserId,
                    FromEntityId = transactionExternalCopyDTO.FromOrgUnitId,
                    ActionId = transactionExternalCopyDTO.ActionId,
                    Date = DateTime.Now,
                    DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                    SendEmail = transactionExternalCopyDTO.SendEmail,
                    ExternalPartyAttachment = ExternalPartyAttachmentMapper.Map(transactionExternalCopyDTO.externalPartyAttachmentDTOs),

                }).ToList();
            return transactionExternalCopys;
        }

        public static List<TransactionExternalCopyDTO> Map(IList<TransactionExternalCopy> transactionExternalCopys)
        {
            if (transactionExternalCopys == null || !transactionExternalCopys.Any())
            {
                return new List<TransactionExternalCopyDTO>();
            }
            List<TransactionExternalCopyDTO> transactionExternalCopyDTOs = transactionExternalCopys
                .Select(transactionExternalCopy => new TransactionExternalCopyDTO()
                {
                    Id = transactionExternalCopy.Id,
                    Key = transactionExternalCopy.Id,
                    OrgUnitId = transactionExternalCopy.Entity.Id,
                    OrgUnitName = transactionExternalCopy.Entity.LocalName,
                    FromUserId = transactionExternalCopy.FromUser?.Id,
                    FromOrgUnitId = transactionExternalCopy.FromEntity != null ? transactionExternalCopy.FromEntity.Id : 0,
                    FromUserName = transactionExternalCopy.FromUser?.LocalName,
                    FromOrgUnitName = transactionExternalCopy.FromEntity?.LocalName,
                    UserId = transactionExternalCopy.User?.Id,
                    UserName = transactionExternalCopy.User?.LocalName,
                    ActionName = transactionExternalCopy.Action.LocalName,
                    ActionId = transactionExternalCopy.Action.Id,
                    externalPartyAttachmentDTOs = ExternalPartyAttachmentMapper.Map(transactionExternalCopy.ExternalPartyAttachment),
                    Date = transactionExternalCopy.Date,
                    DateH = transactionExternalCopy.DateH+" "+ transactionExternalCopy.Date.ToShortTimeString(),
                    Status = transactionExternalCopy.Status,
                    SendEmail = transactionExternalCopy.SendEmail,
                    TransactionId = transactionExternalCopy.TransactionId
                }).ToList();

            return transactionExternalCopyDTOs;
        }

    }
}
