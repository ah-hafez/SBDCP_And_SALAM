using System.Collections.Generic;
using System.Linq;
using MCS.Framework.Encryption;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class TransactionArchiveMapper
    {
        public static List<TransactionArchiveVM> Map(IList<TransactionArchiveDTO> transactionArchiveDTOs)
        {
            if (transactionArchiveDTOs == null || !transactionArchiveDTOs.Any())
            {
                return new List<TransactionArchiveVM>();
            }
            List<TransactionArchiveVM> transactionArchiveVMs = transactionArchiveDTOs
                .Select(transactionArchiveDTO => new TransactionArchiveVM()
                { 
                    ArcivingTypeName = transactionArchiveDTO.ArcivingTypeName,
                    AttachmentTypeId = transactionArchiveDTO.AttachmentTypeId,
                    DocumentId = transactionArchiveDTO.DocumentId,
                    Id = transactionArchiveDTO.Id,
                    EncryptDocumentId = AESEncrytDecry.Base64Encode(transactionArchiveDTO.DocumentId.ToString()),
                    IsDeleted = transactionArchiveDTO.IsDeleted,
                    IsMainDocument = transactionArchiveDTO.IsMainDocument,
                    IsNew = transactionArchiveDTO.IsNew,
                    TransactionAttachmentType = transactionArchiveDTO.TransactionAttachmentType
                }).ToList();

            return transactionArchiveVMs;
        }
        public static List<TransactionArchiveDTO> Map(IList<TransactionArchiveVM> transactionArchiveVMs)
        {
            if (transactionArchiveVMs == null || !transactionArchiveVMs.Any())
            {
                return new List<TransactionArchiveDTO>();
            }
            List<TransactionArchiveDTO> transactionArchiveDTOs = transactionArchiveVMs
                .Select(transactionArchiveVM => new TransactionArchiveDTO()
                {
                    ArcivingTypeName = transactionArchiveVM.ArcivingTypeName,
                    AttachmentTypeId = transactionArchiveVM.AttachmentTypeId,
                    DocumentId = transactionArchiveVM.DocumentId,
                    Id = transactionArchiveVM.Id,
                    IsDeleted = transactionArchiveVM.IsDeleted,
                    IsMainDocument = transactionArchiveVM.IsMainDocument,
                    IsNew = transactionArchiveVM.IsNew,
                    TransactionAttachmentType = transactionArchiveVM.TransactionAttachmentType
                }).ToList();

            return transactionArchiveDTOs;
        }

    }
}