using System;
using System.Collections.Generic;
using Audit.EntityFramework;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    [AuditIgnore]
    public class Notification : EntityBase
    {
        public virtual IList<NotificationDetail> Details { set; get; }
        public virtual IList<NotificationUser> Users { set; get; }
        public int SourceId { set; get; }
        public virtual Lookup Source { set; get; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public bool IsRead { get; set; }
    }
}
