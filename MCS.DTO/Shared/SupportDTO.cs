using System.Collections.Generic;

namespace MCS.DTO
{
    public class SupportDTO
    {
        public string Subject { get; set; }
        public string Description { get; set; }
        public string SupportType { get; set; }
        public string Category { get; set; }

        public string ToEmail { get; set; }
        public List<NotificationAttachmentDTO> NotificationAttachmentDTOS { get; set; } = new List<NotificationAttachmentDTO>();
    }
}
