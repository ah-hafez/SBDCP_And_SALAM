using System;
using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction.Outbound.External;

namespace MCS.UI.Areas.User.Mappers.Transaction.OutBound.External
{
    public class OutboundExternalBasicInfoMapper
    {
        public static AddOutboundExternalBasicInfoVM Map(AddOutboundExternalBasicInfoDTO addOutboundExternalBasicInfoDTO)
        {
            if (addOutboundExternalBasicInfoDTO != null)
            {
                return new AddOutboundExternalBasicInfoVM()
                {
                    ConfidentialityLevelId = addOutboundExternalBasicInfoDTO.ConfidentialityLevelId,
                    DestinationId = addOutboundExternalBasicInfoDTO.DestinationId,
                    ExternalPartyId = addOutboundExternalBasicInfoDTO.ExternalPartyId,
                    DirectedToId = addOutboundExternalBasicInfoDTO.DirectedToId,
                    Hour = addOutboundExternalBasicInfoDTO.Hour,
                    Minute = addOutboundExternalBasicInfoDTO.Minute,
                    OutboundNumber = addOutboundExternalBasicInfoDTO.OutboundNumber,
                    PreparationEntityId = addOutboundExternalBasicInfoDTO.PreparationEntityId,
                    PriorityLevelId = addOutboundExternalBasicInfoDTO.PriorityLevelId,
                    Remarks = addOutboundExternalBasicInfoDTO.Remarks,
                    RemindDate = addOutboundExternalBasicInfoDTO.RemindDate != null ? DateTime.Parse(addOutboundExternalBasicInfoDTO.RemindDate.Value.ToShortDateString()) : DateTime.Now,
                    RemindDateH = addOutboundExternalBasicInfoDTO.RemindDateH,
                    SignedById = addOutboundExternalBasicInfoDTO.SignedById,
                    TransactionTypeId = addOutboundExternalBasicInfoDTO.TransactionTypeId,
                    Subject = addOutboundExternalBasicInfoDTO.Subject,
                    SubjectClassifications = addOutboundExternalBasicInfoDTO.SubjectClassifications,
                    SuggestedTopicId = addOutboundExternalBasicInfoDTO.SuggestedTopicId,
                    LetterTypeId = addOutboundExternalBasicInfoDTO.LetterTypeId,
                    DeliveryMethod = addOutboundExternalBasicInfoDTO.DeliveryMethod,
                    DeliveryMethodId = addOutboundExternalBasicInfoDTO.DeliveryMethodId,
                    POBox = addOutboundExternalBasicInfoDTO.POBox,
                    PostCode = addOutboundExternalBasicInfoDTO.PostCode,
                    IsDraft = addOutboundExternalBasicInfoDTO.IsDraft,
                    ReporterId = addOutboundExternalBasicInfoDTO.ReporterId,
                    TransactionPathId = addOutboundExternalBasicInfoDTO.TransactionPathId,
                    privacyLevelId = addOutboundExternalBasicInfoDTO.privacyLevelId,
                    LetterNumber = addOutboundExternalBasicInfoDTO.LetterNumber,
                    IsPresentationDraft = addOutboundExternalBasicInfoDTO.IsPresentationDraft,
                    PresentationDraftNumber = addOutboundExternalBasicInfoDTO.PresentationDraftNumber,
                    IsElcOutBound = addOutboundExternalBasicInfoDTO.IsElcOutBound,
                    NeedAcknowled = addOutboundExternalBasicInfoDTO.NeedAcknowled,
                    OutBoundDraftNumber = addOutboundExternalBasicInfoDTO.OutBoundDraftNumber,
                    Encrypted = addOutboundExternalBasicInfoDTO.Encrypted

                };
            }
            return new AddOutboundExternalBasicInfoVM();
        }
        public static AddOutboundExternalBasicInfoDTO Map(AddOutboundExternalBasicInfoVM addOutboundExternalBasicInfoVM)
        {
            if (addOutboundExternalBasicInfoVM != null)
            {
                return new AddOutboundExternalBasicInfoDTO()
                {
                    ConfidentialityLevelId = addOutboundExternalBasicInfoVM.ConfidentialityLevelId,
                    DestinationId = addOutboundExternalBasicInfoVM.DestinationId.HasValue ? addOutboundExternalBasicInfoVM.DestinationId.Value : 0,
                    ExternalPartyId = addOutboundExternalBasicInfoVM.ExternalPartyId.HasValue ? addOutboundExternalBasicInfoVM.ExternalPartyId.Value : 0,
                    DirectedToId = addOutboundExternalBasicInfoVM.DirectedToId,
                    Hour = addOutboundExternalBasicInfoVM.Hour,
                    Minute = addOutboundExternalBasicInfoVM.Minute,
                    OutboundNumber = addOutboundExternalBasicInfoVM.OutboundNumber,
                    PreparationEntityId = addOutboundExternalBasicInfoVM.PreparationEntityId.HasValue ? addOutboundExternalBasicInfoVM.PreparationEntityId.Value : 0,
                    PriorityLevelId = addOutboundExternalBasicInfoVM.PriorityLevelId,
                    Remarks = addOutboundExternalBasicInfoVM.Remarks,
                    RemindDate = addOutboundExternalBasicInfoVM.RemindDate,
                    RemindDateH = addOutboundExternalBasicInfoVM.RemindDateH,
                    SignedById = addOutboundExternalBasicInfoVM.SignedById,
                    TransactionTypeId = addOutboundExternalBasicInfoVM.TransactionTypeId,
                    Subject = addOutboundExternalBasicInfoVM.Subject,
                    SubjectClassifications = addOutboundExternalBasicInfoVM.SubjectClassifications,
                    SuggestedTopicId = addOutboundExternalBasicInfoVM.SuggestedTopicId,
                    LetterTypeId = addOutboundExternalBasicInfoVM.LetterTypeId,
                    DeliveryMethod = addOutboundExternalBasicInfoVM.DeliveryMethod,
                    DeliveryMethodId = addOutboundExternalBasicInfoVM.DeliveryMethodId.Value,
                    POBox = addOutboundExternalBasicInfoVM.POBox,
                    PostCode = addOutboundExternalBasicInfoVM.PostCode,
                    IsDraft = addOutboundExternalBasicInfoVM.IsDraft,
                    ReporterId = addOutboundExternalBasicInfoVM.ReporterId,
                    TransactionPathId = addOutboundExternalBasicInfoVM.TransactionPathId,
                    SubjectClassificationsId = addOutboundExternalBasicInfoVM.SubjectClassificationsId,
                    isOutboundInternalDraft = addOutboundExternalBasicInfoVM.isOutboundInternalDraft,
                    ComplaintNumber = addOutboundExternalBasicInfoVM.ComplaintNumber,
                    privacyLevelId = addOutboundExternalBasicInfoVM.privacyLevelId,
                    LetterNumber = addOutboundExternalBasicInfoVM.LetterNumber,
                    IsPresentationDraft = addOutboundExternalBasicInfoVM.IsPresentationDraft,
                    PresentationDraftNumber = addOutboundExternalBasicInfoVM.PresentationDraftNumber,
                    IsElcOutBound = addOutboundExternalBasicInfoVM.IsElcOutBound,
                    NeedAcknowled = addOutboundExternalBasicInfoVM.NeedAcknowled,
                    OutBoundDraftNumber = addOutboundExternalBasicInfoVM.OutBoundDraftNumber,
                    IsDecisionDraft = addOutboundExternalBasicInfoVM.IsDecisionDraft,
                    Summary = addOutboundExternalBasicInfoVM.Summary,
                    Encrypted = addOutboundExternalBasicInfoVM.Encrypted

                };
            }
            return new AddOutboundExternalBasicInfoDTO();
        }
        public static List<AddOutboundExternalBasicInfoDTO> Map(IList<AddOutboundExternalBasicInfoVM> addOutboundExternalBasicInfoVMs)
        {
            if (addOutboundExternalBasicInfoVMs == null || !addOutboundExternalBasicInfoVMs.Any())
            {
                return new List<AddOutboundExternalBasicInfoDTO>();
            }
            List<AddOutboundExternalBasicInfoDTO> addOutboundExternalBasicInfoDTOs = addOutboundExternalBasicInfoVMs
                .Select(addOutboundExternalBasicInfoVM => new AddOutboundExternalBasicInfoDTO()
                {
                    ConfidentialityLevelId = addOutboundExternalBasicInfoVM.ConfidentialityLevelId,
                    DestinationId = addOutboundExternalBasicInfoVM.DestinationId.Value,
                    DirectedToId = addOutboundExternalBasicInfoVM.DirectedToId,
                    Hour = addOutboundExternalBasicInfoVM.Hour,
                    Minute = addOutboundExternalBasicInfoVM.Minute,
                    OutboundNumber = addOutboundExternalBasicInfoVM.OutboundNumber,
                    PreparationEntityId = addOutboundExternalBasicInfoVM.PreparationEntityId.Value,
                    PriorityLevelId = addOutboundExternalBasicInfoVM.PriorityLevelId,
                    Remarks = addOutboundExternalBasicInfoVM.Remarks,
                    RemindDate = addOutboundExternalBasicInfoVM.RemindDate,
                    RemindDateH = addOutboundExternalBasicInfoVM.RemindDateH,
                    SignedById = addOutboundExternalBasicInfoVM.SignedById,
                    TransactionTypeId = addOutboundExternalBasicInfoVM.TransactionTypeId,
                    Subject = addOutboundExternalBasicInfoVM.Subject,
                    SubjectClassifications = addOutboundExternalBasicInfoVM.SubjectClassifications,
                    SuggestedTopicId = addOutboundExternalBasicInfoVM.SuggestedTopicId,
                    LetterTypeId = addOutboundExternalBasicInfoVM.LetterTypeId,
                    DeliveryMethod = addOutboundExternalBasicInfoVM.DeliveryMethod,
                    DeliveryMethodId = addOutboundExternalBasicInfoVM.DeliveryMethodId.Value,
                    POBox = addOutboundExternalBasicInfoVM.POBox,
                    PostCode = addOutboundExternalBasicInfoVM.PostCode,
                    IsDraft = addOutboundExternalBasicInfoVM.IsDraft,
                    privacyLevelId = addOutboundExternalBasicInfoVM.privacyLevelId,
                    LetterNumber = addOutboundExternalBasicInfoVM.LetterNumber,
                    IsPresentationDraft = addOutboundExternalBasicInfoVM.IsPresentationDraft,
                    PresentationDraftNumber = addOutboundExternalBasicInfoVM.PresentationDraftNumber,
                    IsElcOutBound = addOutboundExternalBasicInfoVM.IsElcOutBound,
                    NeedAcknowled = addOutboundExternalBasicInfoVM.NeedAcknowled,
                    OutBoundDraftNumber = addOutboundExternalBasicInfoVM.OutBoundDraftNumber,
                    Encrypted = addOutboundExternalBasicInfoVM.Encrypted,

                }).ToList();
            return addOutboundExternalBasicInfoDTOs;


        }
        public static List<AddOutboundExternalBasicInfoVM> Map(IList<AddOutboundExternalBasicInfoDTO> addOutboundExternalBasicInfoDTOs)
        {
            if (addOutboundExternalBasicInfoDTOs == null || !addOutboundExternalBasicInfoDTOs.Any())
            {
                return new List<AddOutboundExternalBasicInfoVM>();
            }
            List<AddOutboundExternalBasicInfoVM> addOutboundExternalBasicInfoVMs = addOutboundExternalBasicInfoDTOs
                    .Select(addOutboundExternalBasicInfoDTO => new AddOutboundExternalBasicInfoVM()
                    {

                        ConfidentialityLevelId = addOutboundExternalBasicInfoDTO.ConfidentialityLevelId,
                        DestinationId = addOutboundExternalBasicInfoDTO.DestinationId,
                        DirectedToId = addOutboundExternalBasicInfoDTO.DirectedToId,
                        Hour = addOutboundExternalBasicInfoDTO.Hour,
                        Minute = addOutboundExternalBasicInfoDTO.Minute,
                        OutboundNumber = addOutboundExternalBasicInfoDTO.OutboundNumber,
                        PreparationEntityId = addOutboundExternalBasicInfoDTO.PreparationEntityId,
                        PriorityLevelId = addOutboundExternalBasicInfoDTO.PriorityLevelId,
                        Remarks = addOutboundExternalBasicInfoDTO.Remarks,
                        RemindDate = addOutboundExternalBasicInfoDTO.RemindDate,
                        RemindDateH = addOutboundExternalBasicInfoDTO.RemindDateH,
                        SignedById = addOutboundExternalBasicInfoDTO.SignedById,
                        TransactionTypeId = addOutboundExternalBasicInfoDTO.TransactionTypeId,
                        Subject = addOutboundExternalBasicInfoDTO.Subject,
                        SubjectClassifications = addOutboundExternalBasicInfoDTO.SubjectClassifications,
                        SuggestedTopicId = addOutboundExternalBasicInfoDTO.SuggestedTopicId,
                        LetterTypeId = addOutboundExternalBasicInfoDTO.LetterTypeId,
                        DeliveryMethod = addOutboundExternalBasicInfoDTO.DeliveryMethod,
                        DeliveryMethodId = addOutboundExternalBasicInfoDTO.DeliveryMethodId,
                        POBox = addOutboundExternalBasicInfoDTO.POBox,
                        PostCode = addOutboundExternalBasicInfoDTO.PostCode,
                        IsDraft = addOutboundExternalBasicInfoDTO.IsDraft,
                        privacyLevelId = addOutboundExternalBasicInfoDTO.privacyLevelId,
                        LetterNumber = addOutboundExternalBasicInfoDTO.LetterNumber,
                        IsPresentationDraft = addOutboundExternalBasicInfoDTO.IsPresentationDraft,
                        PresentationDraftNumber = addOutboundExternalBasicInfoDTO.PresentationDraftNumber,
                        IsElcOutBound = addOutboundExternalBasicInfoDTO.IsElcOutBound,
                        NeedAcknowled = addOutboundExternalBasicInfoDTO.NeedAcknowled,
                        OutBoundDraftNumber = addOutboundExternalBasicInfoDTO.OutBoundDraftNumber,
                        Encrypted = addOutboundExternalBasicInfoDTO.Encrypted,

                    }).ToList();

            return addOutboundExternalBasicInfoVMs;

        }

