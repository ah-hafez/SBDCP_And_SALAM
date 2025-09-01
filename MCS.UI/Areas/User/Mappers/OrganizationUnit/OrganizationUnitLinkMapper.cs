using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.OrgUnit;

namespace MCS.UI.Areas.User.Mappers.OrgUnit
{
    public static class OrgUnitLinkMapper
    {
        public static List<OrgUnitLinkVM> Map(IList<OrgUnitLinkDTO> organizationUnitLinkDTOs)
        {
            if (organizationUnitLinkDTOs == null || !organizationUnitLinkDTOs.Any())
            {
                return new List<OrgUnitLinkVM>();
            }
            List<OrgUnitLinkVM> organizationUnitLinkVMs = organizationUnitLinkDTOs
                .Select(organizationUnitLinkDTO => new OrgUnitLinkVM()
                {
                    Key = organizationUnitLinkDTO.Key,
                    OrgUnitName = organizationUnitLinkDTO.OrgUnitName
                }).ToList();

            return organizationUnitLinkVMs;
        }
        public static List<OrgUnitLinkDTO> Map(IList<OrgUnitLinkVM> organizationUnitLinkVMs)
        {
            if (organizationUnitLinkVMs == null || !organizationUnitLinkVMs.Any())
            {
                return new List<OrgUnitLinkDTO>();
            }
            List<OrgUnitLinkDTO> organizationUnitLinkDTOs = organizationUnitLinkVMs
                .Select(organizationUnitLinkVM => new OrgUnitLinkDTO()
                {
                    Key = organizationUnitLinkVM.Key,
                    OrgUnitName = organizationUnitLinkVM.OrgUnitName
                }).ToList();

            return organizationUnitLinkDTOs;
        }
    }
}