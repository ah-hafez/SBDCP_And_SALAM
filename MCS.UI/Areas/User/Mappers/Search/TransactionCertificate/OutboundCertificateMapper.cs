using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Assignment;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Mappers.Transaction;
using MCS.UI.Areas.User.Mappers.Transaction.Outbound;
using MCS.UI.Areas.User.Models.Search.TransactionCertificate;

namespace MCS.UI.Areas.User.Mappers.Search.TransactionCertificate
{
    public static class OutboundCertificateMapper
    {
        public static OutboundCertificateVM Map(OutboundCertificateDTO outboundCertificateDTO)
        {
            if (outboundCertificateDTO != null)
            {
                OutboundCertificateVM outboundCertificateVM = new OutboundCertificateVM()
                {
                    Attachments = TransactionAttachmentMapper.Map(outboundCertificateDTO.Attachments),
                    Copies = TransactionCopyMapper.Map(outboundCertificateDTO.Copies),
                    ExternalCopies = TransactionExternalCopyMapper.Map(outboundCertificateDTO.ExternalCopies),
                    ConfidentialityLevel = outboundCertificateDTO.ConfidentialityLevel,
                    CreatedByOrgUnit = outboundCertificateDTO.CreatedByOrgUnit,
                    CreatedByUser = outboundCertificateDTO.CreatedByUser,
                    Date = outboundCertificateDTO.Date,
                    Destination = outboundCertificateDTO.Destination,
                    DirectedTo = outboundCertificateDTO.DirectedTo,
                    DocumentVM = DocumentMapper.Map(outboundCertificateDTO.DocumentDTO),
                    HijriDate = outboundCertificateDTO.HijriDate,
                    Id = outboundCertificateDTO.Id,
                    Links = TransactionCertificateLinkMapper.Map(outboundCertificateDTO.Links),
                    Names = TransactionNameMapper.Map(outboundCertificateDTO.Names),
                    OrgUnit = outboundCertificateDTO.OrgUnit,
                    PriorityLevel = outboundCertificateDTO.PriorityLevel,
                    RemindDateH = outboundCertificateDTO.RemindDateH,
                    RemindTime = outboundCertificateDTO.RemindTime,
                    IsAssignToMoreThanOne = outboundCertificateDTO.IsAssignToMoreThanOne,
                    LatestAssignment = TransactionAssignmentMapper.Map(outboundCertificateDTO.LatestAssignment),
                    SignedBy = outboundCertificateDTO.SignedBy,
                    TransactionType = outboundCertificateDTO.TransactionType,
                    Status = outboundCertificateDTO.Status,
                    Subject = outboundCertificateDTO.Subject,
                    Assignments = TransactionAssignmentMapper.Map(outboundCertificateDTO.Assignments),
                    TransactionCertificateHistory = TransactionCertificateHistoryMapper.Map(outboundCertificateDTO.TransactionCertificateHistory),
                    OutboundNumber = outboundCertificateDTO.OutboundNumber,
                    HasDate = outboundCertificateDTO.HasDate,
                    Remarks = outboundCertificateDTO.Remarks,
                    ToEntity = outboundCertificateDTO.ToEntity,
                    ProcessPeriodTransaction = outboundCertificateDTO.ProcessPeriodTransaction,
                    ClassificationName = outboundCertificateDTO.ClassificationName,
                    FileNumber = outboundCertificateDTO.FileNumber,
                    FileDescription = outboundCertificateDTO.FileDescription,

                };
                return outboundCertificateVM;
            }
            return new OutboundCertificateVM();
        }
        public static OutboundCertificateDTO Map(OutboundCertificateVM outboundCertificateVM)
        {
            if (outboundCertificateVM != null)
            {
                OutboundCertificateDTO outboundCertificateDTO = new OutboundCertificateDTO()
                {
                    Attachments = TransactionAttachmentMapper.Map(outboundCertificateVM.Attachments),
                    Copies = TransactionCopyMapper.Map(outboundCertificateVM.Copies),
                    ExternalCopies = TransactionExternalCopyMapper.Map(outboundCertificateVM.ExternalCopies),
                    ConfidentialityLevel = outboundCertificateVM.ConfidentialityLevel,
                    CreatedByOrgUnit = outboundCertificateVM.CreatedByOrgUnit,
                    CreatedByUser = outboundCertificateVM.CreatedByUser,
                    Date = outboundCertificateVM.Date,
                    Destination = outboundCertificateVM.Destination,
                    DirectedTo = outboundCertificateVM.DirectedTo,
                    DocumentDTO = DocumentMapper.Map(outboundCertificateVM.DocumentVM),
                    HijriDate = outboundCertificateVM.HijriDate,
                    Id = outboundCertificateVM.Id,
                    Links = TransactionCertificateLinkMapper.Map(outboundCertificateVM.Links),
                    Names = TransactionNameMapper.Map(outboundCertificateVM.Names),
                    OrgUnit = outboundCertificateVM.OrgUnit,
                    PriorityLevel = outboundCertificateVM.PriorityLevel,
                    RemindTime = outboundCertificateVM.RemindTime,
                    RemindDateH = outboundCertificateVM.RemindDateH,
                    SignedBy = outboundCertificateVM.SignedBy,
                    TransactionType = outboundCertificateVM.TransactionType,
                    Status = outboundCertificateVM.Status,
                    Subject = outboundCertificateVM.Subject,
                    TransactionCertificateHistory = TransactionCertificateHistoryMapper.Map(outboundCertificateVM.TransactionCertificateHistory),
                    OutboundNumber = outboundCertificateVM.OutboundNumber
                };
                return outboundCertificateDTO;
            }
            return new OutboundCertificateDTO();
        }
    }
}