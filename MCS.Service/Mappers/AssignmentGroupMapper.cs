using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class AssignmentGroupMapper
    {
        public static AssignmentGroup Map(AssignmentGroupDTO assignmentGroupDTO)
        {
            if (assignmentGroupDTO != null)
            {
                AssignmentGroup assignmentGroup = new AssignmentGroup()
                {
                    LocalizationIdentifier = LocalizationIdentifierMapper.Map(assignmentGroupDTO.GroupName),
                    AssignmentGroupDetails = AssignmentGroupDetailMapper.Map(assignmentGroupDTO.GroupDetails)
                };

                return assignmentGroup;
            }
            return null;
        }

        public static AssignmentGroupDTO Map(AssignmentGroup assignmentGroup)
        {
            if (assignmentGroup != null)
            {
                AssignmentGroupDTO assignmentGroupDTO = new AssignmentGroupDTO()
                {
                    Id = assignmentGroup.Id,
                    LocalName = assignmentGroup.LocalName,
                    GroupDetails = AssignmentGroupDetailMapper.Map(assignmentGroup.AssignmentGroupDetails),
                    
                };

                return assignmentGroupDTO;
            }
            return null;
        }

        public static List<AssignmentGroupDTO> Map(IList<AssignmentGroup> assignmentGroups)
        {
            if (assignmentGroups == null || !assignmentGroups.Any())
            {
                return null;
            }

            List<AssignmentGroupDTO> assignmentGroupDTOs = assignmentGroups.Select(assignmentGroupDTO => new AssignmentGroupDTO()
            {
                Id = assignmentGroupDTO.Id,
                LocalName = assignmentGroupDTO.LocalName,
                GroupDetails = AssignmentGroupDetailMapper.Map(assignmentGroupDTO.AssignmentGroupDetails)
            }).ToList();


            return assignmentGroupDTOs;
        }

        public static List<AssignmentGroup> Map(IList<AssignmentGroupDTO> assignmentGroupDTOs)
        {
            if (assignmentGroupDTOs == null || !assignmentGroupDTOs.Any())
            {
                return null;
            }
            List<AssignmentGroup> assignmentGroups = assignmentGroupDTOs.Select(assignmentGroup => new AssignmentGroup()
            {
                LocalizationIdentifier = LocalizationIdentifierMapper.Map(assignmentGroup.GroupName),
                AssignmentGroupDetails = AssignmentGroupDetailMapper.Map(assignmentGroup.GroupDetails)
            }).ToList();

            return assignmentGroups;
        }
    }
}