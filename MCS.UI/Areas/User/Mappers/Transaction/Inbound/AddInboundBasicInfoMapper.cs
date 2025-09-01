using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction.Inbound;

namespace MCS.UI.Areas.User.Mappers.Transaction.Inbound
{
    public static class AddInboundBasicInfoMapper
    {
        public static AddInboundBasicInfoVM Map(AddInboundBasicInfoDTO addInboundBasicInfoDTOs)
        {
            if (addInboundBasicInfoDTOs != null)
            {
                AddInboundBasicInfoVM addInboundBasicInfoVM = new AddInboundBasicInfoVM()
                {
                    ConfidentialityLevelId = addInboundBasicInfoDTOs.ConfidentialityLevelId,
                    DestinationId = addInboundBasicInfoDTOs.DestinationId,
                    DirectedToId = addInboundBasicInfoDTOs.DirectedToId,
                    DirectedToOrgUnitId = addInboundBasicInfoDTOs.DirectedToOrgUnitId,
                    Hour = addInboundBasicInfoDTOs.Hour,
                    InboundDocumentNumber = addInboundBasicInfoDTOs.InboundDocumentNumber,
                    InboundNumber = addInboundBasicInfoDTOs.InboundNumber,
                    Minute = addInboundBasicInfoDTOs.Minute,
                    OutboundNumber = addInboundBasicInfoDTOs.OutboundNumber,
                    PriorityLevelId = addInboundBasicInfoDTOs.PriorityLevelId,
                    Remarks = addInboundBasicInfoDTOs.Remarks,
                    RemindDate = addInboundBasicInfoDTOs.RemindDate,
                    RemindDateH = addInboundBasicInfoDTOs.RemindDateH,
                    SignedById = addInboundBasicInfoDTOs.SignedById,
                    SignedByOrgUnitId = addInboundBasicInfoDTOs.SignedByOrgUnitId,
                    TransactionTypeId = addInboundBasicInfoDTOs.TransactionTypeId,
                    Subject = addInboundBasicInfoDTOs.Subject,
                    SubjectClassifications = addInboundBasicInfoDTOs.SubjectClassifications,
                    SuggestedTopicId = addInboundBasicInfoDTOs.SuggestedTopicId,
                    LetterTypeId = addInboundBasicInfoDTOs.LetterTypeId,
                    DeliveryMethodId = addInboundBasicInfoDTOs.DeliveryMethodId,
                    DeliveryMethod = addInboundBasicInfoDTOs.DeliveryMethod,
                    InboundDateH = addInboundBasicInfoDTOs.InboundDateH,
                    IsForIndividual = addInboundBasicInfoDTOs.IsForIndividual,
                    ReporterId = addInboundBasicInfoDTOs.ReporterId,
                    InboundIntendedPerson = addInboundBasicInfoDTOs.InboundIntendedPerson,
                    SideContactExternalEntityID = addInboundBasicInfoDTOs.SideContactExternalEntityID,
                    NumberContact = addInboundBasicInfoDTOs.NumberContact,
                    ContactDateH = addInboundBasicInfoDTOs.ContactDateH,
                    privacyLevelId = addInboundBasicInfoDTOs.privacyLevelId,
                    LetterNumber = addInboundBasicInfoDTOs.LetterNumber,
                    Encrypted = addInboundBasicInfoDTOs.Encrypted

                };

                return addInboundBasicInfoVM;
            }
            return new AddInboundBasicInfoVM();
        }
        public static AddInboundBasicInfoDTO Map(AddInboundBasicInfoVM addInboundBasicInfoVMs)
        {
            if (addInboundBasicInfoVMs != null)
            {

                AddInboundBasicInfoDTO addInboundBasicInfoDTO = new AddInboundBasicInfoDTO()
                {
                    ConfidentialityLevelId = addInboundBasicInfoVMs.ConfidentialityLevelId,
                    DestinationId = addInboundBasicInfoVMs.DestinationId,
                    DirectedToId = addInboundBasicInfoVMs.DirectedToId,
                    DirectedToOrgUnitId = addInboundBasicInfoVMs.DirectedToOrgUnitId,
                    Hour = addInboundBasicInfoVMs.Hour,
                    InboundDocumentNumber = addInboundBasicInfoVMs.InboundDocumentNumber,
                    InboundNumber = addInboundBasicInfoVMs.InboundNumber,
                    Minute = addInboundBasicInfoVMs.Minute,
                    OutboundNumber = addInboundBasicInfoVMs.OutboundNumber,
                    PriorityLevelId = addInboundBasicInfoVMs.PriorityLevelId,
                    Remarks = addInboundBasicInfoVMs.Remarks,
                    RemindDate = addInboundBasicInfoVMs.RemindDate,
                    RemindDateH = addInboundBasicInfoVMs.RemindDateH,
                    SignedById = addInboundBasicInfoVMs.SignedById,
                    SignedByOrgUnitId = addInboundBasicInfoVMs.SignedByOrgUnitId,
                    TransactionTypeId = addInboundBasicInfoVMs.TransactionTypeId,
                    Subject = addInboundBasicInfoVMs.Subject,
                    SubjectClassifications = addInboundBasicInfoVMs.SubjectClassifications,
                    SuggestedTopicId = addInboundBasicInfoVMs.SuggestedTopicId,
                    LetterTypeId = addInboundBasicInfoVMs.LetterTypeId,
                    DeliveryMethod = addInboundBasicInfoVMs.DeliveryMethod,
                    DeliveryMethodId = addInboundBasicInfoVMs.DeliveryMethodId,
                    InboundDateH = addInboundBasicInfoVMs.InboundDateH,
                    IsForIndividual = addInboundBasicInfoVMs.IsForIndividual,
                    ReporterId = addInboundBasicInfoVMs.ReporterId,
                    InboundIntendedPerson = addInboundBasicInfoVMs.InboundIntendedPerson,
                    SubjectClassificationsId = addInboundBasicInfoVMs.SubjectClassificationsId,
                    RecordNumber = addInboundBasicInfoVMs.RecordNumber,
                    SideContactExternalEntityID = addInboundBasicInfoVMs.SideContactExternalEntityID,
                    NumberContact = addInboundBasicInfoVMs.NumberContact,
                    ContactDateH = addInboundBasicInfoVMs.ContactDateH,
                    privacyLevelId = addInboundBasicInfoVMs.privacyLevelId,
                    LetterNumber = addInboundBasicInfoVMs.LetterNumber,
                    CityId = addInboundBasicInfoVMs.CityId,
                    Summary = addInboundBasicInfoVMs.Summary,
                    Encrypted = addInboundBasicInfoVMs.Encrypted,


                };

                return addInboundBasicInfoDTO;
            }
            return new AddInboundBasicInfoDTO();
        }

    }
}