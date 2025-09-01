using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class AttachmentExtensionMapper
    {
        public static AttachmentExtensionVM Map(AttachmentExtensionDTO attachmentExtensionDTO)
        {
            if (attachmentExtensionDTO == null)
            {
                return null;
            }
            return new AttachmentExtensionVM
            {
                Id = attachmentExtensionDTO.Id,
                ExtensionName = attachmentExtensionDTO.ExtensionName
            };
        }
        public static List<AttachmentExtensionVM> Map(List<AttachmentExtensionDTO> attachmentExtensionDTOs)
        {
            if (attachmentExtensionDTOs == null)
            {
                return null;
            }
            return attachmentExtensionDTOs.Select(ae => Map(ae)).ToList();
        }
    }
}