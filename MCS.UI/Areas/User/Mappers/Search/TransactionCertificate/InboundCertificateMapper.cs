using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Assignment;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Mappers.Transaction;
using MCS.UI.Areas.User.Mappers.Transaction.Outbound;
using MCS.UI.Areas.User.Models.Search.TransactionCertificate;

namespace MCS.UI.Areas.User.Mappers.Search.TransactionCertificate
{
    public static class InboundCertificateMapper
    {
        public static InboundCertificateVM Map(InboundCertificateDTO inboundCertificateDTO)
        {
            if (inboundCertificateDTO != null)
            {
                InboundCertificateVM inboundCertificateVM = new InboundCertificateVM()
                {
                    Assignments = TransactionAssignmentMapper.Map(inboundCertificateDTO.Assignments),
                    Copies = TransactionCopyMapper.Map(inboundCertificateDTO.Copies),
                    ExternalCopies = TransactionExternalCopyMapper.Map(inboundCertificateDTO.ExternalCopies),
                    Attachments = TransactionAttachmentMapper.Map(inboundCertificateDTO.Attachments),
                    Explainations = ExplanationMapper.Map(inboundCertificateDTO.Explainations),
                    ConfidentialityLevel = inboundCertificateDTO.ConfidentialityLevel,
                    CreatedByOrgUnit = inboundCertificateDTO.CreatedByOrgUnit,
                    CreatedByUser = inboundCertificateDTO.CreatedByUser,
                    Date = inboundCertificateDTO.Date,
                    Destination = inboundCertificateDTO.Destination,
                    DirectedTo = inboundCertificateDTO.DirectedTo,
                    DocumentVM = DocumentMapper.Map(inboundCertificateDTO.DocumentDTO),
                    HijriDate = inboundCertificateDTO.HijriDate,
                    Id = inboundCertificateDTO.Id,
                    InboundDocumentNumber = inboundCertificateDTO.InboundDocumentNumber,
                    InboundNumber = inboundCertificateDTO.InboundNumber,
                    IsAssignToMoreThanOne = inboundCertificateDTO.IsAssignToMoreThanOne,
                    LatestAssignment = TransactionAssignmentMapper.Map(inboundCertificateDTO.LatestAssignment),
                    Links = TransactionCertificateLinkMapper.Map(inboundCertificateDTO.Links),
                    Names = TransactionNameMapper.Map(inboundCertificateDTO.Names),
                    OrgUnit = inboundCertificateDTO.OrgUnit,
                    PriorityLevel = inboundCertificateDTO.PriorityLevel,
                    RemindDateH = inboundCertificateDTO.RemindDateH,
                    RemindTime = inboundCertificateDTO.RemindTime,
                    SignedBy = inboundCertificateDTO.SignedBy,
                    TransactionType = inboundCertificateDTO.TransactionType,
                    Status = inboundCertificateDTO.Status,
                    Subject = inboundCertificateDTO.Subject,
                    TransactionCertificateHistory = TransactionCertificateHistoryMapper.Map(inboundCertificateDTO.TransactionCertificateHistory),
                    LetterType = inboundCertificateDTO.LetterType,
                    InboundIntendedPerson = inboundCertificateDTO.InboundIntendedPerson,
                    DeliveryMethod = inboundCertificateDTO.DeliveryMethod,
                    HasDate = inboundCertificateDTO.HasDate,
                    IsForIndividual = inboundCertificateDTO.IsForIndividual,
                    Remarks = inboundCertificateDTO.Remarks,
                    ToEntity = inboundCertificateDTO.ToEntity,
                    ProcessPeriodTransaction = inboundCertificateDTO.ProcessPeriodTransaction,
                    SideContactExternalEntityName = inboundCertificateDTO.SideContactExternalEntityName,
                    NumberContact = inboundCertificateDTO.NumberContact,
                    RecordNumber = inboundCertificateDTO.RecordNumber,
                    ConfidentialityId = inboundCertificateDTO.ConfidentialityId,
                    LetterNumber = inboundCertificateDTO.LetterNumber,
                    Encrypted = inboundCertificateDTO.Encrypted,
                    ClassificationName = inboundCertificateDTO.ClassificationName,
                    FileDescription = inboundCertificateDTO.FileDescription,
                    FileNumber = inboundCertificateDTO.FileNumber

                };
                return inboundCertificateVM;
            }
            return new InboundCertificateVM();
        }
        public static InboundCertificateDTO Map(InboundCertificateVM inboundCertificateVM)
        {
            if (inboundCertificateVM != null)
            {
                InboundCertificateDTO inboundCertificateDTO = new InboundCertificateDTO()
                {
                    Assignments = TransactionAssignmentMapper.Map(inboundCertificateVM.Assignments),
                    Copies = TransactionCopyMapper.Map(inboundCertificateVM.Copies),
                    ExternalCopies = TransactionExternalCopyMapper.Map(inboundCertificateVM.ExternalCopies),
                    Attachments = TransactionAttachmentMapper.Map(inboundCertificateVM.Attachments),
                    ConfidentialityLevel = inboundCertificateVM.ConfidentialityLevel,
                    CreatedByOrgUnit = inboundCertificateVM.CreatedByOrgUnit,
                    CreatedByUser = inboundCertificateVM.CreatedByUser,
                    Date = inboundCertificateVM.Date,
                    Destination = inboundCertificateVM.Destination,
                    DirectedTo = inboundCertificateVM.DirectedTo,
                    DocumentDTO = DocumentMapper.Map(inboundCertificateVM.DocumentVM),
                    HijriDate = inboundCertificateVM.HijriDate,
                    Id = inboundCertificateVM.Id,
                    InboundDocumentNumber = inboundCertificateVM.InboundDocumentNumber,
                    InboundNumber = inboundCertificateVM.InboundNumber,
                    IsAssignToMoreThanOne = inboundCertificateVM.IsAssignToMoreThanOne,
                    LatestAssignment = TransactionAssignmentMapper.Map(inboundCertificateVM.LatestAssignment),
                    Links = TransactionCertificateLinkMapper.Map(inboundCertificateVM.Links),
                    Names = TransactionNameMapper.Map(inboundCertificateVM.Names),
                    OrgUnit = inboundCertificateVM.OrgUnit,
                    PriorityLevel = inboundCertificateVM.PriorityLevel,
                    RemindDateH = inboundCertificateVM.RemindDateH,
                    RemindTime = inboundCertificateVM.RemindTime,
                    SignedBy = inboundCertificateVM.SignedBy,
                    TransactionType = inboundCertificateVM.TransactionType,
                    Status = inboundCertificateVM.Status,
                    Subject = inboundCertificateVM.Subject,
                    TransactionCertificateHistory = TransactionCertificateHistoryMapper.Map(inboundCertificateVM.TransactionCertificateHistory),
                    LetterType = inboundCertificateVM.LetterType,
                    InboundIntendedPerson = inboundCertificateVM.InboundIntendedPerson,
                    SideContactExternalEntityName = inboundCertificateVM.SideContactExternalEntityName,
                    NumberContact = inboundCertificateVM.NumberContact
                };
                return inboundCertificateDTO;
            }
            return new InboundCertificateDTO();
        }
    }
}