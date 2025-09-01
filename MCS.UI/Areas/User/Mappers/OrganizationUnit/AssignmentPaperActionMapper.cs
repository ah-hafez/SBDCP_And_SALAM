using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.OrgUnit;

namespace MCS.UI.Areas.User.Mappers.OrgUnit
{
    public static class AssignmentPaperActionMapper
    {
        public static List<AssignmentPaperActionVM> Map(IList<AssignmentPaperActionDTO> assignmentPaperActionDTOs)
        {
            if (assignmentPaperActionDTOs == null || !assignmentPaperActionDTOs.Any())
            {
                return new List<AssignmentPaperActionVM>();
            }
            List<AssignmentPaperActionVM> assignmentPaperActionVMs = assignmentPaperActionDTOs
                .Select(assignmentPaperActionDTO => new AssignmentPaperActionVM()
                { 
                    Id = assignmentPaperActionDTO.Id,
                    Name = assignmentPaperActionDTO.Name,
                    ActionId = assignmentPaperActionDTO.ActionId,
                    
                }).ToList();

            return assignmentPaperActionVMs;
        }
        public static List<AssignmentPaperActionDTO> Map(IList<AssignmentPaperActionVM> assignmentPaperActionVMs)
        {
            if (assignmentPaperActionVMs == null || !assignmentPaperActionVMs.Any())
            {
                return new List<AssignmentPaperActionDTO>();
            }
            List<AssignmentPaperActionDTO> assignmentPaperActionDTOs = assignmentPaperActionVMs
                .Select(assignmentPaperActionVM => new AssignmentPaperActionDTO()
                { 
                    Id = assignmentPaperActionVM.Id,
                    Name = assignmentPaperActionVM.Name,
                    ActionId = assignmentPaperActionVM.ActionId
                }).ToList();

            return assignmentPaperActionDTOs;
        }
    }
}