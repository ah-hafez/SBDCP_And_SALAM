using System;
using System.Collections.Generic;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class TenantNotification : EntityBase
    {
        public virtual IList<TenantNotificationDetail> Details { set; get; }
        public virtual string  DelegatedEmail { set; get; }
        public int SourceId { set; get; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
    }
}
