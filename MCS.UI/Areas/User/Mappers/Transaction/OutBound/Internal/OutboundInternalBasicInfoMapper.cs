using System;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction.Outbound.Internal;

namespace MCS.UI.Areas.User.Mappers.Transaction.OutBound.Internal
{
    public class OutboundInternalBasicInfoMapper
    {
        public static AddOutboundInternalBasicInfoVM Map(AddOutboundInternalBasicInfoDTO addOutboundInternalBasicInfoDTO)
        {
            if (addOutboundInternalBasicInfoDTO != null)
            {
                AddOutboundInternalBasicInfoVM addOutboundInternalBasicInfoVM = new AddOutboundInternalBasicInfoVM()
                {
                    ConfidentialityLevelId = addOutboundInternalBasicInfoDTO.ConfidentialityLevelId,
                    GroupId = addOutboundInternalBasicInfoDTO.GroupId,
                    Hour = addOutboundInternalBasicInfoDTO.Hour,
                    Minute = addOutboundInternalBasicInfoDTO.Minute,
                    Number = addOutboundInternalBasicInfoDTO.Number,
                    PriorityLevelId = addOutboundInternalBasicInfoDTO.PriorityLevelId,
                    Remarks = addOutboundInternalBasicInfoDTO.Remarks,
                    RemindDate = addOutboundInternalBasicInfoDTO.RemindDate,
                    RemindDateH = addOutboundInternalBasicInfoDTO.RemindDateH,
                    TransactionTypeId = addOutboundInternalBasicInfoDTO.TransactionTypeId,
                    Subject = addOutboundInternalBasicInfoDTO.Subject,
                    SubjectClassifications = addOutboundInternalBasicInfoDTO.SubjectClassifications,
                    SuggestedTopicId = addOutboundInternalBasicInfoDTO.SuggestedTopicId,
                    LetterTypeId = addOutboundInternalBasicInfoDTO.LetterTypeId,
                    DeliveryMethod = addOutboundInternalBasicInfoDTO.DeliveryMethod,
                    DeliveryMethodId = addOutboundInternalBasicInfoDTO.DeliveryMethodId,
                    DirectedToId = addOutboundInternalBasicInfoDTO.DirectedToId,
                    DirectedToOrgUnitId = addOutboundInternalBasicInfoDTO.DirectedToOrgUnitId,
                    ReporterId = addOutboundInternalBasicInfoDTO.ReporterId,
                    LetterNumber = addOutboundInternalBasicInfoDTO.LetterNumber,
                    IsElcOutBound = addOutboundInternalBasicInfoDTO.IsElcOutBound,
                    OutBoundDraftNumber = addOutboundInternalBasicInfoDTO.OutBoundDraftNumber,
                    Encrypted = addOutboundInternalBasicInfoDTO.Encrypted

                };
                return addOutboundInternalBasicInfoVM;
            }
            return new AddOutboundInternalBasicInfoVM();
        }
        public static AddOutboundInternalBasicInfoDTO Map(AddOutboundInternalBasicInfoVM addOutboundInternalBasicInfoVM)
        {
            if (addOutboundInternalBasicInfoVM != null)
            {
                AddOutboundInternalBasicInfoDTO addOutboundInternalBasicInfoDTO = new AddOutboundInternalBasicInfoDTO()
                {

                    ConfidentialityLevelId = addOutboundInternalBasicInfoVM.ConfidentialityLevelId,
                    GroupId = addOutboundInternalBasicInfoVM.GroupId,
                    Hour = addOutboundInternalBasicInfoVM.Hour,
                    Minute = addOutboundInternalBasicInfoVM.Minute,
                    Number = addOutboundInternalBasicInfoVM.Number,
                    PriorityLevelId = addOutboundInternalBasicInfoVM.PriorityLevelId,
                    Remarks = addOutboundInternalBasicInfoVM.Remarks,
                    RemindDate = addOutboundInternalBasicInfoVM.RemindDate,
                    RemindDateH = addOutboundInternalBasicInfoVM.RemindDateH,
                    TransactionTypeId = addOutboundInternalBasicInfoVM.TransactionTypeId,
                    Subject = addOutboundInternalBasicInfoVM.Subject,
                    SubjectClassifications = addOutboundInternalBasicInfoVM.SubjectClassifications,
                    SuggestedTopicId = addOutboundInternalBasicInfoVM.SuggestedTopicId,
                    LetterTypeId = addOutboundInternalBasicInfoVM.LetterTypeId,
                    DeliveryMethod = addOutboundInternalBasicInfoVM.DeliveryMethod,
                    DeliveryMethodId = addOutboundInternalBasicInfoVM.DeliveryMethodId,
                    DirectedToId = addOutboundInternalBasicInfoVM.DirectedToId,
                    DirectedToOrgUnitId = addOutboundInternalBasicInfoVM.DirectedToOrgUnitId,
                    ReporterId = addOutboundInternalBasicInfoVM.ReporterId,
                    SubjectClassificationsId = addOutboundInternalBasicInfoVM.SubjectClassificationsId,
                    RecordNumber = addOutboundInternalBasicInfoVM.RecordNumber,
                    privacyLevelId = addOutboundInternalBasicInfoVM.privacyLevelId,
                    LetterNumber = addOutboundInternalBasicInfoVM.LetterNumber,
                    IsElcOutBound = addOutboundInternalBasicInfoVM.IsElcOutBound,
                    OutBoundDraftNumber = addOutboundInternalBasicInfoVM.OutBoundDraftNumber,
                    Summary = addOutboundInternalBasicInfoVM.Summary,
                    Encrypted = addOutboundInternalBasicInfoVM.Encrypted

                };
                return addOutboundInternalBasicInfoDTO;
            }
            return new AddOutboundInternalBasicInfoDTO();
        }
        public static EditOutboundInternalBasicInfoVM Map(EditOutboundInternalBasicInfoDTO editOutboundInternalBasicInfoDTO)
        {
            if (editOutboundInternalBasicInfoDTO != null)
            {
                EditOutboundInternalBasicInfoVM editOutboundInternalBasicInfoVM = new EditOutboundInternalBasicInfoVM()
                {
                    ConfidentialityLevelId = editOutboundInternalBasicInfoDTO.ConfidentialityLevelId,
                    GroupId = editOutboundInternalBasicInfoDTO.GroupId,
                    Hour = editOutboundInternalBasicInfoDTO.Hour,
                    Minute = editOutboundInternalBasicInfoDTO.Minute,
                    Number = editOutboundInternalBasicInfoDTO.Number,
                    PriorityLevelId = editOutboundInternalBasicInfoDTO.PriorityLevelId,
                    Remarks = editOutboundInternalBasicInfoDTO.Remarks,
                    RemindDate = editOutboundInternalBasicInfoDTO.RemindDate,
                    RemindDateH = editOutboundInternalBasicInfoDTO.RemindDateH,
                    TransactionTypeId = editOutboundInternalBasicInfoDTO.TransactionTypeId,
                    Subject = editOutboundInternalBasicInfoDTO.Subject,
                    SubjectClassifications = editOutboundInternalBasicInfoDTO.SubjectClassifications,
                    SuggestedTopicId = editOutboundInternalBasicInfoDTO.SuggestedTopicId,
                    LetterTypeId = editOutboundInternalBasicInfoDTO.LetterTypeId,
                    DeliveryMethod = editOutboundInternalBasicInfoDTO.DeliveryMethod,
                    DeliveryMethodId = editOutboundInternalBasicInfoDTO.DeliveryMethodId,
                    DirectedToId = editOutboundInternalBasicInfoDTO.DirectedToId,
                    DirectedToOrgUnitId = editOutboundInternalBasicInfoDTO.DirectedToOrgUnitId,
                    ReporterId = editOutboundInternalBasicInfoDTO.ReporterId,
                    ProcessPeriodTransaction = editOutboundInternalBasicInfoDTO.ProcessPeriodTransaction,
                    SubjectClassificationsId = editOutboundInternalBasicInfoDTO.SubjectClassificationsId,
                    RecordNumber = editOutboundInternalBasicInfoDTO.RecordNumber,
                    ConfidentialityLevelText = editOutboundInternalBasicInfoDTO.ConfidentialityLevelText,
                    CreatedDateH = editOutboundInternalBasicInfoDTO.CreatedDateH,
                    EntityName = editOutboundInternalBasicInfoDTO.EntityName,
                     privacyLevelId = editOutboundInternalBasicInfoDTO.privacyLevelId,
                    LetterNumber = editOutboundInternalBasicInfoDTO.LetterNumber,
                    IsElcOutBound = editOutboundInternalBasicInfoDTO.IsElcOutBound,
                    OutBoundDraftNumber = editOutboundInternalBasicInfoDTO.OutBoundDraftNumber,
                    Summary = editOutboundInternalBasicInfoDTO.Summary,
                    Encrypted = editOutboundInternalBasicInfoDTO.Encrypted

                };
                return editOutboundInternalBasicInfoVM;
            }
            return new EditOutboundInternalBasicInfoVM();
        }

