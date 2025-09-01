using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Models.UserPreferences;

namespace MCS.UI.Areas.User.Mappers
{
    public static class DistributionListMapper
    {
        public static DistributionListDTO Map(DistributionListVM distributionList, string cultureName)
        {
            if (distributionList == null)
            {
                return new DistributionListDTO();
            }

            DistributionListDTO distributionListDTO = new DistributionListDTO
            {
                Id = distributionList.Id,
                UserId = distributionList.UserId.Value,
                OrgUnitId = distributionList.OrgUnitId.Value,
                LocalizationIdentifierId = distributionList.LocalizationIdentifierId,
                Name = LocalizationMapper.Map(distributionList.Name),
                OrgUnitName = distributionList.OrgUnitName,
                UserName = distributionList.UserName,
                DistributionListDetails = Map(distributionList.DistributionListDetails, cultureName),
                CreatedBy = distributionList.CreatedBy,
                CreatedOn = distributionList.CreatedOn,
                ModefiedBy = distributionList.ModefiedBy,
                ModefiedOn = distributionList.ModefiedOn
            };
            return distributionListDTO;
        }
        public static DistributionListVM Map(DistributionListDTO distributionListDTO)
        {
            if (distributionListDTO == null)
            {
                return new DistributionListVM();
            }

            DistributionListVM distributionList = new DistributionListVM
            {
                Id = distributionListDTO.Id,
                UserId = distributionListDTO.UserId,
                OrgUnitId = distributionListDTO.OrgUnitId,
                UserName = distributionListDTO.UserName,
                OrgUnitName = distributionListDTO.OrgUnitName,
                LocalizationIdentifierId = distributionListDTO.LocalizationIdentifierId,
                Name = LocalizationMapper.Map(distributionListDTO.Name),
                DistributionListDetails = Map(distributionListDTO.DistributionListDetails),
                CreatedBy = distributionListDTO.CreatedBy,
                CreatedOn = distributionListDTO.CreatedOn,
                ModefiedBy = distributionListDTO.ModefiedBy,
                ModefiedOn = distributionListDTO.ModefiedOn
            };
            return distributionList;
        }
        public static List<DistributionListVM> Map(List<DistributionListDTO> distributionListDTOs)
        {
            if (!distributionListDTOs.Any())
            {
                return new List<DistributionListVM>();
            }

            List<DistributionListVM> distributionLists = distributionListDTOs.Select(DTO =>
            {
                DistributionListVM distributionList = new DistributionListVM
                {
                    Id = DTO.Id,
                    UserId = DTO.UserId,
                    OrgUnitId = DTO.OrgUnitId,
                    UserName = DTO.UserName,
                    OrgUnitName = DTO.OrgUnitName,
                    LocalizationIdentifierId = DTO.LocalizationIdentifierId,
                    Name = LocalizationMapper.Map(DTO.Name),
                    DistributionListDetails = Map(DTO.DistributionListDetails),
                    CreatedBy = DTO.CreatedBy,
                    CreatedOn = DTO.CreatedOn,
                    ModefiedBy = DTO.ModefiedBy,
                    ModefiedOn = DTO.ModefiedOn
                };
                return distributionList;
            }).ToList();

            return distributionLists;
        }
        public static List<DistributionListDTO> Map(List<DistributionListVM> distributionLists, string cultureName)
        {
            if (!distributionLists.Any())
            {
                return new List<DistributionListDTO>();
            }

            List<DistributionListDTO> distributionListDTOs = distributionLists.Select(Model =>
            {
                DistributionListDTO distributionListDTO = new DistributionListDTO
                {
                    Id = Model.Id,
                    UserId = Model.UserId.Value,
                    OrgUnitId = Model.OrgUnitId.Value,
                    OrgUnitName = Model.OrgUnitName,
                    UserName = Model.UserName,
                    LocalizationIdentifierId = Model.LocalizationIdentifierId,
                    Name = LocalizationMapper.Map(Model.Name),
                    DistributionListDetails = Map(Model.DistributionListDetails, cultureName),
                    CreatedBy = Model.CreatedBy,
                    CreatedOn = Model.CreatedOn,
                    ModefiedBy = Model.ModefiedBy,
                    ModefiedOn = Model.ModefiedOn
                };
                return distributionListDTO;
            }).ToList();
            return distributionListDTOs;
        }
        public static List<DistributionListDetailsVM> Map(IList<DistributionListDetailsDTO> distributionListDetailsDTO)
        {
            if (distributionListDetailsDTO == null)
            {
                return new List<DistributionListDetailsVM>();
            }
            List<DistributionListDetailsVM> distributionListDetails = distributionListDetailsDTO.Select(Dld =>
            {
                DistributionListDetailsVM distributionListDetail = new DistributionListDetailsVM
                {
                    Id = Dld.Id,
                    UserId = Dld.UserId,
                    OrgUnitId = Dld.OrgUnitId,
                    UserName = Dld.UserId > 0 ? Dld.UserName : ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Trays.Orgunit"),
                    OrgUnitName = Dld.OrgUnitName,
                    DistributionListId = Dld.DistributionListId,
                    CreatedOn = Dld.CreatedOn,
                    CreatedBy = Dld.CreatedBy,
                    ModefiedOn = Dld.ModefiedOn,
                    ModefiedBy = Dld.ModefiedBy
                };
                return distributionListDetail;
            }).ToList();
            return distributionListDetails;
        }
        public static List<DistributionListDetailsDTO> Map(IList<DistributionListDetailsVM> distributionListDetails, string cultureName)
        {
            if (distributionListDetails == null)
            {
                return new List<DistributionListDetailsDTO>();
            }
            List<DistributionListDetailsDTO> distributionListDetailsDTOs = distributionListDetails.Select(Dld =>
            {
                DistributionListDetailsDTO distributionListDetailsDTO = new DistributionListDetailsDTO
                {
                    Id = Dld.Id,
                    UserId = Dld.UserId,
                    OrgUnitId = Dld.OrgUnitId,
                    DistributionListId = Dld.DistributionListId,
                    OrgUnitName = Dld.OrgUnitName,
                    UserName = Dld.UserName,
                    CreatedOn = Dld.CreatedOn,
                    CreatedBy = Dld.CreatedBy,
                    ModefiedOn = Dld.ModefiedOn,
                    ModefiedBy = Dld.ModefiedBy
                };
                return distributionListDetailsDTO;
            }).ToList();
            return distributionListDetailsDTOs;
        }

    }
}