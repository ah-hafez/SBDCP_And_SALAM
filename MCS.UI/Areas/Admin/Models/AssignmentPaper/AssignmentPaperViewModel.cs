using MCS.DTO;
using MCS.UI.Areas.Admin.Models.OrgUnit;

namespace MCS.UI.Areas.Admin.Models.AssignmentPaper
{
    public class AssignmentPaperViewModel
    {
        public bool IsCreateGroupAllowed { get; set; }
        public AssignmentPaperActionVM ActionVM { get; set; }
        public AssignmentPaperBeneficiaryVM BeneficiaryVM { get; set; }

        public AssignmentPaperViewModel()
        {
            ActionVM = new AssignmentPaperActionVM();
            BeneficiaryVM = new AssignmentPaperBeneficiaryVM();
        }
    }
}