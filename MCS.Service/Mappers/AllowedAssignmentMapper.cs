using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class AllowedAssignmentMapper
    {
        public static AllowedAssignment Map(AllowedAssignmentDTO allowedAssignmentDTO)
        {
            if (allowedAssignmentDTO != null)
            {
                AllowedAssignment allowedAssignment = new AllowedAssignment()
                {
                    UserId = allowedAssignmentDTO.UserId,
                    ToUserId = allowedAssignmentDTO.ToUserId,
                    EntityId = allowedAssignmentDTO.EntityId,
                };

                return allowedAssignment; 
            }
            return null;
        }

        public static AllowedAssignmentDTO  Map(AllowedAssignment allowedAssignments)
        {
            if (allowedAssignments != null)
            {
                AllowedAssignmentDTO allowedAssignmentDTO = new AllowedAssignmentDTO()
                {
                    UserId = allowedAssignments.UserId,
                    ToUserId = allowedAssignments.ToUserId,
                    EntityId = allowedAssignments.EntityId,
                };

                return allowedAssignmentDTO;
            }
            return null;
        }




        public static List<AllowedAssignmentDTO> Map(IList<AllowedAssignment> allowedAssignment , string Culture)
        {
            if (allowedAssignment == null || !allowedAssignment.Any())
            {
                return null;
            }

            List<AllowedAssignmentDTO> AllowedAssignmentDTOs = allowedAssignment.Select(assignmentGroup => new AllowedAssignmentDTO()
            {
                Id = assignmentGroup.Id,
                UserId = assignmentGroup.UserId,
                ToUserId = assignmentGroup.ToUserId,
                User = MapUserProfile(assignmentGroup.User),
                ToUser = MapUserProfile(assignmentGroup.ToUser),
                Entity = MapOrgUnitMapper(assignmentGroup.Entity)

            }).ToList();


            return AllowedAssignmentDTOs;
        }



        public static UserProfileDTO MapUserProfile(UserProfile userProfile)
        {
            if (userProfile == null)
            {
                return null;
            }

            UserProfileDTO userProfileDTO = new UserProfileDTO()
            {
                IsActive = true,
                Id = userProfile.Id,
                LocalName = userProfile.LocalName,
                Email = userProfile.Email,
    
            };

            return userProfileDTO;
        }


        public static OrgUnitDTO MapOrgUnitMapper(OrgUnit organizationUnit)
        {
            if (organizationUnit != null)
            {
                OrgUnitDTO organizationUnitVM = new OrgUnitDTO()
                {
                    Id = organizationUnit.Id,

                  
                    Name = organizationUnit.LocalName,
                
                };

                return organizationUnitVM;
            }
            return new OrgUnitDTO();
        }
    }
}