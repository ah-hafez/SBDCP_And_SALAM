using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction.Outbound.Draft;
using MCS.UI.Areas.User.Models.Transaction.Outbound.External;
using MCS.UI.Areas.User.Models.Transaction.Outbound.Internal;

namespace MCS.UI.Areas.User.Mappers.Transaction.OutBound
{
    public static class OutboundDraftBasicInfoMapper
    {

        public static AddOutboundDraftBasicInfoDTO Map(AddOutboundDraftBasicInfoVM addOutboundDraftBasicInfoVM)
        {
            if (addOutboundDraftBasicInfoVM != null)
            {
                AddOutboundDraftBasicInfoDTO addOutboundDraftBasicInfoDTO = new AddOutboundDraftBasicInfoDTO()
                {
                    DraftNumber = addOutboundDraftBasicInfoVM.DraftNumber,
                    ConfidentialityLevelId = addOutboundDraftBasicInfoVM.ConfidentialityLevelId,
                    DestinationId = addOutboundDraftBasicInfoVM.DestinationId,
                    DirectedToId = addOutboundDraftBasicInfoVM.DirectedToId,
                    Hour = addOutboundDraftBasicInfoVM.Hour,
                    Minute = addOutboundDraftBasicInfoVM.Minute,
                    PriorityLevelId = addOutboundDraftBasicInfoVM.PriorityLevelId,
                    RemindDate = addOutboundDraftBasicInfoVM.RemindDate,
                    RemindDateH = addOutboundDraftBasicInfoVM.RemindDateH,
                    SignedById = addOutboundDraftBasicInfoVM.SignedById,
                    SignedByOrgUnitId = addOutboundDraftBasicInfoVM.SignedByOrgUnitId,
                    TransactionTypeId = addOutboundDraftBasicInfoVM.TransactionTypeId,
                    Subject = addOutboundDraftBasicInfoVM.Subject,
                    SubjectClassifications = addOutboundDraftBasicInfoVM.SubjectClassifications,
                    LetterTypeId = addOutboundDraftBasicInfoVM.LetterTypeId,
                    SuggestedTopicId = addOutboundDraftBasicInfoVM.SuggestedTopicId,
                    IsDraft = addOutboundDraftBasicInfoVM.IsDraft,
                    DeliveryMethodId = addOutboundDraftBasicInfoVM.DeliveryMethodId,
                    DeliveryMethod = addOutboundDraftBasicInfoVM.DeliveryMethod,
                    POBox = addOutboundDraftBasicInfoVM.POBox,
                    PostCode = addOutboundDraftBasicInfoVM.PostCode,
                    LetterNumber = addOutboundDraftBasicInfoVM.LetterNumber

                };
                return addOutboundDraftBasicInfoDTO;
            }
            return new AddOutboundDraftBasicInfoDTO();
        }
        public static AddOutboundDraftBasicInfoVM Map(AddOutboundDraftBasicInfoDTO addOutboundDraftBasicInfoDTO)
        {
            if (addOutboundDraftBasicInfoDTO != null)
            {
                AddOutboundDraftBasicInfoVM addOutboundDraftBasicInfoVM = new AddOutboundDraftBasicInfoVM()
                {
                    DraftNumber = addOutboundDraftBasicInfoDTO.DraftNumber,
                    ConfidentialityLevelId = addOutboundDraftBasicInfoDTO.ConfidentialityLevelId,
                    DestinationId = addOutboundDraftBasicInfoDTO.DestinationId,
                    DirectedToId = addOutboundDraftBasicInfoDTO.DirectedToId,
                    Hour = addOutboundDraftBasicInfoDTO.Hour,
                    Minute = addOutboundDraftBasicInfoDTO.Minute,
                    PriorityLevelId = addOutboundDraftBasicInfoDTO.PriorityLevelId,
                    RemindDate = addOutboundDraftBasicInfoDTO.RemindDate,
                    RemindDateH = addOutboundDraftBasicInfoDTO.RemindDateH,
                    SignedById = addOutboundDraftBasicInfoDTO.SignedById,
                    SignedByOrgUnitId = addOutboundDraftBasicInfoDTO.SignedByOrgUnitId,
                    TransactionTypeId = addOutboundDraftBasicInfoDTO.TransactionTypeId,
                    Subject = addOutboundDraftBasicInfoDTO.Subject,
                    SubjectClassifications = addOutboundDraftBasicInfoDTO.SubjectClassifications,
                    LetterTypeId = addOutboundDraftBasicInfoDTO.LetterTypeId,
                    SuggestedTopicId = addOutboundDraftBasicInfoDTO.SuggestedTopicId,
                    IsDraft = addOutboundDraftBasicInfoDTO.IsDraft,
                    DeliveryMethodId = addOutboundDraftBasicInfoDTO.DeliveryMethodId,
                    DeliveryMethod = addOutboundDraftBasicInfoDTO.DeliveryMethod,
                    POBox = addOutboundDraftBasicInfoDTO.POBox,
                    PostCode = addOutboundDraftBasicInfoDTO.PostCode,
                    LetterNumber = addOutboundDraftBasicInfoDTO.LetterNumber

                };
                return addOutboundDraftBasicInfoVM;
            }
            return new AddOutboundDraftBasicInfoVM();
        }

