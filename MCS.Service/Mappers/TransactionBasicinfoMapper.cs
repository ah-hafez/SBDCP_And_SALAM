using System;
using System.Collections.Generic;
using MCS.Business;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class TransactionBasicInfoMapper
    {
        public static TransactionBasicInfoDTO Map(TransactionBasicInfo transactionBasicinfo)
        {
            if (transactionBasicinfo == null)
            {
                return null;
            }
            TransactionBasicInfoDTO transactionBasicInfoDTO = new TransactionBasicInfoDTO()
            {
                Id = transactionBasicinfo.Id,
                Date = transactionBasicinfo.Date,
                DateH = transactionBasicinfo.DateH,
                Number = transactionBasicinfo.Number,
                DocumentNumber = transactionBasicinfo.DocumentNumber,
                Remarks = transactionBasicinfo.Remarks,
                Subject = transactionBasicinfo.Subject,
                SignedByUserName = transactionBasicinfo.SignedByUserName,
                SignedByUserId = transactionBasicinfo.SignedByUserId,
                SignedByOrgUnitName = transactionBasicinfo.SignedByOrgUnitName,
                SignedByOrgUnitId = transactionBasicinfo.SignedByOrgUnitId,
                ToEntityName = transactionBasicinfo.ToEntityName,
                ToUserName = transactionBasicinfo.ToUserName,
                PriorityName = transactionBasicinfo.PriorityName,
                PriorityId = transactionBasicinfo.PriorityId,
                ConfidentialityName = transactionBasicinfo.ConfidentialityName,
                ConfidentialityId = transactionBasicinfo.ConfidentialityId,
                TransactionTypeName = transactionBasicinfo.TransactionTypeName,
                TransactionTypeId = transactionBasicinfo.TransactionTypeId,
                LetterTypeName = transactionBasicinfo.LetterTypeName,
                LetterTypeId = transactionBasicinfo.LetterTypeId,
                ExternalPartyName = transactionBasicinfo.ExternalPartyName,
                ExternalPartyId = transactionBasicinfo.ExternalPartyId,
                ExternalPartyManagerName = transactionBasicinfo.ExternalPartyManagerName,
                ExternalPartyManagerId = transactionBasicinfo.ExternalPartyManagerId,
                TransactionCategoryId = transactionBasicinfo.TransactionCategoryId,
                OutboundDraftId = transactionBasicinfo.OutboundDraftId,
                SuggestedTopicId = transactionBasicinfo.SuggestedTopicId,
                IsSigned = transactionBasicinfo.IsSigned,
                OutboundDraftEditorType = transactionBasicinfo.OutboundDraftEditorType,
                DeliveryMethod = transactionBasicinfo.DeliveryMethod,
                DeliveryMethodId = transactionBasicinfo.DeliveryMethodId,
                PostCode = transactionBasicinfo.PostCode,
                POBox = transactionBasicinfo.POBox,
                StatusName = transactionBasicinfo.StatusName,
                YearH = transactionBasicinfo.YearH,
                Links = TransactionLinkMapper.Map(transactionBasicinfo.Links),
                Attachments = TransactionAttachmentMapper.Map(transactionBasicinfo.Attachments)

        };

            if (transactionBasicinfo.SubjectClassifications != null && transactionBasicinfo.SubjectClassifications.Count > 0)
            {
                transactionBasicInfoDTO.SubjectClassifications = new List<int>();
                transactionBasicInfoDTO.SubjectClassifications = transactionBasicinfo.SubjectClassifications;
            }

            if (transactionBasicinfo.RemindDate.HasValue)
            {
                transactionBasicInfoDTO.RemindDateH = transactionBasicinfo.RemindDateH;
                transactionBasicInfoDTO.RemindDate = transactionBasicinfo.RemindDate.Value;
                transactionBasicInfoDTO.Hour = transactionBasicinfo.RemindDate.Value.Hour;
                transactionBasicInfoDTO.Minute = transactionBasicinfo.RemindDate.Value.Minute;
            }

            return transactionBasicInfoDTO;
        }

        public static TransactionBasicInfo Map(TransactionBasicInfoDTO transactionBasicInfoDTO)
        {
            if (transactionBasicInfoDTO == null)
            {
                return null;
            }
            TransactionBasicInfo transactionBasicInfo = new TransactionBasicInfo()
            {
                Id = transactionBasicInfoDTO.Id,
                Remarks = transactionBasicInfoDTO.Remarks,
                Subject = transactionBasicInfoDTO.Subject,
                SignedByUserId = transactionBasicInfoDTO.SignedByUserId,
                SignedByOrgUnitId = transactionBasicInfoDTO.SignedByOrgUnitId,
                PriorityId = transactionBasicInfoDTO.PriorityId,
                ConfidentialityId = transactionBasicInfoDTO.ConfidentialityId,
                TransactionTypeId = transactionBasicInfoDTO.TransactionTypeId,
                ExternalPartyId = transactionBasicInfoDTO.ExternalPartyId,
                ExternalPartyManagerId = transactionBasicInfoDTO.ExternalPartyManagerId,
                TransactionCategoryId = transactionBasicInfoDTO.TransactionCategoryId,
                LetterTypeId = transactionBasicInfoDTO.LetterTypeId,
                SuggestedTopicId = transactionBasicInfoDTO.SuggestedTopicId,
                DeliveryMethod = transactionBasicInfoDTO.DeliveryMethod,
                DeliveryMethodId = transactionBasicInfoDTO.DeliveryMethodId,
                POBox = transactionBasicInfoDTO.POBox,
                PostCode = transactionBasicInfoDTO.PostCode,
                StatusName = transactionBasicInfoDTO.StatusName

            };

            if (transactionBasicInfoDTO.SubjectClassifications != null && transactionBasicInfoDTO.SubjectClassifications.Count > 0)
            {
                transactionBasicInfo.SubjectClassifications = new List<int>();
                transactionBasicInfo.SubjectClassifications = transactionBasicInfoDTO.SubjectClassifications;
            }

            if (transactionBasicInfoDTO.RemindDate.HasValue)
            {
                TimeSpan ts = new TimeSpan(transactionBasicInfoDTO.Hour.HasValue ? transactionBasicInfoDTO.Hour.Value : 00, transactionBasicInfoDTO.Minute.HasValue ? transactionBasicInfoDTO.Minute.Value : 00, 00);
                transactionBasicInfo.RemindDate = transactionBasicInfoDTO.RemindDate.Value + ts;
                transactionBasicInfo.RemindDateH = transactionBasicInfoDTO.RemindDateH;
            }

            return transactionBasicInfo;
        }

    }
}