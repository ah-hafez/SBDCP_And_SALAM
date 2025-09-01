using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Models.OrgUnit;

namespace MCS.UI.Areas.User.Mappers.OrgUnit
{
    public static class OrgStructureInfoEditMapper
    {
        public static List<OrgStructureInfoEditVM> Map(IList<OrgStructureInfoEditDTO> organizationStructureInfoEditDTOs)
        {
            if (organizationStructureInfoEditDTOs == null || !organizationStructureInfoEditDTOs.Any())
            {
                return new List<OrgStructureInfoEditVM>();
            }
            List<OrgStructureInfoEditVM> organizationStructureInfoEditVMs = organizationStructureInfoEditDTOs
                .Select(organizationStructureInfoEditDTO => new OrgStructureInfoEditVM()
                {
                    BarCode = organizationStructureInfoEditDTO.BarCode,
                    Counter = CounterMapper.Map(organizationStructureInfoEditDTO.Counter),
                    IsActive = organizationStructureInfoEditDTO.IsActive,
                    IsDeleted = organizationStructureInfoEditDTO.IsDeleted,
                    IsExternal = organizationStructureInfoEditDTO.IsExternal,
                    IsNew = organizationStructureInfoEditDTO.IsNew,
                    IsRoot = organizationStructureInfoEditDTO.IsRoot,
                    IsVirtualUnit = organizationStructureInfoEditDTO.IsVirtualUnit,
                    Key = organizationStructureInfoEditDTO.Key,
                    ManagerId = organizationStructureInfoEditDTO.ManagerId,
                    Names = LocalizationMapper.Map(organizationStructureInfoEditDTO.Names),
                    Number = organizationStructureInfoEditDTO.Number,
                    ParentId = organizationStructureInfoEditDTO.ParentId,
                    TransactionsProcessingPeriod = organizationStructureInfoEditDTO.TransactionsProcessingPeriod,
                    Users = OrgUnitUserMapper.Map(organizationStructureInfoEditDTO.Users)
                }).ToList();

            return organizationStructureInfoEditVMs;
        }
        public static List<OrgStructureInfoEditDTO> Map(IList<OrgStructureInfoEditVM> organizationStructureInfoEditVMs)
        {
            if (organizationStructureInfoEditVMs == null || !organizationStructureInfoEditVMs.Any())
            {
                return new List<OrgStructureInfoEditDTO>();
            }
            List<OrgStructureInfoEditDTO> organizationStructureInfoEditDTOs = organizationStructureInfoEditVMs
                .Select(organizationStructureInfoEditVM => new OrgStructureInfoEditDTO()
                {
                    BarCode = organizationStructureInfoEditVM.BarCode,
                    Counter = CounterMapper.Map(organizationStructureInfoEditVM.Counter),
                    IsActive = organizationStructureInfoEditVM.IsActive,
                    IsDeleted = organizationStructureInfoEditVM.IsDeleted,
                    IsExternal = organizationStructureInfoEditVM.IsExternal,
                    IsNew = organizationStructureInfoEditVM.IsNew,
                    IsRoot = organizationStructureInfoEditVM.IsRoot,
                    IsVirtualUnit = organizationStructureInfoEditVM.IsVirtualUnit,
                    Key = organizationStructureInfoEditVM.Key,
                    ManagerId = organizationStructureInfoEditVM.ManagerId,
                    Names = LocalizationMapper.Map(organizationStructureInfoEditVM.Names),
                    Number = organizationStructureInfoEditVM.Number,
                    ParentId = organizationStructureInfoEditVM.ParentId,
                    TransactionsProcessingPeriod = organizationStructureInfoEditVM.TransactionsProcessingPeriod,
                    Users = OrgUnitUserMapper.Map(organizationStructureInfoEditVM.Users)
                }).ToList();

            return organizationStructureInfoEditDTOs;
        }
    }
}