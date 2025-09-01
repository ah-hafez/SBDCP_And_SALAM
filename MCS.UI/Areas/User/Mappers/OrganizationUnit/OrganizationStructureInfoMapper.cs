using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.BarcodeDesigner;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Models.OrgUnit;

namespace MCS.UI.Areas.User.Mappers.OrgUnit
{
    public static class OrgStructureInfoMapper
    {
        public static List<OrgStructureInfoVM> Map(IList<OrgStructureInfoDTO> organizationStructureInfoDTOs)
        {
            if (organizationStructureInfoDTOs == null || !organizationStructureInfoDTOs.Any())
            {
                return new List<OrgStructureInfoVM>();
            }
            List<OrgStructureInfoVM> organizationStructureInfoVMs = organizationStructureInfoDTOs
                .Select(organizationStructureInfoDTO => new OrgStructureInfoVM()
                {
                    AssignmentPaper = AssignmentPaperMapper.Map(organizationStructureInfoDTO.AssignmentPaper),
                    BarCode = organizationStructureInfoDTO.BarCode,
                    BarcodeDesigners = BarcodeDesignerMapper.Map(organizationStructureInfoDTO.BarcodeDesigners),
                    Counter = CounterMapper.Map(organizationStructureInfoDTO.Counter),
                    IdentifierId = organizationStructureInfoDTO.IdentifierId,
                    IsActive = organizationStructureInfoDTO.IsActive,
                    IsDeleted = organizationStructureInfoDTO.IsDeleted,
                    IsExternal = organizationStructureInfoDTO.IsExternal,
                    IsNew = organizationStructureInfoDTO.IsNew,
                    IsVirtualUnit = organizationStructureInfoDTO.IsVirtualUnit,
                    Key = organizationStructureInfoDTO.Key,
                    LinkUnitsKeys = organizationStructureInfoDTO.LinkUnitsKeys,
                    ManagerId = organizationStructureInfoDTO.ManagerId,
                    Name = organizationStructureInfoDTO.Name,
                    Names = LocalizationMapper.Map(organizationStructureInfoDTO.Names),
                    Number = organizationStructureInfoDTO.Number,
                    ParentId = organizationStructureInfoDTO.ParentId,
                    StructureAsJson = organizationStructureInfoDTO.StructureAsJson,
                    TransactionsProcessingPeriod = organizationStructureInfoDTO.TransactionsProcessingPeriod,
                    Users = OrgUnitUserMapper.Map(organizationStructureInfoDTO.Users),
                }).ToList();
            return organizationStructureInfoVMs;
        }
        public static List<OrgStructureInfoDTO> Map(IList<OrgStructureInfoVM> organizationStructureInfoVMs)
        {
            if (organizationStructureInfoVMs == null || !organizationStructureInfoVMs.Any())
            {
                return new List<OrgStructureInfoDTO>();
            }
            List<OrgStructureInfoDTO> organizationStructureInfoDTOs = organizationStructureInfoVMs
                .Select(organizationStructureInfoVM => new OrgStructureInfoDTO()
                {
                    AssignmentPaper = AssignmentPaperMapper.Map(organizationStructureInfoVM.AssignmentPaper),
                    BarCode = organizationStructureInfoVM.BarCode,
                    BarcodeDesigners = BarcodeDesignerMapper.Map(organizationStructureInfoVM.BarcodeDesigners),
                    Counter = CounterMapper.Map(organizationStructureInfoVM.Counter),
                    IdentifierId = organizationStructureInfoVM.IdentifierId,
                    IsActive = organizationStructureInfoVM.IsActive,
                    IsDeleted = organizationStructureInfoVM.IsDeleted,
                    IsExternal = organizationStructureInfoVM.IsExternal,
                    IsNew = organizationStructureInfoVM.IsNew,
                    IsVirtualUnit = organizationStructureInfoVM.IsVirtualUnit,
                    Key = organizationStructureInfoVM.Key,
                    LinkUnitsKeys = organizationStructureInfoVM.LinkUnitsKeys,
                    ManagerId = organizationStructureInfoVM.ManagerId,
                    Name = organizationStructureInfoVM.Name,
                    Names = LocalizationMapper.Map(organizationStructureInfoVM.Names),
                    Number = organizationStructureInfoVM.Number,
                    ParentId = organizationStructureInfoVM.ParentId,
                    StructureAsJson = organizationStructureInfoVM.StructureAsJson,
                    TransactionsProcessingPeriod = organizationStructureInfoVM.TransactionsProcessingPeriod,
                    Users = OrgUnitUserMapper.Map(organizationStructureInfoVM.Users),
                }).ToList();
            return organizationStructureInfoDTOs;
        }
    }
}