        public static VIPEditOutboundInternalBasicInfoVM VIPMap(EditOutboundInternalBasicInfoDTO editOutboundInternalBasicInfoDTO)
        {
            if (editOutboundInternalBasicInfoDTO != null)
            {
                VIPEditOutboundInternalBasicInfoVM editOutboundInternalBasicInfoVM = new VIPEditOutboundInternalBasicInfoVM()
                {
                    ConfidentialityLevelId = editOutboundInternalBasicInfoDTO.ConfidentialityLevelId,
                    PriorityLevelId = editOutboundInternalBasicInfoDTO.PriorityLevelId,
                    Hour = editOutboundInternalBasicInfoDTO.Hour,
                    Minute = editOutboundInternalBasicInfoDTO.Minute,
                    Number = editOutboundInternalBasicInfoDTO.Number,
                    RemindDate = editOutboundInternalBasicInfoDTO.RemindDate,
                    RemindDateH = editOutboundInternalBasicInfoDTO.RemindDateH,
                    SubjectClassifications = editOutboundInternalBasicInfoDTO.SubjectClassifications,
                    SuggestedTopicId = editOutboundInternalBasicInfoDTO.SuggestedTopicId,
                    LetterTypeId = editOutboundInternalBasicInfoDTO.LetterTypeId,
                    ConfidentialityLevelText = editOutboundInternalBasicInfoDTO.ConfidentialityLevelText,
                    CreatedDateH = editOutboundInternalBasicInfoDTO.CreatedDateH,
                    //EntityName = editOutboundInternalBasicInfoDTO.EntityName,
                    Subject = editOutboundInternalBasicInfoDTO.Subject,
                    LetterNumber = editOutboundInternalBasicInfoDTO.LetterNumber,
                    IsElcOutBound = editOutboundInternalBasicInfoDTO.IsElcOutBound,
                    OutBoundDraftNumber = editOutboundInternalBasicInfoDTO.OutBoundDraftNumber,
                    PriorityLeveText = editOutboundInternalBasicInfoDTO.PriorityLeveText,
                    Summary = editOutboundInternalBasicInfoDTO.Summary,
                };
                return editOutboundInternalBasicInfoVM;
            }
            return new VIPEditOutboundInternalBasicInfoVM();
        }
        public static EditOutboundInternalBasicInfoDTO Map(EditOutboundInternalBasicInfoVM editOutboundInternalBasicInfoVM)
        {
            if (editOutboundInternalBasicInfoVM != null)
            {
                DateTime? dt = null;
                if (editOutboundInternalBasicInfoVM.RemindDate.HasValue)
                {
                    DateTime d = editOutboundInternalBasicInfoVM.RemindDate.Value;
                    dt = new DateTime(d.Year, d.Month, d.Day, 0, 0, 0);
                }
                EditOutboundInternalBasicInfoDTO editOutboundInternalBasicInfoDTO = new EditOutboundInternalBasicInfoDTO()
                {

                    ConfidentialityLevelId = editOutboundInternalBasicInfoVM.ConfidentialityLevelId,
                    GroupId = editOutboundInternalBasicInfoVM.GroupId,
                    Hour = editOutboundInternalBasicInfoVM.Hour,
                    Minute = editOutboundInternalBasicInfoVM.Minute,
                    Number = editOutboundInternalBasicInfoVM.Number,
                    PriorityLevelId = editOutboundInternalBasicInfoVM.PriorityLevelId,
                    Remarks = editOutboundInternalBasicInfoVM.Remarks,
                    RemindDate = dt,
                    RemindDateH = editOutboundInternalBasicInfoVM.RemindDateH,
                    TransactionTypeId = editOutboundInternalBasicInfoVM.TransactionTypeId,
                    Subject = editOutboundInternalBasicInfoVM.Subject,
                    SubjectClassifications = editOutboundInternalBasicInfoVM.SubjectClassifications,
                    SuggestedTopicId = editOutboundInternalBasicInfoVM.SuggestedTopicId,
                    LetterTypeId = editOutboundInternalBasicInfoVM.LetterTypeId,
                    DeliveryMethod = editOutboundInternalBasicInfoVM.DeliveryMethod,
                    DeliveryMethodId = editOutboundInternalBasicInfoVM.DeliveryMethodId,
                    DirectedToId = editOutboundInternalBasicInfoVM.DirectedToId,
                    DirectedToOrgUnitId = editOutboundInternalBasicInfoVM.DirectedToOrgUnitId,
                    ReporterId = editOutboundInternalBasicInfoVM.ReporterId,
                    ProcessPeriodTransaction = editOutboundInternalBasicInfoVM.ProcessPeriodTransaction,
                    SubjectClassificationsId = editOutboundInternalBasicInfoVM.SubjectClassificationsId,
                    privacyLevelId = editOutboundInternalBasicInfoVM.privacyLevelId,
                    LetterNumber = editOutboundInternalBasicInfoVM.LetterNumber,
                    IsElcOutBound = editOutboundInternalBasicInfoVM.IsElcOutBound,
                    OutBoundDraftNumber = editOutboundInternalBasicInfoVM.OutBoundDraftNumber,
                    Summary = editOutboundInternalBasicInfoVM.Summary,
                    Encrypted = editOutboundInternalBasicInfoVM.Encrypted


                };
                return editOutboundInternalBasicInfoDTO;
            }
            return new EditOutboundInternalBasicInfoDTO();
        }

    }
}