using MCS.DTO;
using MCS.UI.Areas.User.Models.OrgUnit;

namespace MCS.UI.Areas.User.Mappers.OrgUnit
{
    public static class AssignmentPaperMapper
    {
        public static AssignmentPaperVM Map(AssignmentPaperDTO assignmentPaperDTOs)
        {
            if (assignmentPaperDTOs != null)
            {
                AssignmentPaperVM assignmentPaperVMs = new AssignmentPaperVM()
                { 
                    Actions = AssignmentPaperActionMapper.Map(assignmentPaperDTOs.Actions),
                    Beneficiaries = AssignmentPaperBeneficiaryMapper.Map(assignmentPaperDTOs.Beneficiaries),
                    Id = assignmentPaperDTOs.Id,
                    IsCreateGroupAllowed = assignmentPaperDTOs.IsCreateGroupAllowed,

                    
                };
                return assignmentPaperVMs;
            }
            return new AssignmentPaperVM();
        }
        public static AssignmentPaperDTO Map(AssignmentPaperVM assignmentPaperVMs)
        {
            if (assignmentPaperVMs != null)
            {
                AssignmentPaperDTO assignmentPaperDTOs = new AssignmentPaperDTO()
                { 
                    Actions = AssignmentPaperActionMapper.Map(assignmentPaperVMs.Actions),
                    Beneficiaries = AssignmentPaperBeneficiaryMapper.Map(assignmentPaperVMs.Beneficiaries),
                    Id = assignmentPaperVMs.Id,
                    IsCreateGroupAllowed = assignmentPaperVMs.IsCreateGroupAllowed,
                };

                return assignmentPaperDTOs;
            }
            return new AssignmentPaperDTO();
        }
    }
}