using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.OrgUnit;

namespace MCS.UI.Areas.User.Mappers.OrgUnit
{
    public static class OrgUnitUserMapper
    {
        public static List<OrgUnitUserVM> Map(IList<OrgUnitUserDTO> organizationUnitUserDTOs)
        {
            if (organizationUnitUserDTOs == null || !organizationUnitUserDTOs.Any())
            {
                return new List<OrgUnitUserVM>();
            }
            List<OrgUnitUserVM> organizationUnitUserVMs = organizationUnitUserDTOs
                .Select(organizationUnitUserDTO => new OrgUnitUserVM()
                { 
                    Id = organizationUnitUserDTO.Id,
                    UserName = organizationUnitUserDTO.UserName
                }).ToList();

            return organizationUnitUserVMs;
        }
        public static List<OrgUnitUserDTO> Map(IList<OrgUnitUserVM> organizationUnitUserVMs)
        {
            if (organizationUnitUserVMs == null || !organizationUnitUserVMs.Any())
            {
                return new List<OrgUnitUserDTO>();
            }
            List<OrgUnitUserDTO> organizationUnitUserDTOs = organizationUnitUserVMs
                .Select(organizationUnitUserVM => new OrgUnitUserDTO()
                {  
                    Id = organizationUnitUserVM.Id,
                    UserName = organizationUnitUserVM.UserName
                }).ToList();

            return organizationUnitUserDTOs;
        }


    }
}