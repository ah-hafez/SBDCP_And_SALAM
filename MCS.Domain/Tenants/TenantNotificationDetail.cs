using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public  class TenantNotificationDetail :  EntityBase
    {
        public int TypeId { get; set; }
        public  string Subject { get; set; }
        public  string Body { get; set; }
        public virtual IList<TenantNotificationAttachment> Attachments { set; get; }
        public virtual TenantNotificationTemplate Template { set; get; }
        public bool IsSent { get; set; }
        public int FailureCount { get; set; }
        public string Email { get; set; }
    }
}
