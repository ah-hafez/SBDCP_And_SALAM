using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.OrgUnit
{
    public class AssignmentPaperBeneficiaryVM : EntityBase
    {
        public int Id { get; set; }
        [CustomDisplayName("Admin.AssignmentPaperBeneficiaries.User")]
        public int? UserId { get; set; }
        public string UserName { get; set; }

        [CustomDisplayName("Admin.AssignmentPaperBeneficiaries.Unit")]
        [CustomRequired("Admin.AssignmentPaperBeneficiaries.UnitRequired")]
        public int? BeneficiaryOrgUnitId { get; set; }

        public int? UserImageId { get; set; }

        public string OrgUnitName { get; set; }
        public AjaxGrid<AssignmentPaperBeneficiaryVM> AssignmentPaperBeneficiaryVMs { get; set; } = (AjaxGrid<AssignmentPaperBeneficiaryVM>)new AjaxGridFactory().CreateAjaxGrid(new List<AssignmentPaperBeneficiaryVM>(), 1, 0, false);
        public AjaxGrid<AssignmentPaperGroupVM> AssignmentPaperGroupVMs { get; set; } = (AjaxGrid<AssignmentPaperGroupVM>)new AjaxGridFactory().CreateAjaxGrid(new List<AssignmentPaperGroupVM>(), 1, 0, false);
        public AssignmentPaperGroupVM AssignmentPaperGroupVM { get; set; }
        public AssignmentPaperGroupEditVM AssignmentPaperGroupEditVM { get; set; }
        // [CustomDisplayName("اسم المجموعة")]
        [CustomDisplayName("User.AssignmentPaper.GroupName")]
        [CustomRequired("User.AssignmentPaper.GroupName")]
        public int GroupId { get; set; }
        public bool? IsCopy { get; set; }
        public string GroupName { get; set; }
        public bool ChkConstant { get; set; }
        public int OrderNo { get; set; }
        public int DefaultActionId { get; set; }
        public int? AssignmentPaperId { get; set; }
        public string OrgUnitCode { get; set; }
        public AssignmentPaperBeneficiaryVM()
        {
            AssignmentPaperGroupVM = new AssignmentPaperGroupVM();
            AssignmentPaperGroupEditVM = new AssignmentPaperGroupEditVM();
        }

    }
}