        public static EditOutboundDraftBasicInfoVM Map(EditOutboundDraftBasicInfoDTO editOutboundDraftBasicInfoDTO)
        {
            if (editOutboundDraftBasicInfoDTO != null)
            {
                EditOutboundDraftBasicInfoVM editOutboundDraftBasicInfoVM = new EditOutboundDraftBasicInfoVM()
                {
                    DraftNumber = editOutboundDraftBasicInfoDTO.DraftNumber,
                    ConfidentialityLevelId = editOutboundDraftBasicInfoDTO.ConfidentialityLevelId,
                    DestinationId = editOutboundDraftBasicInfoDTO.DestinationId,
                    DirectedToId = editOutboundDraftBasicInfoDTO.DirectedToId,
                    Hour = editOutboundDraftBasicInfoDTO.Hour,
                    Minute = editOutboundDraftBasicInfoDTO.Minute,
                    PriorityLevelId = editOutboundDraftBasicInfoDTO.PriorityLevelId,
                    RemindDate = editOutboundDraftBasicInfoDTO.RemindDate,
                    RemindDateH = editOutboundDraftBasicInfoDTO.RemindDateH,
                    SignedById = editOutboundDraftBasicInfoDTO.SignedById,
                    CreatedOn = editOutboundDraftBasicInfoDTO.CreatedOn,
                    TransactionTypeId = editOutboundDraftBasicInfoDTO.TransactionTypeId,
                    Subject = editOutboundDraftBasicInfoDTO.Subject,
                    SubjectClassifications = editOutboundDraftBasicInfoDTO.SubjectClassifications,
                    LetterTypeId = editOutboundDraftBasicInfoDTO.LetterTypeId,
                    SuggestedTopicId = editOutboundDraftBasicInfoDTO.SuggestedTopicId,
                    IsDraft = editOutboundDraftBasicInfoDTO.IsDraft,
                    DeliveryMethodId = editOutboundDraftBasicInfoDTO.DeliveryMethodId,
                    DeliveryMethod = editOutboundDraftBasicInfoDTO.DeliveryMethod,
                    POBox = editOutboundDraftBasicInfoDTO.POBox,
                    PostCode = editOutboundDraftBasicInfoDTO.PostCode,
                    ReporterId = editOutboundDraftBasicInfoDTO.ReporterId,
                    LetterNumber = editOutboundDraftBasicInfoDTO.LetterNumber
                };
                return editOutboundDraftBasicInfoVM;
            }
            return new EditOutboundDraftBasicInfoVM();
        }

