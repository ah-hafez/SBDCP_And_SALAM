using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCS.Domain
{
    public class AssignmentPaperBeneficiary : EntityBase, IAuditable
    {
        public int OrgUnitId { get; set; }
        public virtual OrgUnit OrgUnit { get; set; }
        public int? UserId { get; set; }
        public virtual UserProfile User { get; set; }

        [Column("AssignmentPaperGroup_Id")]
        public int AssignmentPaperGroupId { get; set; }
        public virtual AssignmentPaperGroup AssignmentPaperGroup { get; set; }
        public bool ChkConstant { get; set; }
        public int OrderNo { get; set; }
        public int DefaultActionId { get; set; }
        public virtual Action DefaultAction { get; set; }

        [Column("AssignmentPaper_Id")]
        public int? AssignmentPaperId { get; set; }
        public virtual AssignmentPaper AssignmentPaper { get; set; }

    }
}