        internal static VIPEditOutboundExternalBasicInfoVM VIPMap(EditOutboundExternalBasicInfoDTO outboundExternalBasicInfo)
        {
            if (outboundExternalBasicInfo != null)
            {
                VIPEditOutboundExternalBasicInfoVM vipEditOutboundExternalBasicInfoVM = new VIPEditOutboundExternalBasicInfoVM()
                {
                    ConfidentialityLevelId = outboundExternalBasicInfo.ConfidentialityLevelId,
                    PriorityLevelId = outboundExternalBasicInfo.PriorityLevelId,
                    Hour = outboundExternalBasicInfo.Hour,
                    Minute = outboundExternalBasicInfo.Minute,
                    Number = outboundExternalBasicInfo.OutboundNumber,
                    RemindDate = outboundExternalBasicInfo.RemindDate,
                    RemindDateH = outboundExternalBasicInfo.RemindDateH,
                    SubjectClassifications = outboundExternalBasicInfo.SubjectClassifications,
                    SuggestedTopicId = outboundExternalBasicInfo.SuggestedTopicId,
                    LetterTypeId = outboundExternalBasicInfo.LetterTypeId,
                    ConfidentialityLevelText = outboundExternalBasicInfo.ConfidentialityLevelText,
                    CreatedDateH = outboundExternalBasicInfo.CreatedDateH,
                    EntityName = outboundExternalBasicInfo.EntityName,
                    Subject = outboundExternalBasicInfo.Subject,
                    LetterNumber = outboundExternalBasicInfo.LetterNumber,
                    IsDraft = outboundExternalBasicInfo.IsDraft,
                    Summary = outboundExternalBasicInfo.Summary,

                    PriorityLeveText = outboundExternalBasicInfo.PriorityLeveText
                };
                return vipEditOutboundExternalBasicInfoVM;
            }
            return new VIPEditOutboundExternalBasicInfoVM();
        }

