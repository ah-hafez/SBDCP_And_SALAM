using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.Admin.Models.OrgUnit
{
    public class AssignmentPaperBeneficiaryVM
    {
        public int Id { get; set; }
        public int Key { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }

        [CustomDisplayName("Admin.AssignmentPaperBeneficiaries.Unit")]
        [CustomRequired("Admin.AssignmentPaperBeneficiaries.UnitRequired")]
        public int BeneficiaryOrgUnitId { get; set; }

        public string OrgUnitName { get; set; }
    }
}