using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Transaction.Outbound;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.Transaction;


namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public class TransactionBasicInfoMapper
    {
        public static TransactionBasicInfoVM Map(TransactionBasicInfoDTO transactionBasicInfoDTO)
        {
            if (transactionBasicInfoDTO != null)
            {
                TransactionBasicInfoVM transactionBasicInfoVM = new TransactionBasicInfoVM()
                { 
                    ConfidentialityId = transactionBasicInfoDTO.ConfidentialityId,
                    ConfidentialityName = transactionBasicInfoDTO.ConfidentialityName,
                    Date = transactionBasicInfoDTO.Date,
                    DateH = transactionBasicInfoDTO.DateH,
                    DocumentNumber = transactionBasicInfoDTO.DocumentNumber,
                    ExternalPartyId = transactionBasicInfoDTO.ExternalPartyId,
                    ExternalPartyManagerId = transactionBasicInfoDTO.ExternalPartyManagerId,
                    ExternalPartyManagerName = transactionBasicInfoDTO.ExternalPartyManagerName,
                    ExternalPartyName = transactionBasicInfoDTO.ExternalPartyName,
                    Hour = transactionBasicInfoDTO.Hour,
                    IsSigned = transactionBasicInfoDTO.IsSigned,
                    LetterTypeId = transactionBasicInfoDTO.LetterTypeId,
                    LetterTypeName = transactionBasicInfoDTO.LetterTypeName,
                    Minute = transactionBasicInfoDTO.Minute,
                    Number = transactionBasicInfoDTO.Number,
                    OutboundDraftEditorType = transactionBasicInfoDTO.OutboundDraftEditorType,
                    OutboundDraftId = transactionBasicInfoDTO.OutboundDraftId,
                    PriorityId = transactionBasicInfoDTO.PriorityId,
                    PriorityName = transactionBasicInfoDTO.PriorityName,
                    Remarks = transactionBasicInfoDTO.Remarks,
                    RemindDate = transactionBasicInfoDTO.RemindDate,
                    RemindDateH = transactionBasicInfoDTO.RemindDateH,
                    SignedByOrgUnitId = transactionBasicInfoDTO.SignedByOrgUnitId,
                    SignedByOrgUnitName = transactionBasicInfoDTO.SignedByOrgUnitName,
                    SignedByUserId = transactionBasicInfoDTO.SignedByUserId,
                    SignedByUserName = transactionBasicInfoDTO.SignedByUserName,
                    TransactionTypeId = transactionBasicInfoDTO.TransactionTypeId,
                    TransactionTypeName = transactionBasicInfoDTO.TransactionTypeName,
                    Subject = transactionBasicInfoDTO.Subject,
                    SubjectClassifications = transactionBasicInfoDTO.SubjectClassifications,
                    SuggestedTopicId = transactionBasicInfoDTO.SuggestedTopicId,
                    ToEntityName = transactionBasicInfoDTO.ToEntityName,
                    ToUserName = transactionBasicInfoDTO.ToUserName,
                    TransactionCategoryId = transactionBasicInfoDTO.TransactionCategoryId,
                    DeliveryMethod = transactionBasicInfoDTO.DeliveryMethod,
                    DeliveryMethodId = transactionBasicInfoDTO.DeliveryMethodId,
                    POBox = transactionBasicInfoDTO.POBox,
                    PostCode = transactionBasicInfoDTO.PostCode,
                    RecordNumber = transactionBasicInfoDTO.RecordNumber,
                    LetterNumber = transactionBasicInfoDTO.LetterNumber,
                    Attachments = transactionBasicInfoDTO.Attachments != null && transactionBasicInfoDTO.Attachments.Count > 0 ? TransactionAttachmentMapper.Map(transactionBasicInfoDTO.Attachments) : new List<TransactionAttachmentVM>(),
                    Links = transactionBasicInfoDTO.Links != null && transactionBasicInfoDTO.Links.Count > 0 ? TransactionLinkMapper.Map(transactionBasicInfoDTO.Links) : new List<TransactionLinkVM>(),


            };
                return transactionBasicInfoVM;
            }
            return new TransactionBasicInfoVM();
        }
        public static TransactionBasicInfoDTO Map(TransactionBasicInfoVM transactionBasicInfoVM)
        {
            if (transactionBasicInfoVM != null)
            {
                TransactionBasicInfoDTO transactionBasicInfoDTO = new TransactionBasicInfoDTO()
                {
                    ConfidentialityId = transactionBasicInfoVM.ConfidentialityId,
                    ConfidentialityName = transactionBasicInfoVM.ConfidentialityName,
                    Date = transactionBasicInfoVM.Date,
                    DateH = transactionBasicInfoVM.DateH,
                    DocumentNumber = transactionBasicInfoVM.DocumentNumber,
                    ExternalPartyId = transactionBasicInfoVM.ExternalPartyId,
                    ExternalPartyManagerId = transactionBasicInfoVM.ExternalPartyManagerId,
                    ExternalPartyManagerName = transactionBasicInfoVM.ExternalPartyManagerName,
                    ExternalPartyName = transactionBasicInfoVM.ExternalPartyName,
                    Hour = transactionBasicInfoVM.Hour,
                    IsSigned = transactionBasicInfoVM.IsSigned,
                    LetterTypeId = transactionBasicInfoVM.LetterTypeId,
                    LetterTypeName = transactionBasicInfoVM.LetterTypeName,
                    Minute = transactionBasicInfoVM.Minute,
                    Number = transactionBasicInfoVM.Number,
                    OutboundDraftEditorType = transactionBasicInfoVM.OutboundDraftEditorType,
                    OutboundDraftId = transactionBasicInfoVM.OutboundDraftId,
                    PriorityId = transactionBasicInfoVM.PriorityId,
                    PriorityName = transactionBasicInfoVM.PriorityName,
                    Remarks = transactionBasicInfoVM.Remarks,
                    RemindDate = transactionBasicInfoVM.RemindDate,
                    RemindDateH = transactionBasicInfoVM.RemindDateH,
                    SignedByOrgUnitId = transactionBasicInfoVM.SignedByOrgUnitId,
                    SignedByOrgUnitName = transactionBasicInfoVM.SignedByOrgUnitName,
                    SignedByUserId = transactionBasicInfoVM.SignedByUserId,
                    SignedByUserName = transactionBasicInfoVM.SignedByUserName,
                    TransactionTypeId = transactionBasicInfoVM.TransactionTypeId,
                    TransactionTypeName = transactionBasicInfoVM.TransactionTypeName,
                    Subject = transactionBasicInfoVM.Subject,
                    SubjectClassifications = transactionBasicInfoVM.SubjectClassifications,
                    SuggestedTopicId = transactionBasicInfoVM.SuggestedTopicId,
                    ToEntityName = transactionBasicInfoVM.ToEntityName,
                    ToUserName = transactionBasicInfoVM.ToUserName,
                    TransactionCategoryId = transactionBasicInfoVM.TransactionCategoryId,
                    DeliveryMethod = transactionBasicInfoVM.DeliveryMethod,
                    DeliveryMethodId = transactionBasicInfoVM.DeliveryMethodId,
                    PostCode = transactionBasicInfoVM.PostCode,
                    POBox = transactionBasicInfoVM.POBox,
                    LetterNumber = transactionBasicInfoVM.LetterNumber

                };
                return transactionBasicInfoDTO;
            }
            return new TransactionBasicInfoDTO();
        }
    }
}