using System.Collections.Generic;

namespace MCS.Domain
{
    public class SubjectClassification : LookupBase
    {
        public bool IsNew { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsGroup { get; set; }
        public int? ParentId { get; set; }
        public virtual SubjectClassification Parent { get; set; }
        public virtual IList<SubjectOrgUnit> SubjectOrgUnits { get; set; }
    }
}
