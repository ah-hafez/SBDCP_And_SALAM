using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.OrgUnit;

namespace MCS.UI.Areas.User.Mappers.OrgUnit
{
    public static class OrgUnitStructureDesignMapper
    {
        public static List<OrgUnitStructureDesignVM> Map(IList<OrgUnitStructureDesignDTO> organizationUnitStructureDesignDTOs)
        {
            if (organizationUnitStructureDesignDTOs == null || !organizationUnitStructureDesignDTOs.Any())
            {
                return new List<OrgUnitStructureDesignVM>();
            }
            List<OrgUnitStructureDesignVM> organizationUnitStructureDesignVMs = organizationUnitStructureDesignDTOs
                .Select(organizationUnitStructureDesignDTO => new OrgUnitStructureDesignVM()
                {
                    OrgUnits = OrgStructureInfoMapper.Map(organizationUnitStructureDesignDTO.OrgUnits),
                    Settings = organizationUnitStructureDesignDTO.Settings
                }).ToList();

            return organizationUnitStructureDesignVMs;
        }
        public static List<OrgUnitStructureDesignDTO> Map(IList<OrgUnitStructureDesignVM> organizationUnitStructureDesignVMs)
        {
            if (organizationUnitStructureDesignVMs == null || !organizationUnitStructureDesignVMs.Any())
            {
                return new List<OrgUnitStructureDesignDTO>();
            }
            List<OrgUnitStructureDesignDTO> organizationUnitStructureDesignDTOs = organizationUnitStructureDesignVMs
                .Select(organizationUnitStructureDesignVM => new OrgUnitStructureDesignDTO()
                {
                    OrgUnits = OrgStructureInfoMapper.Map(organizationUnitStructureDesignVM.OrgUnits),
                    Settings = organizationUnitStructureDesignVM.Settings
                }).ToList();

            return organizationUnitStructureDesignDTOs;
        }
        public static OrgUnitStructureDesignDTO Map(OrgUnitStructureDesignVM organizationUnitStructureDesignVM)
        {
            if (organizationUnitStructureDesignVM != null)
            {
                OrgUnitStructureDesignDTO organizationUnitStructureDesignDTO = new OrgUnitStructureDesignDTO()
                {
                    OrgUnits = OrgStructureInfoMapper.Map(organizationUnitStructureDesignVM.OrgUnits),
                    Settings = organizationUnitStructureDesignVM.Settings
                };

                return organizationUnitStructureDesignDTO;
            }
            return new OrgUnitStructureDesignDTO();
        }
        public static OrgUnitStructureDesignVM Map(OrgUnitStructureDesignDTO organizationUnitStructureDesignDTO)
        {
            if (organizationUnitStructureDesignDTO != null)
            {
                OrgUnitStructureDesignVM organizationUnitStructureDesignVM = new OrgUnitStructureDesignVM()
                {
                    OrgUnits = OrgStructureInfoMapper.Map(organizationUnitStructureDesignDTO.OrgUnits),
                    Settings = organizationUnitStructureDesignDTO.Settings
                };

                return organizationUnitStructureDesignVM;
            }
            return new OrgUnitStructureDesignVM();
        }
    }
}