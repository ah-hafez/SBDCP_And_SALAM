using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class FormDepartment : EntityBase
    {
        public int FormId { get; set; }
        public int? DepartmentId { get; set; }
        public virtual Form Form { get; set; }
        public virtual OrgUnit Department { get; set; }
    }
}
