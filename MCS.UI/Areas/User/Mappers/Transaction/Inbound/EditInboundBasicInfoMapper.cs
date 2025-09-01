using System;
using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction.Inbound;

namespace MCS.UI.Areas.User.Mappers.Transaction.Inbound
{
    public class EditInboundBasicInfoMapper
    {
        public static List<EditInboundBasicInfoVM> Map(IList<EditInboundBasicInfoDTO> editInboundBasicInfoDTOs)
        {
            if (editInboundBasicInfoDTOs == null || !editInboundBasicInfoDTOs.Any())
            {
                return new List<EditInboundBasicInfoVM>();
            }
            List<EditInboundBasicInfoVM> editInboundBasicInfoVMs = editInboundBasicInfoDTOs
                .Select(editInboundBasicInfoDTO => new EditInboundBasicInfoVM()
                {
                    ConfidentialityLevelId = editInboundBasicInfoDTO.ConfidentialityLevelId,
                    DestinationId = editInboundBasicInfoDTO.DestinationId,
                    DirectedToId = editInboundBasicInfoDTO.DirectedToId,
                    DirectedToOrgUnitId = editInboundBasicInfoDTO.DirectedToOrgUnitId,
                    Hour = editInboundBasicInfoDTO.Hour,
                    InboundDocumentNumber = editInboundBasicInfoDTO.InboundDocumentNumber,
                    InboundNumber = editInboundBasicInfoDTO.InboundNumber,
                    Minute = editInboundBasicInfoDTO.Minute,
                    PriorityLevelId = editInboundBasicInfoDTO.PriorityLevelId,
                    Remarks = editInboundBasicInfoDTO.Remarks,
                    RemindDate = editInboundBasicInfoDTO.RemindDate,
                    RemindDateH = editInboundBasicInfoDTO.RemindDateH,
                    SignedById = editInboundBasicInfoDTO.SignedById,
                    SignedByOrgUnitId = editInboundBasicInfoDTO.SignedByOrgUnitId,
                    TransactionTypeId = editInboundBasicInfoDTO.TransactionTypeId,
                    Subject = editInboundBasicInfoDTO.Subject,
                    SubjectClassifications = editInboundBasicInfoDTO.SubjectClassifications,
                    SuggestedTopicId = editInboundBasicInfoDTO.SuggestedTopicId,
                    LetterTypeId = editInboundBasicInfoDTO.LetterTypeId,
                    DeliveryMethod = editInboundBasicInfoDTO.DeliveryMethod,
                    DeliveryMethodId = editInboundBasicInfoDTO.DeliveryMethodId,
                    InboundDateH = editInboundBasicInfoDTO.InboundDateH,
                    IsForIndividual = editInboundBasicInfoDTO.IsForIndividual,
                    SideContactExternalEntityID = editInboundBasicInfoDTO.SideContactExternalEntityID,
                    NumberContact = editInboundBasicInfoDTO.NumberContact,
                    LetterNumber = editInboundBasicInfoDTO.LetterNumber,
                    Summary = editInboundBasicInfoDTO.Summary,

                }).ToList();

            return editInboundBasicInfoVMs;
        }
        public static List<EditInboundBasicInfoDTO> Map(IList<EditInboundBasicInfoVM> editInboundBasicInfoVMs)
        {
            if (editInboundBasicInfoVMs == null || !editInboundBasicInfoVMs.Any())
            {
                return new List<EditInboundBasicInfoDTO>();
            }
            List<EditInboundBasicInfoDTO> editInboundBasicInfoDTOs = editInboundBasicInfoVMs
                .Select(editInboundBasicInfoVM => new EditInboundBasicInfoDTO()
                {
                    ConfidentialityLevelId = editInboundBasicInfoVM.ConfidentialityLevelId,
                    DestinationId = editInboundBasicInfoVM.DestinationId,
                    DirectedToId = editInboundBasicInfoVM.DirectedToId,
                    DirectedToOrgUnitId = editInboundBasicInfoVM.DirectedToOrgUnitId,
                    Hour = editInboundBasicInfoVM.Hour,
                    InboundDocumentNumber = editInboundBasicInfoVM.InboundDocumentNumber,
                    InboundNumber = editInboundBasicInfoVM.InboundNumber,
                    Minute = editInboundBasicInfoVM.Minute,
                    PriorityLevelId = editInboundBasicInfoVM.PriorityLevelId,
                    Remarks = editInboundBasicInfoVM.Remarks,
                    RemindDate = editInboundBasicInfoVM.RemindDate,
                    RemindDateH = editInboundBasicInfoVM.RemindDateH,
                    SignedById = editInboundBasicInfoVM.SignedById,
                    SignedByOrgUnitId = editInboundBasicInfoVM.SignedByOrgUnitId,
                    TransactionTypeId = editInboundBasicInfoVM.TransactionTypeId,
                    Subject = editInboundBasicInfoVM.Subject,
                    SubjectClassifications = editInboundBasicInfoVM.SubjectClassifications,
                    SuggestedTopicId = editInboundBasicInfoVM.SuggestedTopicId,
                    LetterTypeId = editInboundBasicInfoVM.LetterTypeId,
                    DeliveryMethod = editInboundBasicInfoVM.DeliveryMethod,
                    DeliveryMethodId = editInboundBasicInfoVM.DeliveryMethodId,
                    InboundDateH = editInboundBasicInfoVM.InboundDateH,
                    IsForIndividual = editInboundBasicInfoVM.IsForIndividual,
                    SideContactExternalEntityID = editInboundBasicInfoVM.SideContactExternalEntityID,
                    NumberContact = editInboundBasicInfoVM.NumberContact,
                    LetterNumber = editInboundBasicInfoVM.LetterNumber,
                    Summary = editInboundBasicInfoVM.Summary,

                }).ToList();

            return editInboundBasicInfoDTOs;
        }
        public static EditInboundBasicInfoDTO Map(EditInboundBasicInfoVM editInboundBasicInfoVM)
        {
            if (editInboundBasicInfoVM != null)
            {
                DateTime? dt = null;
                if (editInboundBasicInfoVM.RemindDate.HasValue)
                {
                    DateTime d = editInboundBasicInfoVM.RemindDate.Value;
                    dt = new DateTime(d.Year, d.Month, d.Day, 0, 0, 0);
                }
                EditInboundBasicInfoDTO editInboundBasicInfoDTO = new EditInboundBasicInfoDTO()
                {
                    ConfidentialityLevelId = editInboundBasicInfoVM.ConfidentialityLevelId,
                    DestinationId = editInboundBasicInfoVM.DestinationId,
                    DirectedToId = editInboundBasicInfoVM.DirectedToId,
                    DirectedToOrgUnitId = editInboundBasicInfoVM.DirectedToOrgUnitId,
                    Hour = editInboundBasicInfoVM.Hour,
                    InboundDocumentNumber = editInboundBasicInfoVM.InboundDocumentNumber,
                    InboundNumber = editInboundBasicInfoVM.InboundNumber,
                    Minute = editInboundBasicInfoVM.Minute,
                    PriorityLevelId = editInboundBasicInfoVM.PriorityLevelId,
                    Remarks = editInboundBasicInfoVM.Remarks,
                    RemindDate = dt,
                    RemindDateH = editInboundBasicInfoVM.RemindDateH,
                    SignedById = editInboundBasicInfoVM.SignedById,
                    SignedByOrgUnitId = editInboundBasicInfoVM.SignedByOrgUnitId,
                    TransactionTypeId = editInboundBasicInfoVM.TransactionTypeId,
                    Subject = editInboundBasicInfoVM.Subject,
                    SubjectClassifications = editInboundBasicInfoVM.SubjectClassifications,
                    SuggestedTopicId = editInboundBasicInfoVM.SuggestedTopicId,
                    LetterTypeId = editInboundBasicInfoVM.LetterTypeId,
                    DeliveryMethod = editInboundBasicInfoVM.DeliveryMethod,
                    DeliveryMethodId = editInboundBasicInfoVM.DeliveryMethodId,
                    InboundDateH = editInboundBasicInfoVM.InboundDateH,
                    IsForIndividual = editInboundBasicInfoVM.IsForIndividual,
                    ReporterId = editInboundBasicInfoVM.ReporterId,
                    InboundIntendedPerson = editInboundBasicInfoVM.InboundIntendedPerson,
                    SubjectClassificationsId = editInboundBasicInfoVM.SubjectClassificationsId,
                    SideContactExternalEntityID = editInboundBasicInfoVM.SideContactExternalEntityID,
                    NumberContact = editInboundBasicInfoVM.NumberContact,
                    ContactDateH = editInboundBasicInfoVM.ContactDateH,
                    LetterNumber = editInboundBasicInfoVM.LetterNumber,
                    CityId = editInboundBasicInfoVM.CityId,
                    Summary = editInboundBasicInfoVM.Summary,
                };
                return editInboundBasicInfoDTO;
            }
            return new EditInboundBasicInfoDTO();
        }

