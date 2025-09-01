using System.Collections.Generic;
using Audit.EntityFramework;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    [AuditIgnore]
    public class NotificationDetail : EntityBase
    {
        public virtual Lookup NotificationType { get; set; }
        public virtual Lookup NotificationTemplateType { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string  Link { get; set; }
        public string Email { get; set; }
        public bool IsSent { get; set; }
        public int FailureCount { get; set; }
        public virtual IList<NotificationAttachment> Attachments { set; get; }
    }
}
