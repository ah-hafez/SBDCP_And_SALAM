using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.Admin.Models.OrgUnit
{
    public class AssignmentPaperVM
    {
        public int Id { get; set; }
        public List<AssignmentPaperActionVM> Actions { get; set; }
        public List<AssignmentPaperBeneficiaryVM> Beneficiaries { get; set; }

        [CustomDisplayName("Admin.AssignmentPaper.IsCreateGroupAllowable")]
        public bool IsCreateGroupAllowed { get; set; }
    }
}