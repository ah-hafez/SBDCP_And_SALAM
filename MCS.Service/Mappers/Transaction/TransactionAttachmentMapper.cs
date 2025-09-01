using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class TransactionAttachmentMapper
    {
        public static List<TransactionAttachmentDTO> Map(IList<Attachment> attachments)
        {
            if (attachments == null || !attachments.Any())
            {
                return null;
            }
            List<TransactionAttachmentDTO> attachmentDTOs = attachments
                .Select(attachment => new TransactionAttachmentDTO()
                {
                    Id = attachment.Id,
                    Attachments = attachment.Description,
                    Number = attachment.Count,
                    Archivable = attachment.Type.Archivable,
                    TypeId = attachment.Type.Id,
                    TypeName = attachment.Type.Text,
                    DocumentDTO = DocumentMapper.Map(attachment.DocumentInfo),
                    AttachmentSource = (int)attachment.AttachmentSource,
                    UserId = attachment.CreatedBy.HasValue? attachment.CreatedBy.Value : 0,
                   
                }).ToList();

            return attachmentDTOs;
        }

        public static List<Attachment> Map(List<TransactionAttachmentDTO> attachmentDTOs)
        {
            if (attachmentDTOs == null || !attachmentDTOs.Any())
            {
                return new List<Attachment>();
            }
            List<Attachment> attachments = attachmentDTOs
                .Select(attachmentDTO => new Attachment()
                {
                    Id = attachmentDTO.Id,
                    Count = attachmentDTO.Number,
                    TypeId = (int)attachmentDTO.TypeId,
                    Description = attachmentDTO.Attachments,
                    DocumentInfo = DocumentMapper.Map(attachmentDTO.DocumentDTO),
                    AttachmentSource = (Common.AttachmentSource)attachmentDTO.AttachmentSource
                }).ToList();

            return attachments;
        }

        public static TransactionAttachmentDTO Map(Attachment attachment)
        {
            if (attachment == null)
                return null;

            TransactionAttachmentDTO transactionAttachmentDTO = new TransactionAttachmentDTO()
            {
                Id = attachment.Id,
                Attachments = attachment.Description,
                Number = attachment.Count,
                Archivable = attachment.Type.Archivable,
                TypeId = attachment.Type.Id,
                TypeName = attachment.Type.Text,
                DocumentDTO = DocumentMapper.Map(attachment.DocumentInfo),
                AttachmentSource = (int)attachment.AttachmentSource,
                UserId = attachment.CreatedBy.HasValue ? attachment.CreatedBy.Value : 0,
            };

            return transactionAttachmentDTO;
        }
    }
}