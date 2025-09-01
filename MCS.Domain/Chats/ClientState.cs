using Audit.EntityFramework;
using MCS.Framework.Entities;

namespace MCS.MCM.Domain
{
    [AuditIgnore]
    public class ClientState : EntityBase
    {
        public string ActiveRoom { get; set; }
    }
}