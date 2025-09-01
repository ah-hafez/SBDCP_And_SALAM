using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    //NotDone
    public class DocumentMapper
    {
        public static DocumentInfo Map(DocumentDTO documentDTO)
        {
            if (documentDTO != null)
            {
                Document document = null;

                if (documentDTO.Content != null && documentDTO.Content.Count() > 0)
                {
                    document = new Document()
                    {
                        Content = documentDTO.Content,
                        Id = documentDTO.DocumentId,

                    };
                }

                DocumentInfo documentInfo = new DocumentInfo()
                {
                    Id = documentDTO.Id,
                    MimeType = documentDTO.MimeType,
                    Name = documentDTO.Name,
                    Size = documentDTO.Size,
                    Document = document,
                    IsDeleted = documentDTO.IsDeleted,
                    FromEntityId = documentDTO.FromEntityId,
                    FromUserId = documentDTO.FromUserId,

                };

                return documentInfo;
            }

            return null;
        }

        public static DocumentDTO Map(DocumentInfo documentInfo)
        {
            if (documentInfo != null)
            {
                DocumentDTO documentDTO = new DocumentDTO()
                {
                    Id = documentInfo.Id,
                    MimeType = documentInfo.MimeType,
                    Name = documentInfo.Name,
                    Size = documentInfo.Size,
                    IsDeleted = documentInfo.IsDeleted,
                    Content = documentInfo?.Document?.Content,
                    FromEntityId = documentInfo.FromEntityId ?? 0,
                    FromUserId = documentInfo.FromUserId ?? 0,
                    FromEntityName = documentInfo.FromEntity != null ? documentInfo.FromEntity.LocalName : string.Empty,
                    FromUserName = documentInfo.FromUser != null ? documentInfo.FromUser.LocalName : string.Empty,
                    DocumentId = documentInfo?.Document?.Id ?? 0
                };

                return documentDTO;
            }

            return null;
        }

        public static DocumentDTO MapWithContent(DocumentInfo documentInfo)
        {
            if (documentInfo != null)
            {
                DocumentDTO documentDTO = new DocumentDTO()
                {
                    Id = documentInfo.Id,
                    MimeType = documentInfo.MimeType,
                    Name = documentInfo.Name,
                    Size = documentInfo.Size,
                    Content = documentInfo.Document != null ? documentInfo.Document.Content : null,
                    FromEntityId = documentInfo.FromEntityId ?? 0,
                    FromUserId = documentInfo.FromUserId ?? 0,
                    FromEntityName = documentInfo.FromEntity != null ? documentInfo.FromEntity.LocalName : string.Empty,
                    FromUserName = documentInfo.FromUser != null ? documentInfo.FromUser.UserName : string.Empty,
                    DocumentId = documentInfo?.Document?.Id ?? 0
                };

                return documentDTO;
            }
            return null;
        }
    }
}