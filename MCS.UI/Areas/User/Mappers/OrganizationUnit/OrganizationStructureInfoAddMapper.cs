using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Models.OrgUnit;

namespace MCS.UI.Areas.User.Mappers.OrgUnit
{
    public static class OrgStructureInfoAddMapper
    {
        public static List<OrgStructureInfoAddVM> Map(IList<OrgStructureInfoAddDTO> organizationStructureInfoAddDTOs)
        {
            if (organizationStructureInfoAddDTOs == null || !organizationStructureInfoAddDTOs.Any())
            {
                return new List<OrgStructureInfoAddVM>();
            }
            List<OrgStructureInfoAddVM> organizationStructureInfoAddVMs = organizationStructureInfoAddDTOs
                .Select(organizationStructureInfoAddDTO => new OrgStructureInfoAddVM()
                {
                    BarCode = organizationStructureInfoAddDTO.BarCode,
                    Counter = CounterMapper.Map(organizationStructureInfoAddDTO.Counter),
                    IsActive = organizationStructureInfoAddDTO.IsActive,
                    IsDeleted = organizationStructureInfoAddDTO.IsDeleted,
                    IsExternal = organizationStructureInfoAddDTO.IsExternal,
                    IsNew = organizationStructureInfoAddDTO.IsNew,
                    IsRoot = organizationStructureInfoAddDTO.IsRoot,
                    IsVirtualUnit = organizationStructureInfoAddDTO.IsVirtualUnit,
                    Key = organizationStructureInfoAddDTO.Key,
                    ManagerId = organizationStructureInfoAddDTO.ManagerId,
                    Names = LocalizationMapper.Map(organizationStructureInfoAddDTO.Names),
                    Number = organizationStructureInfoAddDTO.Number,
                    ParentId = organizationStructureInfoAddDTO.ParentId,
                    TransactionsProcessingPeriod = organizationStructureInfoAddDTO.TransactionsProcessingPeriod,
                    Users = OrgUnitUserMapper.Map(organizationStructureInfoAddDTO.Users)
                }).ToList();

            return organizationStructureInfoAddVMs;
        }
        public static List<OrgStructureInfoAddDTO> Map(IList<OrgStructureInfoAddVM> organizationStructureInfoAddVMs)
        {
            if (organizationStructureInfoAddVMs == null || !organizationStructureInfoAddVMs.Any())
            {
                return new List<OrgStructureInfoAddDTO>();
            }
            List<OrgStructureInfoAddDTO> organizationStructureInfoAddDTOs = organizationStructureInfoAddVMs
                .Select(organizationStructureInfoAddVM => new OrgStructureInfoAddDTO()
                {
                    BarCode = organizationStructureInfoAddVM.BarCode,
                    Counter = CounterMapper.Map(organizationStructureInfoAddVM.Counter),
                    IsActive = organizationStructureInfoAddVM.IsActive,
                    IsDeleted = organizationStructureInfoAddVM.IsDeleted,
                    IsExternal = organizationStructureInfoAddVM.IsExternal,
                    IsNew = organizationStructureInfoAddVM.IsNew,
                    IsRoot = organizationStructureInfoAddVM.IsRoot,
                    IsVirtualUnit = organizationStructureInfoAddVM.IsVirtualUnit,
                    Key = organizationStructureInfoAddVM.Key,
                    ManagerId = organizationStructureInfoAddVM.ManagerId,
                    Names = LocalizationMapper.Map(organizationStructureInfoAddVM.Names),
                    Number = organizationStructureInfoAddVM.Number,
                    ParentId = organizationStructureInfoAddVM.ParentId,
                    TransactionsProcessingPeriod = organizationStructureInfoAddVM.TransactionsProcessingPeriod,
                    Users = OrgUnitUserMapper.Map(organizationStructureInfoAddVM.Users)
                }).ToList();

            return organizationStructureInfoAddDTOs;
        }
    }
}