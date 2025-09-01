using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class AttachmentExtensionMapper
    {
        public static AttachmentExtensionDTO Map(AttachmentExtension attachmentExtention)
        {
            if (attachmentExtention == null)
            {
                return null;
            }
            return new AttachmentExtensionDTO
            {
                Id = attachmentExtention.Id,
                ExtensionName = attachmentExtention.ExtensionName
            };
        }
        public static List<AttachmentExtensionDTO> Map(List<AttachmentExtension> attachmentExtentions)
        {
            if (attachmentExtentions == null)
            {
                return null;
            }
            return attachmentExtentions.Select(ae => Map(ae)).ToList();
        }
    }
}