using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Business;
using MCS.Domain;
using MCS.DTO.ExternalParties;

namespace MCS.Service.Mappers
{
    public static class ExternalPartyAttachmentMapper
    {
        public static ExternalPartyAttachment Map(ExternalPartyAttachmentDTO externalPartyAttachmentDTO)
        {
            if (externalPartyAttachmentDTO == null)
            {
                return null;
            }

            IExternalPartyBL externalPartyBL = IoC.Resolve<IExternalPartyBL>();

            ExternalPartyAttachment externalPartyAttachment = new ExternalPartyAttachment
            {
                DocumentInfo = DocumentMapper.Map(externalPartyAttachmentDTO.DocumentDTO),
                PartyId = externalPartyAttachmentDTO.PartyId,
                Name = externalPartyAttachmentDTO.Name
            };

            return externalPartyAttachment;
        }

        public static ExternalPartyAttachmentDTO Map(ExternalPartyAttachment externalPartyAttachment)
        {
            if (externalPartyAttachment == null)
            {
                return null;
            }

            ExternalPartyAttachmentDTO externalPartyAttachmentDTO = new ExternalPartyAttachmentDTO
            {
                Id = externalPartyAttachment.Id,
                DocumentDTO = DocumentMapper.Map(externalPartyAttachment.DocumentInfo),
                PartyId = externalPartyAttachment.PartyId,
                Name = externalPartyAttachment.Name,
                IsDeleted = externalPartyAttachment.IsDeleted
            };

            return externalPartyAttachmentDTO;
        }

        public static List<ExternalPartyAttachmentDTO> Map(IList<ExternalPartyAttachment> externalPartyAttachments)
        {
            if (externalPartyAttachments == null || !externalPartyAttachments.Any())
            {
                return null;
            }
            List<ExternalPartyAttachmentDTO> externalPartyAttachmentDTOs = externalPartyAttachments
                .Select(Attachment => new ExternalPartyAttachmentDTO()
                {
                    Id = Attachment.Id,
                    PartyId = Attachment.PartyId,
                    DocumentDTO = DocumentMapper.Map(Attachment.DocumentInfo),
                    Name = Attachment.Name,
                    IsDeleted = Attachment.IsDeleted
                }).ToList();


            return externalPartyAttachmentDTOs;

        }

        public static List<ExternalPartyAttachment> Map(IList<ExternalPartyAttachmentDTO> externalPartyAttachmentDTOs)
        {
            if (externalPartyAttachmentDTOs == null || !externalPartyAttachmentDTOs.Any())
            {
                return null;
            }
            List<ExternalPartyAttachment> externalPartyAttachments = externalPartyAttachmentDTOs
                .Select(Attachment => new ExternalPartyAttachment()
                {
                    Id = Attachment.Id,
                    PartyId = Attachment.PartyId,
                    DocumentInfo = DocumentMapper.Map(Attachment.DocumentDTO),
                    Name = Attachment.Name,
                    IsDeleted = Attachment.IsDeleted

                }).ToList();


            return externalPartyAttachments;

        }


    }
}