using System;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class TenantNotificationAttachment : EntityBase
    {
        public virtual Byte[] Binary { get; set; }
        public virtual string FileName { get; set; }
        public virtual string ContentType { get; set; }
        public virtual int ContentLength { get; set; }
    }
}
