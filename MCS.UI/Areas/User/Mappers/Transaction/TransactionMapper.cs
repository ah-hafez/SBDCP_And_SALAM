using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.DTO.Transaction;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Mappers.Transaction.Outbound;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class TransactionMapper
    {
        public static List<TransactionVM> Map(IList<TransactionDTO> transactionDTOs)
        {
            if (transactionDTOs == null || !transactionDTOs.Any())
            {
                return new List<TransactionVM>();
            }
            List<TransactionVM> transactionVMs = transactionDTOs
                .Select(transactionDTO => new TransactionVM()
                { 
                    Attachments = TransactionAttachmentMapper.Map(transactionDTO.Attachments),
                    DocumentVM = DocumentMapper.Map(transactionDTO.DocumentDTO),
                    HijriRecordDate = transactionDTO.HijriRecordDate,
                    Id = transactionDTO.Id,
                    IsSigned = transactionDTO.IsSigned,
                    Links = TransactionLinkMapper.Map(transactionDTO.Links),
                    Names = TransactionNameMapper.Map(transactionDTO.Names),
                    OrgUnitId = transactionDTO.OrgUnitId,
                    RecordDate = transactionDTO.RecordDate,
                    StatusId = transactionDTO.StatusId,
                    UserId = transactionDTO.UserId,
                    ExternalCopies= TransactionExternalCopyMapper.Map(transactionDTO.ExternalCopies)
                }).ToList();

            return transactionVMs;
        }
        public static TransactionVM Map(TransactionDTO transactionDTO)
        {
            if (transactionDTO == null)
            {
                return new TransactionVM();
            }
            TransactionVM transactionVM = new TransactionVM()
            {
                Attachments = TransactionAttachmentMapper.Map(transactionDTO.Attachments),
                DocumentVM = DocumentMapper.Map(transactionDTO.DocumentDTO),
                HijriRecordDate = transactionDTO.HijriRecordDate,
                Id = transactionDTO.Id,
                IsSigned = transactionDTO.IsSigned,
                Links = TransactionLinkMapper.Map(transactionDTO.Links),
                Names = TransactionNameMapper.Map(transactionDTO.Names),
                OrgUnitId = transactionDTO.OrgUnitId,
                RecordDate = transactionDTO.RecordDate,
                StatusId = transactionDTO.StatusId,
                UserId = transactionDTO.UserId,
                ExternalCopies = TransactionExternalCopyMapper.Map(transactionDTO.ExternalCopies)
            };

            return transactionVM;
        }
        public static EditSubjectTransactionDTO Map(EditSubjectTransactionVM editSubjectTransactionVM)
        {
            return new EditSubjectTransactionDTO
            {
                Id = editSubjectTransactionVM.Id,
                Subject = editSubjectTransactionVM.Subject
            };
        }
    }
}