using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models;
using MCS.UI.Areas.Admin.Models.OrgUnit;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class OrgUnitMapper
    {
        public static OrgUnitVM Map(OrgUnitDTO organizationUnitDTO)
        {
            if (organizationUnitDTO != null)
            {
                OrgUnitVM organizationUnitVM = new OrgUnitVM()
                {
                    Id = organizationUnitDTO.Id,
                    IsSelected = organizationUnitDTO.IsSelected,
                    IsVirtualUnit = organizationUnitDTO.IsVirtualUnit,
                    Key = organizationUnitDTO.Key,
                    LinkUnitsKeys = organizationUnitDTO.LinkUnitsKeys,
                    Name = organizationUnitDTO.Name,
                    Number = organizationUnitDTO.Number,
                    ParentId = organizationUnitDTO.ParentId,
                    HasChilds = organizationUnitDTO.HasChilds,
                    Lineage = organizationUnitDTO.Lineage
                };

                return organizationUnitVM;
            }
            return new OrgUnitVM();
        }
        public static List<OrgUnitVM> Map(IList<OrgUnitDTO> organizationUnitDTOs)
        {
            if (organizationUnitDTOs == null || !organizationUnitDTOs.Any())
            { return null; }
            List<OrgUnitVM> organizationUnitVMs = organizationUnitDTOs
                .Select(b => new OrgUnitVM
                {
                    Id = b.Id,
                    IsSelected = b.IsSelected,
                    IsVirtualUnit = b.IsVirtualUnit,
                    Key = b.Key,
                    LinkUnitsKeys = b.LinkUnitsKeys,
                    Name = b.Name,
                    Number = b.Number,
                    ParentId = b.ParentId,
                    HasChilds = b.HasChilds,
                    Lineage = b.Lineage

                }).ToList();
            return organizationUnitVMs;
        }
        public static List<OrgUnitDTO> Map(IList<OrgUnitVM> organizationUnitVMs)
        {
            if (organizationUnitVMs == null || !organizationUnitVMs.Any())
            { return null; }
            List<OrgUnitDTO> organizationUnitDTOs = organizationUnitVMs
                .Select(b => new OrgUnitDTO
                {
                    Id = b.Id,
                    IsSelected = b.IsSelected,
                    IsVirtualUnit = b.IsVirtualUnit,
                    Key = b.Key,
                    LinkUnitsKeys = b.LinkUnitsKeys,
                    Name = b.Name,
                    Number = b.Number,
                    ParentId = b.ParentId,
                    HasChilds = b.HasChilds
                }).ToList();
            return organizationUnitDTOs;
        }
        public static List<OrgUnitUserDTO> Map(IList<OrgUnitUserVM> organizationUnitUserVMs)
        {
            if (organizationUnitUserVMs == null || !organizationUnitUserVMs.Any())
            { return null; }
            List<OrgUnitUserDTO> organizationUnitUserDTOs = organizationUnitUserVMs
                .Select(b => new OrgUnitUserDTO
                {
                    Id = b.Id,
                    UserName = b.UserName

                }).ToList();
            return organizationUnitUserDTOs;
        }
        public static List<OrgUnitUserVM> Map(IList<OrgUnitUserDTO> organizationUnitUserDTOs)
        {
            if (organizationUnitUserDTOs == null || !organizationUnitUserDTOs.Any())
            { return null; }
            List<OrgUnitUserVM> organizationUnitUserVMs = organizationUnitUserDTOs
                .Select(b => new OrgUnitUserVM
                {
                    Id = b.Id,
                    UserName = b.UserName,
                    IsActive = b.IsActive,
                    Email = b.Email,
                    PhoneNumber = b.PhoneNumber,
                    LocalName = b.LocalName,
                    RoleName = b.RoleName,
                    MainOrgUnitName = b.MainOrgUnitName,
                    ExternalId = b.ExternalId

                }).ToList();
            return organizationUnitUserVMs;
        }
        public static List<OrgUnitStructureDesignVM> Map(IList<OrgUnitStructureDesignDTO> organizationUnitStructureDesignDTOs, string cultureName)
        {
            if (organizationUnitStructureDesignDTOs == null || !organizationUnitStructureDesignDTOs.Any())
            { return null; }
            List<OrgUnitStructureDesignVM> organizationUnitStructureDesignVMs = organizationUnitStructureDesignDTOs
                .Select(b => new OrgUnitStructureDesignVM
                {
                    OrgUnits = OrgStructureInfoMapper.Map(b.OrgUnits, cultureName),
                    Settings = b.Settings

                }).ToList();
            return organizationUnitStructureDesignVMs;
        }
        public static List<OrgUnitStructureDesignDTO> Map(IList<OrgUnitStructureDesignVM> organizationUnitStructureDesignVMs)
        {
            if (organizationUnitStructureDesignVMs == null || !organizationUnitStructureDesignVMs.Any())
            { return null; }
            List<OrgUnitStructureDesignDTO> organizationUnitStructureDesignDTOs = organizationUnitStructureDesignVMs
                .Select(b => new OrgUnitStructureDesignDTO
                {
                    OrgUnits = OrgStructureInfoMapper.Map(b.OrgUnits),
                    Settings = b.Settings

                }).ToList();
            return organizationUnitStructureDesignDTOs;
        }
        public static List<OrgUnitLinkVM> Map(IList<OrgUnitLinkDTO> organizationUnitLinkDTOs)
        {
            if (organizationUnitLinkDTOs == null || !organizationUnitLinkDTOs.Any())
            { return null; }
            List<OrgUnitLinkVM> organizationUnitLinkVMs = organizationUnitLinkDTOs
                .Select(b => new OrgUnitLinkVM
                {
                    Key = b.Key,
                    OrgUnitName = b.OrgUnitName

                }).ToList();
            return organizationUnitLinkVMs;
        }
        public static List<OrgUnitLinkDTO> Map(IList<OrgUnitLinkVM> organizationUnitLinkVMs)
        {
            if (organizationUnitLinkVMs == null || !organizationUnitLinkVMs.Any())
            { return null; }
            List<OrgUnitLinkDTO> organizationUnitLinkDTOs = organizationUnitLinkVMs
                .Select(b => new OrgUnitLinkDTO
                {
                    Key = b.Key,
                    OrgUnitName = b.OrgUnitName

                }).ToList();
            return organizationUnitLinkDTOs;
        }
        public static OrgUnitStructureDesignVM Map(OrgUnitStructureDesignDTO organizationUnitStructureDesignDTOs, string cultureName)
        {
            if (organizationUnitStructureDesignDTOs != null)
            {
                return new OrgUnitStructureDesignVM
                {
                    OrgUnits = OrgStructureInfoMapper.Map(organizationUnitStructureDesignDTOs.OrgUnits, cultureName),
                    Settings = organizationUnitStructureDesignDTOs.Settings
                };
            }
            return null;
        }
        public static OrgUnitStructureDesignDTO Map(OrgUnitStructureDesignVM organizationUnitStructureDesignVMs)
        {
            if (organizationUnitStructureDesignVMs != null)
            {
                return new OrgUnitStructureDesignDTO
                {
                    OrgUnits = OrgStructureInfoMapper.Map(organizationUnitStructureDesignVMs.OrgUnits),
                    Settings = organizationUnitStructureDesignVMs.Settings

                };
            }
            return null;
        }
        public static List<OrgUnitUserVM> MapToOrgUnitUser(IList<UserProfileDTO> userProfileDTOs)
        {
            if (userProfileDTOs == null || !userProfileDTOs.Any())
            {
                return new List<OrgUnitUserVM>();
            }
            List<OrgUnitUserVM> organizationUnitUserVMs = userProfileDTOs
                .Select(b => new OrgUnitUserVM
                {
                    Id = b.Id,
                    UserName = b.UserName,
                    Email = b.Email,
                    PhoneNumber = b.PhoneNumber,
                    IsActive = b.IsActive,
                    LocalName = b.LocalName,
                    RoleName = b.RoleName,
                }).ToList();
            return organizationUnitUserVMs;
        }






        #region CustToDepartment

        public static List<OrgunitSapDto> Map(SectorSapVM sectorSapVM)
        {
            if (sectorSapVM?.d?.results == null || sectorSapVM.d.results.Count == 0)
            { return null; }
            List<OrgunitSapDto> sectorSapDtos = sectorSapVM.d?.results
                .Select(b => new OrgunitSapDto
                {
                    Code = b.externalCode,
                    NameAr = b.externalName_ar_SA,
                    NameEn = b.externalName_en_US,
                    SystemStatus = b.mdfSystemStatus,
                    ParentCode = null
                }).ToList();
            return sectorSapDtos;
        }
        #endregion
        #region Division 

        public static List<OrgunitSapDto> Map(DivisionSapVM divisionSapVM)
        {

            List<OrgunitSapDto> orgunitSapDtos = new List<OrgunitSapDto>();
            if (!(divisionSapVM?.d?.results != null && divisionSapVM?.d?.results.Count > 0))
            {
                return null;
            }
            orgunitSapDtos = divisionSapVM.d.results.Select(org => new OrgunitSapDto
            {
                Code = org.externalCode,
                NameAr = !string.IsNullOrWhiteSpace(org.name_ar_SA) ? org.name_ar_SA : !string.IsNullOrWhiteSpace(org.name_en_US) ? org.name_en_US : "",
                NameEn = !string.IsNullOrWhiteSpace(org.name_en_US) ? org.name_en_US : !string.IsNullOrWhiteSpace(org.name_ar_SA) ? org.name_ar_SA : "",
                SystemStatus = org.status,
                ParentCode = org?.cust_Sector?.results?.FirstOrDefault()?.externalCode ?? org?.cust_Sub_Sector?.results?.FirstOrDefault()?.externalCode ?? null

            }).ToList();


            return orgunitSapDtos;
        }

        public static List<OrgunitSapDto> Map(SubSectorVM subSectorVM)
        {

            List<OrgunitSapDto> orgunitSapDtos = new List<OrgunitSapDto>();
            if (!(subSectorVM?.d?.results != null && subSectorVM?.d?.results.Count > 0))
            {
                return null;
            }
            orgunitSapDtos = subSectorVM.d.results.Select(org => new OrgunitSapDto
            {
                Code = org.externalCode,
                NameAr = !string.IsNullOrWhiteSpace(org.cust_Name_ar_SA) ? org.cust_Name_ar_SA : !string.IsNullOrWhiteSpace(org.cust_Name_en_US) ? org.cust_Name_en_US : "",
                NameEn = !string.IsNullOrWhiteSpace(org.cust_Name_en_US) ? org.cust_Name_en_US : !string.IsNullOrWhiteSpace(org.cust_Name_ar_SA) ? org.cust_Name_ar_SA : "",
                SystemStatus = org.mdfSystemStatus,
                ParentCode = org?.cust_Sector?.externalCode ?? null

            }).ToList();


            return orgunitSapDtos;
        }

        #endregion

        #region Department
        public static List<OrgunitSapDto> Map(DepartmentSapVM divisionSapVM)
        {

            List<OrgunitSapDto> orgunitSapDtos = new List<OrgunitSapDto>();
            if (!(divisionSapVM?.d?.results != null && divisionSapVM?.d?.results.Count > 0))
            {
                return null;
            }
            orgunitSapDtos = divisionSapVM.d.results.Select(org => new OrgunitSapDto
            {
                Code = org.externalCode,
                NameAr = !string.IsNullOrWhiteSpace(org.name_ar_SA) ? org.name_ar_SA : !string.IsNullOrWhiteSpace(org.name_en_US) ? org.name_en_US : "",
                NameEn = !string.IsNullOrWhiteSpace(org.name_en_US) ? org.name_en_US : !string.IsNullOrWhiteSpace(org.name_ar_SA) ? org.name_ar_SA : "",
                SystemStatus = org.status,
                ParentCode = org?.cust_toDivision?.results?.FirstOrDefault()?.externalCode ?? null

            }).ToList();


            return orgunitSapDtos;
        }

        #endregion

        #region Section
        public static List<OrgunitSapDto> Map(SectionSapVM serviceOrgUnitHierarchyVM)
        {

            List<OrgunitSapDto> orgunitSapDtos = new List<OrgunitSapDto>();
            if (!(serviceOrgUnitHierarchyVM?.d?.results != null && serviceOrgUnitHierarchyVM?.d?.results.Count > 0))
            {
                return null;
            }
            orgunitSapDtos = serviceOrgUnitHierarchyVM.d.results.Select(org => new OrgunitSapDto
            {
                Code = org.externalCode,
                NameAr = !string.IsNullOrWhiteSpace(org.externalName_ar_SA) ? org.externalName_ar_SA : !string.IsNullOrWhiteSpace(org.externalName_en_US) ? org.externalName_en_US : "",
                NameEn = !string.IsNullOrWhiteSpace(org.externalName_en_US) ? org.externalName_en_US : "",
                ParentCode = org?.cust_toDepartment?.results?.FirstOrDefault()?.externalCode ?? null,
                SystemStatus = org.mdfSystemStatus


            }).ToList();


            return orgunitSapDtos;
        }
        #endregion

    }
}