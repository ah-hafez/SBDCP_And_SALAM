using System.Collections.Generic;

namespace MCS.DTO.Tenants
{
    public class TenantNotifcationDetailsDto
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsSent { get; set; }
        public int FailureCount { get; set; }
        public string Email { get; set; }
        public IList<AttachmentDTO> tenantNotificationAttachment { get; set; }

    }
}
