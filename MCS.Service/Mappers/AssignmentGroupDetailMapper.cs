using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Business;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class AssignmentGroupDetailMapper
    {


        public static List<AssignmentGroupDetail> Map(List<AssignmentGroupDetailDTO> assignmentGroupDetailDTOs)
        {
            if (assignmentGroupDetailDTOs == null || !assignmentGroupDetailDTOs.Any())
            {
                return null;
            }
            IOrgUnitBL organizationUnitBL = IoC.Resolve<IOrgUnitBL>();
            IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
            List<AssignmentGroupDetail> assignmentGroupDetails = assignmentGroupDetailDTOs
                .Select(assignmentGroupDetailDTO => new AssignmentGroupDetail()
                {
                    OrgUnit = organizationUnitBL.GetOrgUnitById(assignmentGroupDetailDTO.OrgUnitId),
                    UserProfile = assignmentGroupDetailDTO.UserProfileId.HasValue
                    ? userManagementBL.GetUserById(assignmentGroupDetailDTO.UserProfileId.Value)
                    : null
                }).ToList();

            return assignmentGroupDetails;
        }

        

        public static List<AssignmentGroupDetailDTO> Map(IList<AssignmentGroupDetail> assignmentGroupDetails)
        {
            if (assignmentGroupDetails == null || !assignmentGroupDetails.Any())
            {
                return null;
            }

            List<AssignmentGroupDetailDTO> assignmentGroupDetailDTOs = assignmentGroupDetails
                .Select(assignmentGroupDetail => new AssignmentGroupDetailDTO
                {
                    Id = assignmentGroupDetail.Id,
                    OrgUnitId = assignmentGroupDetail.OrgUnit.Id,
                    OrgUnitName = assignmentGroupDetail?.OrgUnit.LocalName,
                    UserProfileId = assignmentGroupDetail?.UserProfile.Id,
                    UserProfileName = assignmentGroupDetail?.UserProfile.LocalName,


        }).ToList();


            return assignmentGroupDetailDTOs;
        }
}
}