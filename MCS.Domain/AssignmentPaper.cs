using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class AssignmentPaper : EntityBase, IAuditable
    {
        public bool IsCreateGroupAllowed { get; set; }
        public virtual IList<AssignmentPaperAction> AssignmentPaperActions { get; set; }
        public virtual IList<AssignmentPaperBeneficiary> AssignmentPaperBeneficiaries { get; set; }
    }
}
