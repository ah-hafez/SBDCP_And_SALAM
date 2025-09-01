using System.Collections.Generic;

namespace MCS.Domain
{
    public class Form : LookupBase
    {
        public virtual IList<FormDepartment> Departments { get; set; }
        public virtual DocumentInfo FormContent { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public int? LockedBy { get; set; }
    }
}
