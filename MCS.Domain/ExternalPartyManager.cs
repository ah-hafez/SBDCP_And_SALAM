using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class ExternalPartyManager : EntityBase, IAuditable
    {
        public virtual LocalizationIdentifier Name { get; set; }
        public virtual ExternalParty ExternalParty { get; set; }
        public string LocalName { get; set; }
        public string EmailAddress { get; set; }
    }
}
