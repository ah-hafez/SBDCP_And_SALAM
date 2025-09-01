
using System.Collections.Generic;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using MCS.Common;

namespace MCS.Domain
{
    public class ExternalParty : EntityBase, IAuditable
    {
        public string Number { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Fax { get; set; }
        public bool IsVirtual { get; set; }
        public virtual LocalizationIdentifier Name { get; set; }
        public virtual LocalizationIdentifier Address { get; set; }
        public ExternalPartyType PartyType { get; set; }
        public int? ParentId { get; set; }
        public virtual ExternalParty Parent { get; set; }
        public virtual IList<ExternalPartyManager> PartyManagers { get; set; }
        public string LocalName { get; set; }
        public string LocalAddress { get; set; }
        public bool HasChilds { get; set; }
        public bool YasserRegistered { get; set; }
        public bool? IsActive { get; set; }
        public string Lineage { get; set; }
    }
}
