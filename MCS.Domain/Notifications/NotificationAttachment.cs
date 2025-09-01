using System;
using Audit.EntityFramework;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    [AuditIgnore]
    public class NotificationAttachment : EntityBase
    {
        public virtual Byte[] Binary { get; set; }
        public virtual string FileName { get; set; }
        public virtual string ContentType { get; set; }
        public virtual int ContentLength { get; set; }
    }
}
