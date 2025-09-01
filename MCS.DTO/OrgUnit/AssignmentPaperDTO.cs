using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class AssignmentPaperDTO
    {
        public int Id { get; set; }
        public List<AssignmentPaperActionDTO> Actions { get; set; }
        public List<AssignmentPaperBeneficiaryDTO> Beneficiaries { get; set; }

        //[CustomDisplayName("Admin.AssignmentPaper.IsCreateGroupAllowable")]
        public bool IsCreateGroupAllowed { get; set; }
        public int OrderNo { get; set; }



    }
}
