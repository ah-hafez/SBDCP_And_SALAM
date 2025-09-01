using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.OrgUnit;

namespace MCS.UI.Areas.User.Mappers.OrgUnit
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
            {
                return new List<OrgUnitVM>();
            }
            List<OrgUnitVM> organizationUnitVMs = organizationUnitDTOs
                .Select(organizationUnitDTO => new OrgUnitVM()
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
                    Lineage = organizationUnitDTO.Lineage,
                    IsCurrentTreeRoot = organizationUnitDTO.IsCurrentTreeRoot,
                }).ToList();
            return organizationUnitVMs;
        }
        public static OrgUnitDTO Map(OrgUnitVM organizationUnitVM)
        {
            if (organizationUnitVM != null)
            {
                OrgUnitDTO organizationUnitDTO = new OrgUnitDTO()
                {
                    Id = organizationUnitVM.Id,
                    IsSelected = organizationUnitVM.IsSelected,
                    IsVirtualUnit = organizationUnitVM.IsVirtualUnit,
                    Key = organizationUnitVM.Key,
                    LinkUnitsKeys = organizationUnitVM.LinkUnitsKeys,
                    Name = organizationUnitVM.Name,
                    Number = organizationUnitVM.Number,
                    ParentId = organizationUnitVM.ParentId,
                    Lineage = organizationUnitVM.Lineage
                };

                return organizationUnitDTO;
            }
            return new OrgUnitDTO();
        }
        public static List<OrgUnitDTO> Map(IList<OrgUnitVM> organizationUnitVMs)
        {
            if (organizationUnitVMs == null || !organizationUnitVMs.Any())
            {
                return new List<OrgUnitDTO>();
            }
            List<OrgUnitDTO> organizationUnitDTOs = organizationUnitVMs.Select(organizationUnitDTO => new OrgUnitDTO()
            {
                Id = organizationUnitDTO.Id,
                IsSelected = organizationUnitDTO.IsSelected,
                IsVirtualUnit = organizationUnitDTO.IsVirtualUnit,
                Key = organizationUnitDTO.Key,
                LinkUnitsKeys = organizationUnitDTO.LinkUnitsKeys,
                Name = organizationUnitDTO.Name,
                Number = organizationUnitDTO.Number,
                ParentId = organizationUnitDTO.ParentId,
                Lineage = organizationUnitDTO.Lineage
            }).ToList();

            return organizationUnitDTOs;
        }
    }
}