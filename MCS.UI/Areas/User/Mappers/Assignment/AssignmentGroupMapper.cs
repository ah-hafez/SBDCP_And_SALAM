using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Models.Assignment;

namespace MCS.UI.Areas.User.Mappers.Assignment
{
    public class AssignmentGroupMapper
    {
        public static List<AssignmentGroupVM> Map(IList<AssignmentGroupDTO> assignmentGroupDTOs)
        {
            if (assignmentGroupDTOs == null || !assignmentGroupDTOs.Any())
            {
                return new List<AssignmentGroupVM>();
            }
            List<AssignmentGroupVM> assignmentGroups = assignmentGroupDTOs
                .Select(assignmentGroupDTO => new AssignmentGroupVM()
                {  
                    Id = assignmentGroupDTO.Id,
                    GroupName = LocalizationMapper.Map(assignmentGroupDTO.GroupName),
                    GroupDetails = AssignmentGroupDetailMapper.Map(assignmentGroupDTO.GroupDetails),
                    LocalName = assignmentGroupDTO.LocalName
                }).ToList();

            return assignmentGroups;
        }
        public static List<AssignmentGroupDTO> Map(IList<AssignmentGroupVM> AssignmentGroupVMs)
        {
            if (AssignmentGroupVMs == null || !AssignmentGroupVMs.Any())
            {
                return new List<AssignmentGroupDTO>();
            }
            List<AssignmentGroupDTO> assignmentGroups = AssignmentGroupVMs
                .Select(assignmentGroupVM => new AssignmentGroupDTO()
                { 
                    Id = assignmentGroupVM.Id,
                    GroupName = LocalizationMapper.Map(assignmentGroupVM.GroupName),
                    GroupDetails = AssignmentGroupDetailMapper.Map(assignmentGroupVM.GroupDetails),
                    LocalName = assignmentGroupVM.LocalName
                }).ToList();

            return assignmentGroups;
        }
        public static AssignmentGroupDTO Map(AssignmentGroupVM assignmentGroupVM)
        {
            if (assignmentGroupVM != null)
            {
                AssignmentGroupDTO assignmentGroups = new AssignmentGroupDTO()
                {
                    Id = assignmentGroupVM.Id,
                    GroupName = LocalizationMapper.Map(assignmentGroupVM.GroupName),
                    GroupDetails = AssignmentGroupDetailMapper.Map(assignmentGroupVM.GroupDetails),
                    LocalName = assignmentGroupVM.LocalName
                };

                return assignmentGroups;
            }
            return new AssignmentGroupDTO();
        }
        public static AssignmentGroupVM Map(AssignmentGroupDTO assignmentGroupDTO)
        {
            if (assignmentGroupDTO != null)
            {
                AssignmentGroupVM assignmentGroupVM = new AssignmentGroupVM()
                { 
                    Id = assignmentGroupDTO.Id,
                    GroupName = LocalizationMapper.Map(assignmentGroupDTO.GroupName),
                    GroupDetails = AssignmentGroupDetailMapper.Map(assignmentGroupDTO.GroupDetails),
                    LocalName = assignmentGroupDTO.LocalName
                };

                return assignmentGroupVM;
            }
            return new AssignmentGroupVM();
        }
    }
}