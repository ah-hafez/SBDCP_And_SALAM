using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public class TransactionCopyMapper
    {
        public static List<TransactionCopyVM> Map(IList<TransactionCopyDTO> transactionCopyDTOs)
        {
            if (transactionCopyDTOs == null || !transactionCopyDTOs.Any())
            {
                return new List<TransactionCopyVM>();
            }
            List<TransactionCopyVM> transactionCopyVMs = transactionCopyDTOs.Select(transactionCopyDTO => new TransactionCopyVM
            {
                Id = transactionCopyDTO.Id,
                Date = transactionCopyDTO.Date,
                DateH = SessionInfo.CultureShortName == "ar" ? transactionCopyDTO.DateH.Replace("PM", "م").Replace("AM", "ص") : transactionCopyDTO.DateH,
                Key = transactionCopyDTO.Key,
                OrgUnitId = transactionCopyDTO.OrgUnitId,
                OrgUnitName = transactionCopyDTO.OrgUnitName != null ? transactionCopyDTO.OrgUnitName : "استقبال الادارة",
                UserId = transactionCopyDTO.UserId.HasValue ? transactionCopyDTO.UserId : -1,
                UserName = transactionCopyDTO.UserName,
                ActionId = transactionCopyDTO.ActionId,
                ActionName = transactionCopyDTO.ActionName,
                ActionTypeId = transactionCopyDTO.ActionTypeId,
                IsSent = transactionCopyDTO.IsSent,
                FromUserId = transactionCopyDTO.FromUserId,
                FromOrgUnitId = transactionCopyDTO.FromOrgUnitId,
                FromOrgUnitName = transactionCopyDTO.FromOrgUnitName,
                FromUserName = transactionCopyDTO.FromUserName,
                Status = transactionCopyDTO.Status,
                SentDate = transactionCopyDTO.SentDate,
                GeneralExplanation = transactionCopyDTO.GeneralExplanation,
                SpecialExplanation = (transactionCopyDTO.UserId == SessionInfo.CurrentUser.Id || transactionCopyDTO.OrgUnitId == SessionInfo.OrgUnitId) || transactionCopyDTO.FromUserId == SessionInfo.CurrentUser.Id ? transactionCopyDTO.SpecialExplanation : "---------",
                SpecialCopy = transactionCopyDTO.SpecialCopy,
                IsBcc = transactionCopyDTO.IsBcc,
                IsOpr = transactionCopyDTO.IsOpr,
                OprEntityId = transactionCopyDTO.OprEntityId,
                OprEntityName = transactionCopyDTO.OprEntityName,
                ViewedOnDateH = SessionInfo.CultureShortName == "ar" ? transactionCopyDTO.ViewedOnDateH?.Replace("PM", "م")?.Replace("AM", "ص") : transactionCopyDTO.ViewedOnDateH,
                ViewedBy = transactionCopyDTO.ViewedBy,


            }).ToList();
            return transactionCopyVMs;
        }
        public static List<TransactionCopyDTO> Map(IList<TransactionCopyVM> transactionCopyVMs)
        {
            if (transactionCopyVMs == null || !transactionCopyVMs.Any())
            {
                return new List<TransactionCopyDTO>();
            }
            List<TransactionCopyDTO> transactionCopyDTOs = transactionCopyVMs.Select(transactionCopyVM => new TransactionCopyDTO
            {
                Id = transactionCopyVM.Id,
                Date = transactionCopyVM.Date,
                DateH = transactionCopyVM.DateH,
                Key = transactionCopyVM.Key,
                OrgUnitId = transactionCopyVM.OrgUnitId,
                OrgUnitName = transactionCopyVM.OrgUnitName,
                UserId = transactionCopyVM.UserId,
                UserName = transactionCopyVM.UserName,
                ActionId = transactionCopyVM.ActionId,
                ActionName = transactionCopyVM.ActionName,
                ActionTypeId = transactionCopyVM.ActionTypeId,
                IsSent = transactionCopyVM.IsSent,
                FromUserId = SessionInfo.CurrentUser.Id,
                FromOrgUnitId = SessionInfo.OrgUnitId,
                SendEmail = transactionCopyVM.SendEmail,
                SentDate = transactionCopyVM.SentDate,
                IsBcc = transactionCopyVM.IsBcc
            }).ToList();
            return transactionCopyDTOs;
        }

    }
}