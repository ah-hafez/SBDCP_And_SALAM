using System.Collections.Generic;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class AssignmentGroup : EntityBase
    {
        public string LocalName { get; set; }
        public int OwnerId { get; set; }
        public virtual UserProfile Owner { get; set; }
        public virtual LocalizationIdentifier LocalizationIdentifier { get; set; }
        public virtual IList<AssignmentGroupDetail> AssignmentGroupDetails { get; set; }
 
    }
}
