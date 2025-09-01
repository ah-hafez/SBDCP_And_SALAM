using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class DistributionList : EntityBase
    {
        public int? UserId { get; set; }
        public int OrgUnitId { get; set; }
        public int LocalizationIdentifierId { get; set; }
        public virtual UserProfile User { get; set; }
        public virtual OrgUnit OrgUnit { get; set; }
        public virtual LocalizationIdentifier Name { get; set; }
        public IList<DistributionListDetails> DistributionListDetails { get; set; }
    }
}
