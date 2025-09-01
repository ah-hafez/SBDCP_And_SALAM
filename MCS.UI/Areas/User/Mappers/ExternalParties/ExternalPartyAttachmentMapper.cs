using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using MCS.DTO.ExternalParties;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Models;

namespace MCS.UI.Areas.User.Mappers.ExternalParties
{
    public static class ExternalPartyAttachmentMapper
    {
        public static ExternalPartyAttachmentVM Map(ExternalPartyAttachmentDTO externalPartyAttachmentDTO)
        {
            if (externalPartyAttachmentDTO == null)
            {
                return null;
            }

            ExternalPartyAttachmentVM externalPartyAttachment = new ExternalPartyAttachmentVM
            {
                PartyId = externalPartyAttachmentDTO.PartyId,
                DocumentVM = DocumentMapper.Map(externalPartyAttachmentDTO.DocumentDTO),
                Name = externalPartyAttachmentDTO.Name
            };

            return externalPartyAttachment;
        }

        public static ExternalPartyAttachmentDTO Map(ExternalPartyAttachmentVM externalPartyAttachmentVM)
        {
            if (externalPartyAttachmentVM == null)
            {
                return null;
            }

            ExternalPartyAttachmentDTO externalPartyAttachment = new ExternalPartyAttachmentDTO
            {
                DocumentDTO = DocumentMapper.Map(externalPartyAttachmentVM.DocumentVM),
                PartyId = externalPartyAttachmentVM.PartyId,
                Name = externalPartyAttachmentVM.Name
            };

            return externalPartyAttachment;
        }

        public static List<ExternalPartyAttachmentDTO> Map(IList<ExternalPartyAttachmentVM> externalPartyAttachments)
        {
            if (externalPartyAttachments == null || !externalPartyAttachments.Any())
            {
                return null;
            }
            List<ExternalPartyAttachmentDTO> externalPartyAttachmentDTOs = externalPartyAttachments
                .Select(AttachmentDTO => new ExternalPartyAttachmentDTO()
                {
                    Id = AttachmentDTO.Id,
                    PartyId = AttachmentDTO.PartyId,
                    DocumentDTO = DocumentMapper.Map(AttachmentDTO.DocumentVM),
                    Name = AttachmentDTO.Name,
                    IsDeleted = AttachmentDTO.IsDeleted

                }).ToList();


            return externalPartyAttachmentDTOs;

        }

        public static List<ExternalPartyAttachmentVM> Map(IList<ExternalPartyAttachmentDTO> externalPartyAttachments)
        {
            if (externalPartyAttachments == null || !externalPartyAttachments.Any())
            {
                return null;
            }
            List<ExternalPartyAttachmentVM> externalPartyAttachmentVMs = externalPartyAttachments
                .Select(AttachmentDTO => new ExternalPartyAttachmentVM()
                {
                    Id = AttachmentDTO.Id,
                    PartyId = AttachmentDTO.PartyId,
                    DocumentVM = DocumentMapper.Map(AttachmentDTO.DocumentDTO),
                    Name = AttachmentDTO.Name
                }).ToList();

            return externalPartyAttachmentVMs;

        }

        public static string getAttachmentNames(IList<ExternalPartyAttachmentDTO> externalPartyAttachments)
        {
            string sNames = string.Empty;
            var list = externalPartyAttachments.Select(data => new { Id = data.Id, Name = data.Name, IsDeleted = 0 }).ToList();
            return JsonConvert.SerializeObject(list);
        }

    }
}