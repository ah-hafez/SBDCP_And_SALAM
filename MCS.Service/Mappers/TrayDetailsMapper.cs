using System.Collections.Generic;
using System.Linq;
using MCS.Business;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class TrayDetailsMapper
    {
        public static List<TransactionAssignmentInfoDTO> Map(IList<TransactionAssignmentInfo> transactionAssignmentInfos)
        {
            if (transactionAssignmentInfos == null || !transactionAssignmentInfos.Any())
            {
                return null;
            }

            List<TransactionAssignmentInfoDTO> transactionAssignmentInfoDTOs = transactionAssignmentInfos
                .Select(transactionAssignmentInfo => new TransactionAssignmentInfoDTO()
                {
                    FromUserId = transactionAssignmentInfo.FromUserId,
                    Action = transactionAssignmentInfo.Action,
                    ActionId = transactionAssignmentInfo.ActionId,
                    Date = transactionAssignmentInfo.Date,
                    DateH = transactionAssignmentInfo.DateH,
                    FromEntity = transactionAssignmentInfo.FromEntity,
                    FromEntityId = transactionAssignmentInfo.FromEntityId,
                    FromUser = transactionAssignmentInfo.FromUser,
                    ToEntity = transactionAssignmentInfo.ToEntity,
                    ToEntityId = transactionAssignmentInfo.ToEntityId,
                    ToUser = transactionAssignmentInfo.ToUser,
                    ToUserId = transactionAssignmentInfo.ToUserId,
                    Id = transactionAssignmentInfo.Id,
                    HasCollaboration = transactionAssignmentInfo.HasCollaboration,
                    IsLate = transactionAssignmentInfo.IsLate,
                    Viewed = transactionAssignmentInfo.Viewed,
                    Description = transactionAssignmentInfo.Description,
                }).ToList();


            return transactionAssignmentInfoDTOs;
        }


        public static List<TransactionAssignmentInfo> Map(IList<TransactionAssignmentInfoDTO> transactionAssignmentInfoDTOs)
        {
            if (transactionAssignmentInfoDTOs == null || !transactionAssignmentInfoDTOs.Any())
            {
                return null;
            }
            List<TransactionAssignmentInfo> transactionAssignmentInfos = transactionAssignmentInfoDTOs
                .Select(transactionAssignmentInfoDTO => new TransactionAssignmentInfo()
                {
                    FromUserId = transactionAssignmentInfoDTO.FromUserId,
                    Action = transactionAssignmentInfoDTO.Action,
                    ActionId = transactionAssignmentInfoDTO.ActionId,
                    Date = transactionAssignmentInfoDTO.Date,
                    DateH = transactionAssignmentInfoDTO.DateH,
                    FromEntity = transactionAssignmentInfoDTO.FromEntity,
                    FromEntityId = transactionAssignmentInfoDTO.FromEntityId,
                    FromUser = transactionAssignmentInfoDTO.FromUser,
                    ToEntity = transactionAssignmentInfoDTO.ToEntity,
                    ToEntityId = transactionAssignmentInfoDTO.ToEntityId,
                    ToUser = transactionAssignmentInfoDTO.ToUser,
                    ToUserId = transactionAssignmentInfoDTO.ToUserId,
                    Id = transactionAssignmentInfoDTO.Id,
                    HasCollaboration = transactionAssignmentInfoDTO.HasCollaboration,
                    IsLate = transactionAssignmentInfoDTO.IsLate,
                    Description = transactionAssignmentInfoDTO.Description,
                }).ToList();
            return transactionAssignmentInfos;
        }

        public static TransactionDetailsInfoDTO Map(TransactionDetailsInfo transactionDetailsInfo)
        {
            if (transactionDetailsInfo == null)
            {
                return null;
            }
            TransactionDetailsInfoDTO transactionDetailsInfoDTO = new TransactionDetailsInfoDTO()
            {
                Id = transactionDetailsInfo.Id,
                Date = transactionDetailsInfo.Date,
                DateH = transactionDetailsInfo.DateH,
                Number = transactionDetailsInfo.Number,
                Remarks = transactionDetailsInfo.Remarks,
                RemindDate = transactionDetailsInfo.RemindDate,
                RemindDateH = transactionDetailsInfo.RemindDateH,
                Subject = transactionDetailsInfo.Subject,
                DocumentNumber = transactionDetailsInfo.DocumentNumber,
                TransactionCategoryId = transactionDetailsInfo.TransactionCategoryId,
                ConfidentialityName = transactionDetailsInfo.ConfidentialityName,
                ConfidentialityId = transactionDetailsInfo.ConfidentialityId,
                ExternalPartyName = transactionDetailsInfo.ExternalPartyName,
                ExternalPartyId = transactionDetailsInfo.ExternalPartyId,
                ExternalPartyManagerName = transactionDetailsInfo.ExternalPartyManagerName,
                ExternalPartyManagerId = transactionDetailsInfo.ExternalPartyManagerId,
                LetterTypeName = transactionDetailsInfo.LetterTypeName,
                PriorityName = transactionDetailsInfo.PriorityName,
                PriorityId = transactionDetailsInfo.PriorityId,
                SignedByOrgUnitName = transactionDetailsInfo.SignedByOrgUnitName,
                SignedByOrgUnitId = transactionDetailsInfo.SignedByOrgUnitId,
                SignedByUserName = transactionDetailsInfo.SignedByUserName,
                SignedByUserId = transactionDetailsInfo.SignedByUserId,
                TransactionTypeName = transactionDetailsInfo.TransactionTypeName,
                TransactionTypeId = transactionDetailsInfo.TransactionTypeId,
                TransactionTypeColorId = transactionDetailsInfo.TransactionTypeColorId,
                ToEntityName = transactionDetailsInfo.ToEntityName,
                ToUserName = transactionDetailsInfo.ToUserName,
                TransactionCategory = transactionDetailsInfo.TransactionCategory,
                EntityName = transactionDetailsInfo.EntityName,
                User = transactionDetailsInfo.User,
                Status = transactionDetailsInfo.Status,
                StatusId = transactionDetailsInfo.StatusId,
                RejectionReason = transactionDetailsInfo.RejectionReason,
                UserId = transactionDetailsInfo.UserId,
                ToUserId = transactionDetailsInfo.ToUserId,
                IsLate = transactionDetailsInfo.IsLate,
                AttachmentCount = transactionDetailsInfo.AttachmentCount,
                HasPermission = transactionDetailsInfo.HasPermission,
                SavedReason = transactionDetailsInfo.SavedReason,
                DeliveryMethodId = transactionDetailsInfo.DeliveryMethodId,
                TransactionPathId = transactionDetailsInfo.TransactionPathId,
                IsIndividual = transactionDetailsInfo.IsIndividual,
                DeliveryMethodName = transactionDetailsInfo.DeliveryMethodName,
                FollowupDate = transactionDetailsInfo.FollowupDate,
                FollowupDateH = transactionDetailsInfo.FollowupDateH,
                HasLinks = transactionDetailsInfo.HasLinks,
                YasserRegistered = transactionDetailsInfo.YesserRegistered,
                PrivecyName = transactionDetailsInfo.PrivecyName,
                PrivecyId = transactionDetailsInfo.PrivecyId,
                isDeleted = transactionDetailsInfo.isDeleted,
                IsPresentationDraft = transactionDetailsInfo.IsPresentationDraft,
                IsElcOutBound = transactionDetailsInfo.IsElcOutBound,
                SpecialCopy = transactionDetailsInfo.SpecialCopy,
                IsOpr = transactionDetailsInfo.IsOpr,
                IsBcc = transactionDetailsInfo.IsBcc,
                OprEntityId = transactionDetailsInfo.OprEntityId,
                OprEntityName = transactionDetailsInfo.OprEntityName,
                IsImportant = transactionDetailsInfo.IsImportant,
                TransactionCopyId = transactionDetailsInfo.TransactionCopyId,
                HasTask = transactionDetailsInfo.HasTask,
                Encrypted = transactionDetailsInfo.Encrypted,

            };

            return transactionDetailsInfoDTO;
        }

        public static TransactionDetailsInfo Map(TransactionDetailsInfoDTO transactionDetailsDTO)
        {
            if (transactionDetailsDTO == null)
            {
                return null;
            }
            TransactionDetailsInfo transactionDetailsInfo = new TransactionDetailsInfo()
            {
                Id = transactionDetailsDTO.Id,
                Date = transactionDetailsDTO.Date,
                DateH = transactionDetailsDTO.DateH,
                Number = transactionDetailsDTO.Number,
                Remarks = transactionDetailsDTO.Remarks,
                RemindDate = transactionDetailsDTO.RemindDate,
                RemindDateH = transactionDetailsDTO.RemindDateH,
                Subject = transactionDetailsDTO.Subject,
                DocumentNumber = transactionDetailsDTO.DocumentNumber,
                TransactionCategoryId = transactionDetailsDTO.TransactionCategoryId,
                ConfidentialityName = transactionDetailsDTO.ConfidentialityName,
                ConfidentialityId = transactionDetailsDTO.ConfidentialityId,
                ExternalPartyName = transactionDetailsDTO.ExternalPartyName,
                ExternalPartyId = transactionDetailsDTO.ExternalPartyId,
                ExternalPartyManagerName = transactionDetailsDTO.ExternalPartyManagerName,
                ExternalPartyManagerId = transactionDetailsDTO.ExternalPartyManagerId,
                LetterTypeName = transactionDetailsDTO.LetterTypeName,
                PriorityName = transactionDetailsDTO.PriorityName,
                PriorityId = transactionDetailsDTO.PriorityId,
                SignedByOrgUnitName = transactionDetailsDTO.SignedByOrgUnitName,
                SignedByOrgUnitId = transactionDetailsDTO.SignedByOrgUnitId,
                SignedByUserName = transactionDetailsDTO.SignedByUserName,
                SignedByUserId = transactionDetailsDTO.SignedByUserId,
                TransactionTypeName = transactionDetailsDTO.TransactionTypeName,
                TransactionTypeId = transactionDetailsDTO.TransactionTypeId,
                TransactionTypeColorId = transactionDetailsDTO.TransactionTypeColorId,
                ToEntityName = transactionDetailsDTO.ToEntityName,
                ToUserName = transactionDetailsDTO.ToUserName,
                TransactionCategory = transactionDetailsDTO.TransactionCategory,
                EntityName = transactionDetailsDTO.EntityName,
                User = transactionDetailsDTO.User,
                Status = transactionDetailsDTO.Status,
                UserId = transactionDetailsDTO.UserId,
                IsLate = transactionDetailsDTO.IsLate,
                HasLinks = transactionDetailsDTO.HasLinks,
            };

            return transactionDetailsInfo;
        }
        public static List<TransactionTrayInfoDTO> Map(IList<TransactionTrayInfo> transactionTrayInfos)
        {
            if (transactionTrayInfos == null || !transactionTrayInfos.Any())
            {
                return null;
            }
            List<TransactionTrayInfoDTO> transactionAssignmentInfoDTOs = transactionTrayInfos
                .Select(transactionTrayInfo => new TransactionTrayInfoDTO()
                {
                    TransactionDetailsInfoDTOs = Map(transactionTrayInfo.transactionDetailsInfo),
                    TransactionAssignmentInfoDTOs = Map(transactionTrayInfo.TransactionAssignmentInfos),
                }).ToList();

            return transactionAssignmentInfoDTOs;
        }

        public static List<TransactionTrayInfo> Map(IList<TransactionTrayInfoDTO> transactionTrayInfoDTOs)
        {
            if (transactionTrayInfoDTOs == null || !transactionTrayInfoDTOs.Any())
            {
                return null;
            }
            List<TransactionTrayInfo> transactionTrayInfos = transactionTrayInfoDTOs
                .Select(transactionTrayInfoDTO => new TransactionTrayInfo()
                {
                    transactionDetailsInfo = Map(transactionTrayInfoDTO.TransactionDetailsInfoDTOs),
                    TransactionAssignmentInfos = Map(transactionTrayInfoDTO.TransactionAssignmentInfoDTOs),
                }).ToList();
            return transactionTrayInfos;
        }

        public static TrayDetailsDTO Map(TrayDetailsInfo TrayDetailsInfo)
        {
            TrayDetailsDTO trayDetailsInfoDTO = new TrayDetailsDTO()
            {
                Id = TrayDetailsInfo.Id,
                Name = TrayDetailsInfo.Name,
                AllTransactionCount = TrayDetailsInfo.AllTransactionCount,
                TodayTransactionCount = TrayDetailsInfo.TodayTransactionCount,
            };

            if (TrayDetailsInfo.TransactionTraysInfo != null)
            {
                trayDetailsInfoDTO.TransactionTrayInfoDTOs = Map(TrayDetailsInfo.TransactionTraysInfo);
            }

            return trayDetailsInfoDTO;
        }

        public static List<TrayDetailsDTO> Map(IList<TrayDetailsInfo> traysDetails)
        {
            if (traysDetails == null || !traysDetails.Any())
            {
                return null;
            }
            List<TrayDetailsDTO> traysDetailsDTO = traysDetails
                .Select(trayDetails => new TrayDetailsDTO()
                {
                    Id = trayDetails.Id,
                    Name = trayDetails.Name,
                    AllTransactionCount = trayDetails.AllTransactionCount,
                    TodayTransactionCount = trayDetails.TodayTransactionCount,
                    TransactionTrayInfoDTOs = Map(trayDetails?.TransactionTraysInfo)

                }).ToList();

            return traysDetailsDTO;
        }

        public static List<TrayDetailsInfo> Map(IList<TrayDetailsDTO> trayDetailsDTOs)
        {
            if (trayDetailsDTOs == null || !trayDetailsDTOs.Any())
            {
                return null;
            }
            List<TrayDetailsInfo> trayDetailsInfos = trayDetailsDTOs
                .Select(trayDetailsDTO => new TrayDetailsInfo()
                {
                    Id = trayDetailsDTO.Id,
                    Name = trayDetailsDTO.Name,
                    AllTransactionCount = trayDetailsDTO.AllTransactionCount,
                    TodayTransactionCount = trayDetailsDTO.TodayTransactionCount,
                    TransactionTraysInfo = Map(trayDetailsDTO?.TransactionTrayInfoDTOs)
                }).ToList();
            return trayDetailsInfos;
        }
    }
}