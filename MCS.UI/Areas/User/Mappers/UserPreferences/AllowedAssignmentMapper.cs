using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Mappers;
using MCS.UI.Areas.User.Mappers.UserPreferences.UserDelegation;
using MCS.UI.Areas.User.Models.OrgUnit;
using MCS.UI.Areas.User.Models.UserManagement;
using MCS.UI.Areas.User.Models.UserPreferences;

namespace MCS.UI.Areas.User.Mappers.UserPreferences
{
    public static class AllowedAssignmentMapper
    {

        public static List<AllowedAssignmentVM> Map(IList<AllowedAssignmentDTO> allowedAssignmentDTOs)
        {
            if (allowedAssignmentDTOs == null || !allowedAssignmentDTOs.Any())
            {
                return new List<AllowedAssignmentVM>();
            }

            List<AllowedAssignmentVM> allowedAssignmentVM = allowedAssignmentDTOs.Select(assignmentGroup => new AllowedAssignmentVM()
            {
                Id = assignmentGroup.Id,
                UserId = assignmentGroup.UserId,
                ToUserId = assignmentGroup.ToUserId,
                User = MapUserProfile(assignmentGroup.User),
                ToUser = MapUserProfile(assignmentGroup.ToUser),
                Entity = MapOrgUnitMapper(assignmentGroup.Entity)
            }).ToList();
            return allowedAssignmentVM;
        }

        public static OrgUnitVM MapOrgUnitMapper(OrgUnitDTO organizationUnitDTO)
        {
            if (organizationUnitDTO != null)
            {
                OrgUnitVM organizationUnitVM = new OrgUnitVM()
                {
                    Id = organizationUnitDTO.Id,
                    Name = organizationUnitDTO.Name,

                };

                return organizationUnitVM;
            }
            return new OrgUnitVM();
        }

        public static UserProfileVM MapUserProfile(UserProfileDTO userProfileDTOs)
        {
            if (userProfileDTOs == null)
            {
                return null;
            }
            UserProfileVM userProfileVM = new UserProfileVM()
            {
                IsActive = true,
                Id = userProfileDTOs.Id,
                LocalName = userProfileDTOs.LocalName,
                Email = userProfileDTOs.Email,
            };
            return userProfileVM;
        }

    }
}