        public static EditInboundBasicInfoDTO Map(VIPEditInboundBasicInfoVM editInboundBasicInfoVM)
        {
            if (editInboundBasicInfoVM != null)
            {
                DateTime? dt = null;
                if (editInboundBasicInfoVM.RemindDate.HasValue)
                {
                    DateTime d = editInboundBasicInfoVM.RemindDate.Value;
                    dt = new DateTime(d.Year, d.Month, d.Day, 0, 0, 0);
                }
                EditInboundBasicInfoDTO editInboundBasicInfoDTO = new EditInboundBasicInfoDTO()
                {
                    ConfidentialityLevelId = editInboundBasicInfoVM.ConfidentialityLevelId,
                    DestinationId = editInboundBasicInfoVM.DestinationId,
                    DirectedToId = editInboundBasicInfoVM.DirectedToId,
                    DirectedToOrgUnitId = editInboundBasicInfoVM.DirectedToOrgUnitId,
                    Hour = editInboundBasicInfoVM.Hour,
                    InboundDocumentNumber = editInboundBasicInfoVM.InboundDocumentNumber,
                    InboundNumber = editInboundBasicInfoVM.InboundNumber,
                    Minute = editInboundBasicInfoVM.Minute,
                    PriorityLevelId = editInboundBasicInfoVM.PriorityLevelId,
                    Remarks = editInboundBasicInfoVM.Remarks,
                    RemindDate = dt,
                    RemindDateH = editInboundBasicInfoVM.RemindDateH,
                    SignedById = editInboundBasicInfoVM.SignedById,
                    SignedByOrgUnitId = editInboundBasicInfoVM.SignedByOrgUnitId,
                    TransactionTypeId = editInboundBasicInfoVM.TransactionTypeId,
                    Subject = editInboundBasicInfoVM.Subject,
                    SubjectClassifications = editInboundBasicInfoVM.SubjectClassifications,
                    SuggestedTopicId = editInboundBasicInfoVM.SuggestedTopicId,
                    LetterTypeId = editInboundBasicInfoVM.LetterTypeId,
                    DeliveryMethod = editInboundBasicInfoVM.DeliveryMethod,
                    DeliveryMethodId = editInboundBasicInfoVM.DeliveryMethodId,
                    InboundDateH = editInboundBasicInfoVM.InboundDateH,
                    IsForIndividual = editInboundBasicInfoVM.IsForIndividual,
                    ReporterId = editInboundBasicInfoVM.ReporterId,
                    InboundIntendedPerson = editInboundBasicInfoVM.InboundIntendedPerson,
                    SubjectClassificationsId = editInboundBasicInfoVM.SubjectClassificationsId,
                    SideContactExternalEntityID = editInboundBasicInfoVM.SideContactExternalEntityID,
                    NumberContact = editInboundBasicInfoVM.NumberContact,
                    LetterNumber = editInboundBasicInfoVM.LetterNumber,
                    Summary = editInboundBasicInfoVM.Summary,
                };
                return editInboundBasicInfoDTO;
            }
            return new EditInboundBasicInfoDTO();
        }
        public static EditInboundBasicInfoVM Map(EditInboundBasicInfoDTO editInboundBasicInfoDTO)
        {
            if (editInboundBasicInfoDTO != null)
            {
                EditInboundBasicInfoVM editInboundBasicInfoVM = new EditInboundBasicInfoVM()
                {
                    ConfidentialityLevelId = editInboundBasicInfoDTO.ConfidentialityLevelId,
                    DestinationId = editInboundBasicInfoDTO.DestinationId,
                    DirectedToId = editInboundBasicInfoDTO.DirectedToId,
                    DirectedToOrgUnitId = editInboundBasicInfoDTO.DirectedToOrgUnitId,
                    Hour = editInboundBasicInfoDTO.Hour,
                    InboundDocumentNumber = editInboundBasicInfoDTO.InboundDocumentNumber,
                    InboundNumber = editInboundBasicInfoDTO.InboundNumber,
                    Minute = editInboundBasicInfoDTO.Minute,
                    PriorityLevelId = editInboundBasicInfoDTO.PriorityLevelId,
                    Remarks = editInboundBasicInfoDTO.Remarks,
                    RemindDate = editInboundBasicInfoDTO.RemindDate,
                    RemindDateH = editInboundBasicInfoDTO.RemindDateH,
                    SignedById = editInboundBasicInfoDTO.SignedById,
                    SignedByOrgUnitId = editInboundBasicInfoDTO.SignedByOrgUnitId,
                    TransactionTypeId = editInboundBasicInfoDTO.TransactionTypeId,
                    Subject = editInboundBasicInfoDTO.Subject,
                    SubjectClassifications = editInboundBasicInfoDTO.SubjectClassifications,
                    SuggestedTopicId = editInboundBasicInfoDTO.SuggestedTopicId,
                    LetterTypeId = editInboundBasicInfoDTO.LetterTypeId,
                    DeliveryMethod = editInboundBasicInfoDTO.DeliveryMethod,
                    DeliveryMethodId = editInboundBasicInfoDTO.DeliveryMethodId,
                    OutboundDraftId = editInboundBasicInfoDTO.OutboundDraftId,
                    InboundDateH = editInboundBasicInfoDTO.InboundDateH,
                    IsForIndividual = editInboundBasicInfoDTO.IsForIndividual,
                    ReporterId = editInboundBasicInfoDTO.ReporterId,
                    InboundIntendedPerson = editInboundBasicInfoDTO.InboundIntendedPerson,
                    ProcessPeriodTransaction = (int)editInboundBasicInfoDTO.ProcessPeriodTransaction,
                    SubjectClassificationsId = editInboundBasicInfoDTO.SubjectClassificationsId,
                    SideContactExternalEntityID = editInboundBasicInfoDTO.SideContactExternalEntityID,
                    NumberContact = editInboundBasicInfoDTO.NumberContact,
                    ContactDateH = editInboundBasicInfoDTO.ContactDateH,
                    LetterNumber = editInboundBasicInfoDTO.LetterNumber,
                    CityId = editInboundBasicInfoDTO.CityId,
                    Summary = editInboundBasicInfoDTO.Summary,  
                };
                return editInboundBasicInfoVM;
            }
            return new EditInboundBasicInfoVM();
        }
        public static VIPEditInboundBasicInfoVM VIPMap(EditInboundBasicInfoDTO editInboundBasicInfoDTO)
        {
            if (editInboundBasicInfoDTO != null)
            {
                VIPEditInboundBasicInfoVM editInboundBasicInfoVM = new VIPEditInboundBasicInfoVM()
                {
                    ConfidentialityLevelId = editInboundBasicInfoDTO.ConfidentialityLevelId,

                    Hour = editInboundBasicInfoDTO.Hour,

                    Minute = editInboundBasicInfoDTO.Minute,

                    RemindDate = editInboundBasicInfoDTO.RemindDate,
                    RemindDateH = editInboundBasicInfoDTO.RemindDateH,
                    SubjectClassifications = editInboundBasicInfoDTO.SubjectClassifications,
                    SuggestedTopicId = editInboundBasicInfoDTO.SuggestedTopicId,
                    LetterTypeId = editInboundBasicInfoDTO.LetterTypeId,
                    InboundNumber = editInboundBasicInfoDTO.InboundNumber,
                    ConfidentialityLevelText = editInboundBasicInfoDTO.ConfidentialityLevelText,
                    EntityName = editInboundBasicInfoDTO.EntityName,
                    CreatedDateH = editInboundBasicInfoDTO.CreatedDateH,
                    Subject = editInboundBasicInfoDTO.Subject,
                    PriorityLevelId = editInboundBasicInfoDTO.PriorityLevelId,
                    LetterNumber = editInboundBasicInfoDTO.LetterNumber,
                    DeliveryMethodId = editInboundBasicInfoDTO.DeliveryMethodId,
                    PriorityLevelText = editInboundBasicInfoDTO.PriorityLevelText,
                    Summary = editInboundBasicInfoDTO.Summary,

                };
                return editInboundBasicInfoVM;
            }
            return new VIPEditInboundBasicInfoVM();
        }
    }
}