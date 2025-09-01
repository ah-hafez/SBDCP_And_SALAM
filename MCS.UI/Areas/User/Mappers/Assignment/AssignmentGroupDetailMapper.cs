using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models;

namespace MCS.UI.Areas.User.Mappers.Assignment
{
    public static class AssignmentGroupDetailMapper
    {
        public static List<AssignmentGroupDetailVM> Map(IList<AssignmentGroupDetailDTO> assignmentGroupDetailDTOs)
        {
            if (assignmentGroupDetailDTOs == null || !assignmentGroupDetailDTOs.Any())
            {
                return new List<AssignmentGroupDetailVM>();
            }
            List<AssignmentGroupDetailVM> assignmentGroupDetails = assignmentGroupDetailDTOs
                .Select(assignmentGroupDetailDTO => new AssignmentGroupDetailVM()
                {  
                    Id = assignmentGroupDetailDTO.Id,
                    OrgUnitId = assignmentGroupDetailDTO.OrgUnitId,
                    OrgUnitName = assignmentGroupDetailDTO.OrgUnitName,
                    UserProfileId = assignmentGroupDetailDTO.UserProfileId,
                    UserProfileName = assignmentGroupDetailDTO.UserProfileName
                }).ToList();

            return assignmentGroupDetails;
        }
        public static List<AssignmentGroupDetailDTO> Map(IList<AssignmentGroupDetailVM> assignmentGroupDetails)
        {
            if (assignmentGroupDetails == null || !assignmentGroupDetails.Any())
            {
                return new List<AssignmentGroupDetailDTO>();
            }
            List<AssignmentGroupDetailDTO> assignmentGroupDetailDTOs = assignmentGroupDetails
                .Select(assignmentGroupDetailVM => new AssignmentGroupDetailDTO
                {
                    Id = assignmentGroupDetailVM.Id,
                    OrgUnitId = assignmentGroupDetailVM.OrgUnitId,
                    OrgUnitName = assignmentGroupDetailVM.OrgUnitName,
                    UserProfileId = assignmentGroupDetailVM.UserProfileId,
                    UserProfileName = assignmentGroupDetailVM.UserProfileName,
                }).ToList();
            return assignmentGroupDetailDTOs;
        }
    }
}