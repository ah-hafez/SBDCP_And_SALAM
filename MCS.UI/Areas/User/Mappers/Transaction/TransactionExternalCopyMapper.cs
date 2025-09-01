using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.ExternalParties;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public class TransactionExternalCopyMapper
    {
        public static List<TransactionExternalCopyVM> Map(IList<TransactionExternalCopyDTO> transactionExternalCopyDTOs)
        {
            if (transactionExternalCopyDTOs == null || !transactionExternalCopyDTOs.Any())
            {
                return new List<TransactionExternalCopyVM>();
            }
            List<TransactionExternalCopyVM> transactionExternalCopyVMs = transactionExternalCopyDTOs
                .Select(transactionExternalCopyDTO => new TransactionExternalCopyVM
                {

                    AttachmentCount = (transactionExternalCopyDTO.externalPartyAttachmentDTOs) != null ? transactionExternalCopyDTO.externalPartyAttachmentDTOs.Count : 0,
                    Id = transactionExternalCopyDTO.Id,
                    Date = transactionExternalCopyDTO.Date,
                    DateH = SessionInfo.CultureShortName == "ar" ? transactionExternalCopyDTO.DateH.Replace("PM", "م").Replace("AM", "ص") : transactionExternalCopyDTO.DateH,
                    Key = transactionExternalCopyDTO.Key,
                    OrgUnitId = transactionExternalCopyDTO.OrgUnitId,
                    OrgUnitName = transactionExternalCopyDTO.OrgUnitName,
                    UserId = transactionExternalCopyDTO.UserId,
                    UserName = transactionExternalCopyDTO.UserName,
                    ActionId = transactionExternalCopyDTO.ActionId,
                    ActionName = transactionExternalCopyDTO.ActionName,
                    ActionTypeId = transactionExternalCopyDTO.ActionTypeId,
                    externalPartyAttachmentVMs = ExternalPartyAttachmentMapper.Map(transactionExternalCopyDTO.externalPartyAttachmentDTOs),
                    attachmentNames = (transactionExternalCopyDTO.externalPartyAttachmentDTOs) != null ? ExternalPartyAttachmentMapper.getAttachmentNames(transactionExternalCopyDTO.externalPartyAttachmentDTOs) : string.Empty,
                    Status = transactionExternalCopyDTO.Status,
                    SendEmail = transactionExternalCopyDTO.SendEmail,
                    TransactionId = transactionExternalCopyDTO.TransactionId,
                    FromUserId = transactionExternalCopyDTO.FromUserId,
                    FromOrgUnitId = transactionExternalCopyDTO.FromOrgUnitId,
                    FromOrgUnitName = transactionExternalCopyDTO.FromOrgUnitName,
                    FromUserName = transactionExternalCopyDTO.FromUserName
                }).ToList();
            return transactionExternalCopyVMs;
        }
        public static TransactionExternalCopyVM Map(TransactionExternalCopyDTO transactionExternalCopyDTO)
        {
            if (transactionExternalCopyDTO != null)
            {

                TransactionExternalCopyVM transactionExternalCopyVMs = new TransactionExternalCopyVM
                {
                    AttachmentCount = transactionExternalCopyDTO.externalPartyAttachmentDTOs.Count,
                    Id = transactionExternalCopyDTO.Id,
                    Date = transactionExternalCopyDTO.Date,
                    DateH = transactionExternalCopyDTO.DateH,
                    Key = transactionExternalCopyDTO.Key,
                    OrgUnitId = transactionExternalCopyDTO.OrgUnitId,
                    OrgUnitName = transactionExternalCopyDTO.OrgUnitName,
                    UserId = transactionExternalCopyDTO.UserId,
                    UserName = transactionExternalCopyDTO.UserName,
                    ActionId = transactionExternalCopyDTO.ActionId,
                    ActionName = transactionExternalCopyDTO.ActionName,
                    ActionTypeId = transactionExternalCopyDTO.ActionTypeId,
                    FromUserId = transactionExternalCopyDTO.FromUserId,
                    FromOrgUnitId = transactionExternalCopyDTO.FromOrgUnitId,
                    FromOrgUnitName = transactionExternalCopyDTO.FromOrgUnitName,
                    FromUserName = transactionExternalCopyDTO.FromUserName,
                    SendEmail = transactionExternalCopyDTO.SendEmail,
                    externalPartyAttachmentVMs = ExternalPartyAttachmentMapper.Map(transactionExternalCopyDTO.externalPartyAttachmentDTOs),
                    attachmentNames = ExternalPartyAttachmentMapper.getAttachmentNames(transactionExternalCopyDTO.externalPartyAttachmentDTOs)
                };
                return transactionExternalCopyVMs;
            }
            return new TransactionExternalCopyVM();
        }
        public static List<TransactionExternalCopyDTO> Map(IList<TransactionExternalCopyVM> transactionExternalCopyVMs)
        {
            if (transactionExternalCopyVMs == null || !transactionExternalCopyVMs.Any())
            {
                return new List<TransactionExternalCopyDTO>();
            }
            List<TransactionExternalCopyDTO> transactionExternalCopyDTOs = transactionExternalCopyVMs.Select(transactionExternalCopyVM =>
            {
                if (transactionExternalCopyVM.attachmentNames == null)
                {
                    transactionExternalCopyVM.attachmentNames = "";
                }
                List<ExternalPartyAttachmentVM> externalPartyAttachmentVM = JsonConvert.DeserializeObject<List<ExternalPartyAttachmentVM>>(transactionExternalCopyVM.attachmentNames);

                if (transactionExternalCopyVM.externalPartyAttachmentVMs == null && externalPartyAttachmentVM != null)
                {
                    transactionExternalCopyVM.externalPartyAttachmentVMs = externalPartyAttachmentVM.Where(e => e.IsDeleted == true).ToList();
                }
                else if (externalPartyAttachmentVM != null)
                {
                    transactionExternalCopyVM.externalPartyAttachmentVMs.AddRange(externalPartyAttachmentVM.Where(e => e.IsDeleted == true).ToList());
                }

                TransactionExternalCopyDTO transactionExternalCopyDTO = new TransactionExternalCopyDTO
                {
                    Id = transactionExternalCopyVM.Id,
                    Date = transactionExternalCopyVM.Date,
                    DateH = transactionExternalCopyVM.DateH,
                    Key = transactionExternalCopyVM.Key,
                    OrgUnitId = transactionExternalCopyVM.OrgUnitId,
                    OrgUnitName = transactionExternalCopyVM.OrgUnitName,
                    UserId = transactionExternalCopyVM.UserId,
                    UserName = transactionExternalCopyVM.UserName,
                    ActionId = transactionExternalCopyVM.ActionId,
                    ActionName = transactionExternalCopyVM.ActionName,
                    ActionTypeId = transactionExternalCopyVM.ActionTypeId,
                    FromUserId = SessionInfo.CurrentUser.Id,
                    FromOrgUnitId = SessionInfo.OrgUnitId,
                    SendEmail = transactionExternalCopyVM.SendEmail,
                    externalPartyAttachmentDTOs = ExternalPartyAttachmentMapper.Map(transactionExternalCopyVM.externalPartyAttachmentVMs)
                };

                return transactionExternalCopyDTO;
            }).ToList();
            return transactionExternalCopyDTOs;
        }

    }
}