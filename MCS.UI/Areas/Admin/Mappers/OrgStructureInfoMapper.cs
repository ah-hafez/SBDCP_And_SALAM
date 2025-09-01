using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.OrgUnit;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class OrgStructureInfoMapper
    {
        public static List<OrgStructureInfoVM> Map(IList<OrgStructureInfoDTO> organizationStructureInfoDTOs, string cultureName)
        {
            if (organizationStructureInfoDTOs == null || !organizationStructureInfoDTOs.Any())
            {
                return new List<OrgStructureInfoVM>();
            }
            List<OrgStructureInfoVM> organizationStructureInfoVMs = organizationStructureInfoDTOs
                .Select(b => new OrgStructureInfoVM
                {
                    AssignmentPaper = AssignmentPaperMapper.Map(b.AssignmentPaper),
                    BarCode = b.BarCode,
                    BarcodeDesigners = BarcodeDesignerMapper.Map(b.BarcodeDesigners),
                    Counter = CounterMapper.Map(b.Counter, cultureName),
                    IdentifierId = b.IdentifierId,
                    IsActive = b.IsActive,
                    IsDeleted = b.IsDeleted,
                    IsExternal = b.IsExternal,
                    LinkUnitsKeys = b.LinkUnitsKeys,
                    IsNew = b.IsNew,
                    ManagerId = b.ManagerId,
                    Names = LocalizationMapper.Map(b.Names),
                    StructureAsJson = b.StructureAsJson,
                    TransactionsProcessingPeriod = b.TransactionsProcessingPeriod,
                    Users = OrgUnitMapper.Map(b.Users),
                    IsVirtualUnit = b.IsVirtualUnit,
                    Key = b.Key,
                    Name = b.Name,
                    Number = b.Number,
                    ParentId = b.ParentId,
                    HasChilds = b.HasChilds,
                    Lineage = b.Lineage,
                    ExternalId = b.ExternalId,
                    IoDepartment = b.IoDepartment,
                    FollowUpDepartment = b.FollowUpDepartment,
                    IsExecutive = b.IsExecutive,
                    ReceiveElcOutBoundWithAcknowled = b.ReceiveElcOutBoundWithAcknowled,
                    SendSpecialCopy = b.SendSpecialCopy,
                    IsGeneralIoDepartment = b.IsGeneralIoDepartment,
                }).ToList();
            return organizationStructureInfoVMs;
        }
        public static List<OrgStructureInfoDTO> Map(IList<OrgStructureInfoVM> organizationStructureInfoVMs)
        {
            if (organizationStructureInfoVMs == null || !organizationStructureInfoVMs.Any())
            { return null; }
            List<OrgStructureInfoDTO> organizationStructureInfoDTOs = organizationStructureInfoVMs
                .Select(b => new OrgStructureInfoDTO
                {
                    AssignmentPaper = AssignmentPaperMapper.Map(b.AssignmentPaper),
                    BarCode = b.BarCode,
                    BarcodeDesigners = BarcodeDesignerMapper.Map(b.BarcodeDesigners),
                    Counter = CounterMapper.Map(b.Counter),
                    IdentifierId = b.IdentifierId,
                    IsActive = b.IsActive,
                    IsDeleted = b.IsDeleted,
                    IsExternal = b.IsExternal,
                    IsNew = b.IsNew,
                    ManagerId = b.ManagerId,
                    Names = LocalizationMapper.Map(b.Names),
                    StructureAsJson = b.StructureAsJson,
                    TransactionsProcessingPeriod = b.TransactionsProcessingPeriod,
                    Users = OrgUnitMapper.Map(b.Users),
                    IsVirtualUnit = b.IsVirtualUnit,
                    Key = b.Key,
                    LinkUnitsKeys = b.LinkUnitsKeys,
                    Name = b.Name,
                    Number = b.Number,
                    ParentId = b.ParentId

                }).ToList();
            return organizationStructureInfoDTOs;
        }
        public static List<OrgStructureInfoAddDTO> Map(IList<OrgStructureInfoAddVM> organizationStructureInfoAddVMs)
        {
            if (organizationStructureInfoAddVMs == null || !organizationStructureInfoAddVMs.Any())
            { return null; }
            List<OrgStructureInfoAddDTO> organizationStructureInfoAddDTOs = organizationStructureInfoAddVMs
                .Select(b => new OrgStructureInfoAddDTO
                {

                    BarCode = b.BarCode,
                    Counter = CounterMapper.Map(b.Counter),
                    IsActive = b.IsActive,
                    IsDeleted = b.IsDeleted,
                    IsExternal = b.IsExternal,
                    IsNew = b.IsNew,
                    ManagerId = b.ManagerId.HasValue? b.ManagerId.Value : 0,
                    Names = LocalizationMapper.Map(b.Names),
                    IsRoot = b.IsRoot,
                    TransactionsProcessingPeriod = b.TransactionsProcessingPeriod,
                    Users = OrgUnitMapper.Map(b.Users),
                    IsVirtualUnit = b.IsVirtualUnit,
                    Key = b.Key,
                    Number = b.Number,
                    ParentId = b.ParentId
                }).ToList();
            return organizationStructureInfoAddDTOs;
        }
        public static OrgStructureInfoDTO Map(OrgStructureInfoVM orgStructureInfoAddVM)
        {
            if (orgStructureInfoAddVM == null)
            {
                return null;
            }

            var organizationStructureInfoAddDTOs = new OrgStructureInfoDTO
            {
                BarCode = orgStructureInfoAddVM.BarCode,
                Counter = CounterMapper.Map(orgStructureInfoAddVM.Counter),
                IsActive = orgStructureInfoAddVM.IsActive,
                IsDeleted = orgStructureInfoAddVM.IsDeleted,
                IsExternal = orgStructureInfoAddVM.IsExternal,
                IsNew = orgStructureInfoAddVM.IsNew,
                ManagerId = orgStructureInfoAddVM.ManagerId,
                Names = LocalizationMapper.Map(orgStructureInfoAddVM.Names),
                TransactionsProcessingPeriod = orgStructureInfoAddVM.TransactionsProcessingPeriod,
                Users = OrgUnitMapper.Map(orgStructureInfoAddVM.Users),
                IsVirtualUnit = orgStructureInfoAddVM.IsVirtualUnit,
                Key = orgStructureInfoAddVM.Key,
                Number = orgStructureInfoAddVM.Number,
                ParentId = orgStructureInfoAddVM.ParentId,
                ExternalId = orgStructureInfoAddVM.ExternalId,
                IoDepartment = orgStructureInfoAddVM.IoDepartment,
                FollowUpDepartment = orgStructureInfoAddVM.FollowUpDepartment,
                IsExecutive = orgStructureInfoAddVM.IsExecutive,
                ReceiveElcOutBoundWithAcknowled = orgStructureInfoAddVM.ReceiveElcOutBoundWithAcknowled,
                SendSpecialCopy = orgStructureInfoAddVM.SendSpecialCopy,
                IsGeneralIoDepartment = orgStructureInfoAddVM.IsGeneralIoDepartment,
                Lineage=orgStructureInfoAddVM.Lineage,
            };
            return organizationStructureInfoAddDTOs;
        }
        public static List<OrgStructureInfoEditDTO> Map(IList<OrgStructureInfoEditVM> organizationStructureInfoEditVMs)
        {
            if (organizationStructureInfoEditVMs == null || !organizationStructureInfoEditVMs.Any())
            { return null; }
            List<OrgStructureInfoEditDTO> organizationStructureInfoEditDTOs = organizationStructureInfoEditVMs
                .Select(b => new OrgStructureInfoEditDTO
                {

                    BarCode = b.BarCode,
                    Counter = CounterMapper.Map(b.Counter),
                    IsActive = b.IsActive,
                    IsDeleted = b.IsDeleted,
                    IsExternal = b.IsExternal,
                    IsNew = b.IsNew,
                    ManagerId = b.ManagerId.HasValue ? b.ManagerId.Value : 0,
                    Names = LocalizationMapper.Map(b.Names),
                    IsRoot = b.IsRoot,
                    TransactionsProcessingPeriod = b.TransactionsProcessingPeriod,
                    Users = OrgUnitMapper.Map(b.Users),
                    IsVirtualUnit = b.IsVirtualUnit,
                    Key = b.Key,
                    Number = b.Number,
                    ParentId = b.ParentId
                }).ToList();
            return organizationStructureInfoEditDTOs;
        }
        public static List<OrgStructureInfoEditVM> Map(IList<OrgStructureInfoEditDTO> organizationStructureInfoEditDTOs, string cultureName)
        {
            if (organizationStructureInfoEditDTOs == null || !organizationStructureInfoEditDTOs.Any())
            { return null; }
            List<OrgStructureInfoEditVM> organizationStructureInfoEditVMs = organizationStructureInfoEditDTOs
                .Select(b => new OrgStructureInfoEditVM
                {

                    BarCode = b.BarCode,
                    Counter = CounterMapper.Map(b.Counter, cultureName),
                    IsActive = b.IsActive,
                    IsDeleted = b.IsDeleted,
                    IsExternal = b.IsExternal,
                    IsNew = b.IsNew,
                    ManagerId = b.ManagerId,
                    Names = LocalizationMapper.Map(b.Names),
                    IsRoot = b.IsRoot,
                    TransactionsProcessingPeriod = b.TransactionsProcessingPeriod,
                    Users = OrgUnitMapper.Map(b.Users),
                    IsVirtualUnit = b.IsVirtualUnit,
                    Key = b.Key,
                    Number = b.Number,
                    ParentId = b.ParentId
                }).ToList();
            return organizationStructureInfoEditVMs;
        }
        public static OrgUnitStructureDesignDTO Map(OrgUnitStructureDesignVM organizationUnitStructureDesignVMs)
        {
            if (organizationUnitStructureDesignVMs != null)
            {
                return new OrgUnitStructureDesignDTO
                {

                    Settings = organizationUnitStructureDesignVMs.Settings,
                    OrgUnits = OrgStructureInfoMapper.Map(organizationUnitStructureDesignVMs.OrgUnits)

                };
            }
            return null;
        }
        public static OrgUnitStructureDesignVM Map(OrgUnitStructureDesignDTO organizationUnitStructureDesignDTO, string cultureName)
        {
            if (organizationUnitStructureDesignDTO != null)
            {
                return new OrgUnitStructureDesignVM
                {
                    Settings = organizationUnitStructureDesignDTO.Settings,
                    OrgUnits = OrgStructureInfoMapper.Map(organizationUnitStructureDesignDTO.OrgUnits, cultureName)

                };
            }
            return null;
        }
        public static OrgStructureInfoVM Map(OrgStructureInfoDTO organizationStructureInfoDTO, string cultureName)
        {
            if (organizationStructureInfoDTO == null)
            {
                return null;
            }

            OrgStructureInfoVM organizationStructureInfoVM = new OrgStructureInfoVM
            {
                AssignmentPaper = AssignmentPaperMapper.Map(organizationStructureInfoDTO.AssignmentPaper),
                BarCode = organizationStructureInfoDTO.BarCode,
                BarcodeDesigners = BarcodeDesignerMapper.Map(organizationStructureInfoDTO.BarcodeDesigners),
                Counter = CounterMapper.Map(organizationStructureInfoDTO.Counter, cultureName),
                IdentifierId = organizationStructureInfoDTO.IdentifierId,
                IsActive = organizationStructureInfoDTO.IsActive,
                IsDeleted = organizationStructureInfoDTO.IsDeleted,
                IsExternal = organizationStructureInfoDTO.IsExternal,
                IsNew = organizationStructureInfoDTO.IsNew,
                ManagerId = organizationStructureInfoDTO.ManagerId,
                Names = LocalizationMapper.Map(organizationStructureInfoDTO.Names),
                StructureAsJson = organizationStructureInfoDTO.StructureAsJson,
                TransactionsProcessingPeriod = organizationStructureInfoDTO.TransactionsProcessingPeriod,
                Users = OrgUnitMapper.Map(organizationStructureInfoDTO.Users),
                IsVirtualUnit = organizationStructureInfoDTO.IsVirtualUnit,
                Key = organizationStructureInfoDTO.Key,
                LinkUnitsKeys = organizationStructureInfoDTO.LinkUnitsKeys,
                Name = organizationStructureInfoDTO.Name,
                Number = organizationStructureInfoDTO.Number,
                ParentId = organizationStructureInfoDTO.ParentId,
                HasChilds = organizationStructureInfoDTO.HasChilds,
                Lineage = organizationStructureInfoDTO.Lineage,
            };
            return organizationStructureInfoVM;
        }
    }
}