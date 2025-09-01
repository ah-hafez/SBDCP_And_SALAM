using System.ComponentModel.DataAnnotations.Schema;
using Audit.EntityFramework;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    [AuditIgnore]
    public class Document : EntityBase, IAuditable
    {
        public byte[] Content { get; set; }
    }
}