        public static List<EditOutboundExternalBasicInfoVM> Map(IList<EditOutboundExternalBasicInfoDTO> EditOutboundExternalBasicInfoDTOs)
        {
            if (EditOutboundExternalBasicInfoDTOs == null || !EditOutboundExternalBasicInfoDTOs.Any())
            {
                return new List<EditOutboundExternalBasicInfoVM>();
            }
            List<EditOutboundExternalBasicInfoVM> EditOutboundExternalBasicInfoVMs = EditOutboundExternalBasicInfoDTOs
                .Select(EditOutboundExternalBasicInfoDTO => new EditOutboundExternalBasicInfoVM()
                {

                    ConfidentialityLevelId = EditOutboundExternalBasicInfoDTO.ConfidentialityLevelId,
                    DestinationId = EditOutboundExternalBasicInfoDTO.DestinationId,
                    DirectedToId = EditOutboundExternalBasicInfoDTO.DirectedToId,
                    Hour = EditOutboundExternalBasicInfoDTO.Hour,
                    Minute = EditOutboundExternalBasicInfoDTO.Minute,
                    OutboundNumber = EditOutboundExternalBasicInfoDTO.OutboundNumber,
                    PreparationEntityId = EditOutboundExternalBasicInfoDTO.PreparationEntityId,
                    PriorityLevelId = EditOutboundExternalBasicInfoDTO.PriorityLevelId,
                    Remarks = EditOutboundExternalBasicInfoDTO.Remarks,
                    RemindDate = EditOutboundExternalBasicInfoDTO.RemindDate,
                    RemindDateH = EditOutboundExternalBasicInfoDTO.RemindDateH,
                    SignedById = EditOutboundExternalBasicInfoDTO.SignedById,
                    TransactionTypeId = EditOutboundExternalBasicInfoDTO.TransactionTypeId,
                    Subject = EditOutboundExternalBasicInfoDTO.Subject,
                    SubjectClassifications = EditOutboundExternalBasicInfoDTO.SubjectClassifications,
                    SuggestedTopicId = EditOutboundExternalBasicInfoDTO.SuggestedTopicId,
                    LetterTypeId = EditOutboundExternalBasicInfoDTO.LetterTypeId,
                    DeliveryMethod = EditOutboundExternalBasicInfoDTO.DeliveryMethod,
                    DeliveryMethodId = EditOutboundExternalBasicInfoDTO.DeliveryMethodId,
                    IsDraft = EditOutboundExternalBasicInfoDTO.IsDraft,
                    POBox = EditOutboundExternalBasicInfoDTO.POBox,
                    PostCode = EditOutboundExternalBasicInfoDTO.PostCode,
                    privacyLevelId = EditOutboundExternalBasicInfoDTO.privacyLevelId,
                    LetterNumber = EditOutboundExternalBasicInfoDTO.LetterNumber,
                    IsPresentationDraft = EditOutboundExternalBasicInfoDTO.IsPresentationDraft,
                    PresentationDraftNumber = EditOutboundExternalBasicInfoDTO.PresentationDraftNumber,
                    IsElcOutBound = EditOutboundExternalBasicInfoDTO.IsElcOutBound,
                    NeedAcknowled = EditOutboundExternalBasicInfoDTO.NeedAcknowled,
                    OutBoundDraftNumber = EditOutboundExternalBasicInfoDTO.OutBoundDraftNumber,
                    Encrypted = EditOutboundExternalBasicInfoDTO.Encrypted

                }).ToList();

            return EditOutboundExternalBasicInfoVMs;

        }
        public static List<EditOutboundExternalBasicInfoDTO> Map(IList<EditOutboundExternalBasicInfoVM> EditOutboundExternalBasicInfoVMs)
        {
            if (EditOutboundExternalBasicInfoVMs == null || !EditOutboundExternalBasicInfoVMs.Any())
            {
                return new List<EditOutboundExternalBasicInfoDTO>();
            }
            List<EditOutboundExternalBasicInfoDTO> EditOutboundExternalBasicInfoDTOs = EditOutboundExternalBasicInfoVMs
                .Select(EditOutboundExternalBasicInfoVM => new EditOutboundExternalBasicInfoDTO()
                {

                    ConfidentialityLevelId = EditOutboundExternalBasicInfoVM.ConfidentialityLevelId,
                    DestinationId = EditOutboundExternalBasicInfoVM.DestinationId,
                    DirectedToId = EditOutboundExternalBasicInfoVM.DirectedToId,
                    Hour = EditOutboundExternalBasicInfoVM.Hour,
                    Minute = EditOutboundExternalBasicInfoVM.Minute,
                    OutboundNumber = EditOutboundExternalBasicInfoVM.OutboundNumber,
                    PreparationEntityId = EditOutboundExternalBasicInfoVM.PreparationEntityId.Value,
                    PriorityLevelId = EditOutboundExternalBasicInfoVM.PriorityLevelId,
                    Remarks = EditOutboundExternalBasicInfoVM.Remarks,
                    RemindDate = EditOutboundExternalBasicInfoVM.RemindDate,
                    RemindDateH = EditOutboundExternalBasicInfoVM.RemindDateH,
                    SignedById = EditOutboundExternalBasicInfoVM.SignedById,
                    TransactionTypeId = EditOutboundExternalBasicInfoVM.TransactionTypeId,
                    Subject = EditOutboundExternalBasicInfoVM.Subject,
                    SubjectClassifications = EditOutboundExternalBasicInfoVM.SubjectClassifications,
                    SuggestedTopicId = EditOutboundExternalBasicInfoVM.SuggestedTopicId,
                    LetterTypeId = EditOutboundExternalBasicInfoVM.LetterTypeId,
                    DeliveryMethod = EditOutboundExternalBasicInfoVM.DeliveryMethod,
                    DeliveryMethodId = EditOutboundExternalBasicInfoVM.DeliveryMethodId,
                    IsDraft = EditOutboundExternalBasicInfoVM.IsDraft,
                    POBox = EditOutboundExternalBasicInfoVM.POBox,
                    PostCode = EditOutboundExternalBasicInfoVM.PostCode,
                    privacyLevelId = EditOutboundExternalBasicInfoVM.privacyLevelId,
                    LetterNumber = EditOutboundExternalBasicInfoVM.LetterNumber,
                    IsPresentationDraft = EditOutboundExternalBasicInfoVM.IsPresentationDraft,
                    PresentationDraftNumber = EditOutboundExternalBasicInfoVM.PresentationDraftNumber,
                    IsElcOutBound = EditOutboundExternalBasicInfoVM.IsElcOutBound,
                    NeedAcknowled = EditOutboundExternalBasicInfoVM.NeedAcknowled,
                    OutBoundDraftNumber = EditOutboundExternalBasicInfoVM.OutBoundDraftNumber,
                    Encrypted = EditOutboundExternalBasicInfoVM.Encrypted

                }).ToList();

            return EditOutboundExternalBasicInfoDTOs;

        }
        public static EditOutboundExternalBasicInfoVM Map(EditOutboundExternalBasicInfoDTO EditOutboundExternalBasicInfoDTO)
        {
            if (EditOutboundExternalBasicInfoDTO != null)
            {
                EditOutboundExternalBasicInfoVM EditOutboundExternalBasicInfoVM = new EditOutboundExternalBasicInfoVM()
                {

                    ConfidentialityLevelId = EditOutboundExternalBasicInfoDTO.ConfidentialityLevelId,
                    DestinationId = EditOutboundExternalBasicInfoDTO.DestinationId,
                    DirectedToId = EditOutboundExternalBasicInfoDTO.DirectedToId,
                    Hour = EditOutboundExternalBasicInfoDTO.Hour,
                    Minute = EditOutboundExternalBasicInfoDTO.Minute,
                    OutboundNumber = EditOutboundExternalBasicInfoDTO.OutboundNumber,
                    PreparationEntityId = EditOutboundExternalBasicInfoDTO.PreparationEntityId,
                    PriorityLevelId = EditOutboundExternalBasicInfoDTO.PriorityLevelId,
                    Remarks = EditOutboundExternalBasicInfoDTO.Remarks,
                    RemindDate = EditOutboundExternalBasicInfoDTO.RemindDate,
                    RemindDateH = EditOutboundExternalBasicInfoDTO.RemindDateH,
                    SignedById = EditOutboundExternalBasicInfoDTO.SignedById,
                    TransactionTypeId = EditOutboundExternalBasicInfoDTO.TransactionTypeId,
                    Subject = EditOutboundExternalBasicInfoDTO.Subject,
                    SubjectClassifications = EditOutboundExternalBasicInfoDTO.SubjectClassifications,
                    SuggestedTopicId = EditOutboundExternalBasicInfoDTO.SuggestedTopicId,
                    LetterTypeId = EditOutboundExternalBasicInfoDTO.LetterTypeId,
                    DeliveryMethod = EditOutboundExternalBasicInfoDTO.DeliveryMethod,
                    DeliveryMethodId = EditOutboundExternalBasicInfoDTO.DeliveryMethodId,
                    IsDraft = EditOutboundExternalBasicInfoDTO.IsDraft,
                    POBox = EditOutboundExternalBasicInfoDTO.POBox,
                    PostCode = EditOutboundExternalBasicInfoDTO.PostCode,
                    ReporterId = EditOutboundExternalBasicInfoDTO.ReporterId,
                    TransactionPathId = EditOutboundExternalBasicInfoDTO.TransactionPathId,
                    ProcessPeriodTransaction = EditOutboundExternalBasicInfoDTO.ProcessPeriodTransaction,
                    SubjectClassificationsId = EditOutboundExternalBasicInfoDTO.SubjectClassificationsId,
                    isOutboundInternalDraft = EditOutboundExternalBasicInfoDTO.isOutboundInternalDraft,
                    ComplaintNumber = EditOutboundExternalBasicInfoDTO.ComplaintNumber,
                    privacyLevelId = EditOutboundExternalBasicInfoDTO.privacyLevelId,
                    LetterNumber = EditOutboundExternalBasicInfoDTO.LetterNumber,
                    ExternalPartyId = EditOutboundExternalBasicInfoDTO.ExternalPartyId,
                    IsPresentationDraft = EditOutboundExternalBasicInfoDTO.IsPresentationDraft,
                    PresentationDraftNumber = EditOutboundExternalBasicInfoDTO.PresentationDraftNumber,
                    IsElcOutBound = EditOutboundExternalBasicInfoDTO.IsElcOutBound,
                    NeedAcknowled = EditOutboundExternalBasicInfoDTO.NeedAcknowled,
                    OutBoundDraftNumber = EditOutboundExternalBasicInfoDTO.OutBoundDraftNumber,
                    IsDecisionDraft = EditOutboundExternalBasicInfoDTO.IsDecisionDraft,
                    Summary = EditOutboundExternalBasicInfoDTO.Summary,
                    Encrypted = EditOutboundExternalBasicInfoDTO.Encrypted

                };

                return EditOutboundExternalBasicInfoVM;
            }
            return new EditOutboundExternalBasicInfoVM();

        }
        public static EditOutboundExternalBasicInfoDTO Map(EditOutboundExternalBasicInfoVM EditOutboundExternalBasicInfoVM)
        {
            if (EditOutboundExternalBasicInfoVM != null)
            {
                DateTime? dt = null;
                if (EditOutboundExternalBasicInfoVM.RemindDate.HasValue)
                {
                    DateTime d = EditOutboundExternalBasicInfoVM.RemindDate.Value;
                    dt = new DateTime(d.Year, d.Month, d.Day, 0, 0, 0);
                }
                EditOutboundExternalBasicInfoDTO EditOutboundExternalBasicInfoDTO = new EditOutboundExternalBasicInfoDTO()
                {

                    ConfidentialityLevelId = EditOutboundExternalBasicInfoVM.ConfidentialityLevelId,
                    DestinationId = EditOutboundExternalBasicInfoVM.DestinationId,
                    DirectedToId = EditOutboundExternalBasicInfoVM.DirectedToId,
                    Hour = EditOutboundExternalBasicInfoVM.Hour,
                    Minute = EditOutboundExternalBasicInfoVM.Minute,
                    OutboundNumber = EditOutboundExternalBasicInfoVM.OutboundNumber,
                    PreparationEntityId = EditOutboundExternalBasicInfoVM.PreparationEntityId.Value,
                    PriorityLevelId = EditOutboundExternalBasicInfoVM.PriorityLevelId,
                    Remarks = EditOutboundExternalBasicInfoVM.Remarks,
                    RemindDate = dt,
                    RemindDateH = EditOutboundExternalBasicInfoVM.RemindDateH,
                    SignedById = EditOutboundExternalBasicInfoVM.SignedById,
                    TransactionTypeId = EditOutboundExternalBasicInfoVM.TransactionTypeId,
                    Subject = EditOutboundExternalBasicInfoVM.Subject,
                    SubjectClassifications = EditOutboundExternalBasicInfoVM.SubjectClassifications,
                    SuggestedTopicId = EditOutboundExternalBasicInfoVM.SuggestedTopicId,
                    LetterTypeId = EditOutboundExternalBasicInfoVM.LetterTypeId,
                    DeliveryMethod = EditOutboundExternalBasicInfoVM.DeliveryMethod,
                    DeliveryMethodId = EditOutboundExternalBasicInfoVM.DeliveryMethodId,
                    IsDraft = EditOutboundExternalBasicInfoVM.IsDraft,
                    POBox = EditOutboundExternalBasicInfoVM.POBox,
                    PostCode = EditOutboundExternalBasicInfoVM.PostCode,
                    ReporterId = EditOutboundExternalBasicInfoVM.ReporterId,
                    TransactionPathId = EditOutboundExternalBasicInfoVM.TransactionPathId,
                    ProcessPeriodTransaction = EditOutboundExternalBasicInfoVM.ProcessPeriodTransaction,
                    isOutboundInternalDraft = EditOutboundExternalBasicInfoVM.isOutboundInternalDraft,
                    ComplaintNumber = EditOutboundExternalBasicInfoVM.ComplaintNumber,
                    privacyLevelId = EditOutboundExternalBasicInfoVM.privacyLevelId,
                    LetterNumber = EditOutboundExternalBasicInfoVM.LetterNumber,
                    ExternalPartyId = EditOutboundExternalBasicInfoVM.ExternalPartyId ?? 0,
                    IsPresentationDraft = EditOutboundExternalBasicInfoVM.IsPresentationDraft,
                    PresentationDraftNumber = EditOutboundExternalBasicInfoVM.PresentationDraftNumber,
                    IsElcOutBound = EditOutboundExternalBasicInfoVM.IsElcOutBound,
                    NeedAcknowled = EditOutboundExternalBasicInfoVM.NeedAcknowled,
                    OutBoundDraftNumber = EditOutboundExternalBasicInfoVM.OutBoundDraftNumber,
                    Summary = EditOutboundExternalBasicInfoVM.Summary,
                    Encrypted = EditOutboundExternalBasicInfoVM.Encrypted,

                };

                return EditOutboundExternalBasicInfoDTO;
            }
            return new EditOutboundExternalBasicInfoDTO();

        }

    }
}