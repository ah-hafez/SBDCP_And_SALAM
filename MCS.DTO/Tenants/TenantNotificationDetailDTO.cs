using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO.Tenants
{
    public class TenantNotificationDetailDTO : BaseDTO
    {
        public int TypeId { get; set; }
        public TenantLookupDTO Type { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<TenantNotificationAttachmentDTO> Attachments { set; get; }
        public TenantNotificationTemplateDTO Template { set; get; }
    }
}
