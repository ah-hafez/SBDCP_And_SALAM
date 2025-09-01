using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO.Tenants
{
    public class TenantNotificationAttachmentDTO : BaseDTO
    {
        public virtual byte[] Binary { get; set; }
        public virtual string FileName { get; set; }
        public virtual string ContentType { get; set; }
        public virtual int ContentLength { get; set; }
    }
}
