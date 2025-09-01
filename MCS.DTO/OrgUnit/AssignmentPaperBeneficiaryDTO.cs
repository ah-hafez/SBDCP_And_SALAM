using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class AssignmentPaperBeneficiaryDTO
    {
        public int Id { get; set; }
        public int Key { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        //[CustomDisplayName("Admin.AssignmentPaperBeneficiaries.Unit")]
        [CustomRequired("Admin.AssignmentPaperBeneficiaries.UnitRequired")]
        public int BeneficiaryOrgUnitId { get; set; }
        public int? UserImageId { get; set; }
        public string OrgUnitName { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool ChkConstant { get; set; }
        public int OrderNo { get; set; }
        public int DefaultActionId { get; set; }
        public int? AssignmentPaperId { get; set; }
        public int GroupOrderNo { get; set; }
        public string SpecialExplanation { get; set; }



    }
}
