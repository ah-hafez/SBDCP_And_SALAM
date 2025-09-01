using MCS.Domain;
using MCS.DTO;
using System.Linq;

namespace MCS.YESSER.Proxy
{
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
                        Id = documentDTO.Id,

                    };
                }

                DocumentInfo documentInfo = new DocumentInfo()
                {
                    Id = documentDTO.Id,
                    MimeType = documentDTO.MimeType,
                    Name = documentDTO.Name,
                    Size = documentDTO.Size,
                    Document = document,
                    IsDeleted = documentDTO.IsDeleted
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
                    Content = documentInfo.Document != null ? documentInfo.Document.Content : null
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
                    Content = documentInfo.Document != null ? documentInfo.Document.Content : null
                };

                return documentDTO;
            }
            return null;
        }
    }
}