        public static VIPEditOutboundExternalBasicInfoVM VIPMap(EditOutboundDraftBasicInfoDTO outboundExternalBasicInfo)
        {
            if (outboundExternalBasicInfo != null)
            {
                VIPEditOutboundExternalBasicInfoVM editOutboundInternalBasicInfoVM = new VIPEditOutboundExternalBasicInfoVM()
                {

                    ConfidentialityLevelId = outboundExternalBasicInfo.ConfidentialityLevelId,
                    PriorityLevelId = outboundExternalBasicInfo.PriorityLevelId,
                    Number = outboundExternalBasicInfo.DraftNumber.Value,
                    ConfidentialityLevelText = outboundExternalBasicInfo.ConfidentialityLevelText,
                    CreatedDateH = outboundExternalBasicInfo.CreatedDateH,
                    EntityName = outboundExternalBasicInfo.EntityName,
                    Subject = outboundExternalBasicInfo.Subject,
                    LetterNumber = outboundExternalBasicInfo.LetterNumber,
                    IsDecisionDraft = outboundExternalBasicInfo.IsDecisionDraft,
                    PriorityLeveText = outboundExternalBasicInfo.PriorityLeveText,
                    Summary = outboundExternalBasicInfo.Summary,

                };
                return editOutboundInternalBasicInfoVM;

            }
            return new VIPEditOutboundExternalBasicInfoVM();
        }
        public static EditOutboundDraftBasicInfoDTO Map(EditOutboundDraftBasicInfoVM editOutboundDraftBasicInfoVM)
        {
            if (editOutboundDraftBasicInfoVM != null)
            {
                EditOutboundDraftBasicInfoDTO editOutboundDraftBasicInfoDTO = new EditOutboundDraftBasicInfoDTO()
                {
                    DraftNumber = editOutboundDraftBasicInfoVM.DraftNumber,
                    ConfidentialityLevelId = editOutboundDraftBasicInfoVM.ConfidentialityLevelId,
                    DestinationId = editOutboundDraftBasicInfoVM.DestinationId,
                    DirectedToId = editOutboundDraftBasicInfoVM.DirectedToId,
                    Hour = editOutboundDraftBasicInfoVM.Hour,
                    Minute = editOutboundDraftBasicInfoVM.Minute,
                    PriorityLevelId = editOutboundDraftBasicInfoVM.PriorityLevelId,
                    RemindDate = editOutboundDraftBasicInfoVM.RemindDate,
                    RemindDateH = editOutboundDraftBasicInfoVM.RemindDateH,
                    SignedById = editOutboundDraftBasicInfoVM.SignedById,
                    CreatedOn = editOutboundDraftBasicInfoVM.CreatedOn,
                    TransactionTypeId = editOutboundDraftBasicInfoVM.TransactionTypeId,
                    Subject = editOutboundDraftBasicInfoVM.Subject,
                    SubjectClassifications = editOutboundDraftBasicInfoVM.SubjectClassifications,
                    LetterTypeId = editOutboundDraftBasicInfoVM.LetterTypeId,
                    SuggestedTopicId = editOutboundDraftBasicInfoVM.SuggestedTopicId,
                    IsDraft = editOutboundDraftBasicInfoVM.IsDraft,
                    DeliveryMethodId = editOutboundDraftBasicInfoVM.DeliveryMethodId,
                    DeliveryMethod = editOutboundDraftBasicInfoVM.DeliveryMethod,
                    POBox = editOutboundDraftBasicInfoVM.POBox,
                    PostCode = editOutboundDraftBasicInfoVM.PostCode,
                    LetterNumber = editOutboundDraftBasicInfoVM.LetterNumber

                };
                return editOutboundDraftBasicInfoDTO;
            }
            return new EditOutboundDraftBasicInfoDTO();
        }


    }
}