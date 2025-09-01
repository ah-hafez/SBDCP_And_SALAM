using MCS.DTO;
using MCS.IntegrationServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Mappers
{
    public class OutboundExternalBasicInfoMapper
    {
        public static AddOutboundExternalBasicInfoDTO Map(AddOutboundExternalBasicInfoVM addOutboundExternalBasicInfoVM)
        {
            if (addOutboundExternalBasicInfoVM != null)
            {
                return new AddOutboundExternalBasicInfoDTO()
                {
                    ConfidentialityLevelId = addOutboundExternalBasicInfoVM.ConfidentialityLevelId,
                    DestinationId = addOutboundExternalBasicInfoVM.DestinationId.Value,
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
                    isOutboundInternalDraft = addOutboundExternalBasicInfoVM.isOutboundInternalDraft
                };
            }
            return new AddOutboundExternalBasicInfoDTO();
        }
    }
}