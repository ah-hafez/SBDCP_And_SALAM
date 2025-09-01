using MCS.DTO;
using MCS.Framework.Encryption;
using MCS.UI.Areas.User.Models.Shared;
using System.Collections.Generic;
using System.Linq;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class DocumentMapper
    {
        public static List<DocumentDTO> Map(IList<DocumentVM> documentVMs)
        {
            if (documentVMs == null || !documentVMs.Any())
            {
                return new List<DocumentDTO>();
            }
            List<DocumentDTO> documentDTOs = documentVMs
                .Select(b => new DocumentDTO
                {
                    Id = b.Id,
                    Content = b.Content,
                    IsDeleted = b.IsDeleted,
                    MimeType = b.MimeType,
                    Name = b.Name,
                    Size = b.Size,
                    FromEntityId = b.FromEntityId,
                    FromUserId = b.FromUserId,
                    DocumentId = b.DocumentId
                }).ToList();
            return documentDTOs;
        }
        public static List<DocumentVM> Map(IList<DocumentDTO> documentDTOs)
        {
            if (documentDTOs == null || !documentDTOs.Any())
            {
                return new List<DocumentVM>();
            }
            List<DocumentVM> documentVMs = documentDTOs
                .Select(b => new DocumentVM
                {
                    Id = b.Id,
                    EncryptedId = AESEncrytDecry.Base64Encode(b.Id.ToString()),
                    Content = b.Content,
                    IsDeleted = b.IsDeleted,
                    MimeType = b.MimeType,
                    Name = b.Name,
                    Size = b.Size,
                    FromEntityId = b.FromEntityId ?? 0,
                    FromEntityName = b.FromEntityName,
                    FromUserId = b.FromUserId ?? 0,
                    FromUserName = b.FromUserName,
                    DocumentId = b.DocumentId
                }).ToList();
            return documentVMs;
        }
        public static DocumentVM Map(DocumentDTO documentDTO)
        {
            if (documentDTO != null)
            {
                DocumentVM documentVM = new DocumentVM()
                {
                    Content = documentDTO.Content,
                    Id = documentDTO.Id,
                    EncryptedId = AESEncrytDecry.Base64Encode(documentDTO.Id.ToString()),
                    IsDeleted = documentDTO.IsDeleted,
                    MimeType = documentDTO.MimeType,
                    Name = documentDTO.Name,
                    Size = documentDTO.Size,
                    FromEntityId = documentDTO.FromEntityId ?? 0,
                    FromEntityName = documentDTO.FromEntityName,
                    FromUserId = documentDTO.FromUserId ?? 0,
                    FromUserName = documentDTO.FromUserName,
                    DocumentId = documentDTO.DocumentId
                };

                return documentVM;
            }
            return null;
        }
        public static DocumentDTO Map(DocumentVM documentVM)
        {
            if (documentVM != null)
            {
                DocumentDTO documentDTO = new DocumentDTO()
                {
                    Content = documentVM.Content,
                    Id = documentVM.Id,
                    IsDeleted = documentVM.IsDeleted,
                    MimeType = documentVM.MimeType,
                    Name = documentVM.Name,
                    Size = documentVM.Size,
                    DocumentId = documentVM.DocumentId
                };
                if (documentVM.FromEntityId != 0)
                {
                    documentDTO.FromEntityId = documentVM.FromEntityId;
                }
                if (documentVM.FromUserId != 0)
                {
                    documentDTO.FromUserId = documentVM.FromUserId;
                }
                return documentDTO;
            }
            return null;
        }
    }
}