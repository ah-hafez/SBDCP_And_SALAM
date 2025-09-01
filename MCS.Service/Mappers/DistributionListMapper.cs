using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class DistributionListMapper
    {
        public static DistributionListDTO Map(DistributionList distributionList, string cultureName)
        {
            if (distributionList == null)
            {
                return new DistributionListDTO();
            }

            DistributionListDTO distributionListDTO = new DistributionListDTO
            {
                Id = distributionList.Id,
                UserId = distributionList.UserId,
                OrgUnitId = distributionList.OrgUnitId,
                LocalizationIdentifierId = distributionList.LocalizationIdentifierId,
                Name = LocalizationIdentifierMapper.Map(distributionList.Name.Localizations),
                OrgUnitName = distributionList.OrgUnit.LocalizationIdentifier.Localizations.Where(s => s.Culture.ShortName == cultureName).FirstOrDefault().Text,
                UserName = distributionList.User != null ?  distributionList.User.LocalizationIdentifier.Localizations.Where(s => s.Culture.ShortName == cultureName).FirstOrDefault().Text  : "",
                DistributionListDetails = Map(distributionList.DistributionListDetails, cultureName),
                CreatedBy = distributionList.CreatedBy,
                CreatedOn = distributionList.CreatedOn,
                ModefiedBy = distributionList.ModefiedBy,
                ModefiedOn = distributionList.ModefiedOn
            };
            return distributionListDTO;
        }
        public static DistributionList Map(DistributionListDTO distributionListDTO)
        {
            if (distributionListDTO == null)
            {
                return new DistributionList();
            }

            DistributionList distributionList = new DistributionList
            {
                Id = distributionListDTO.Id,
                UserId = distributionListDTO.UserId,
                OrgUnitId = distributionListDTO.OrgUnitId,
                LocalizationIdentifierId = distributionListDTO.LocalizationIdentifierId,
                Name = LocalizationIdentifierMapper.Map(distributionListDTO.Name),
                DistributionListDetails = Map(distributionListDTO.DistributionListDetails),
                CreatedBy = distributionListDTO.CreatedBy,
                CreatedOn = distributionListDTO.CreatedOn,
                ModefiedBy = distributionListDTO.ModefiedBy,
                ModefiedOn = distributionListDTO.ModefiedOn
            };
            return distributionList;
        }
        public static List<DistributionList> Map(List<DistributionListDTO> distributionListDTOs)
        {
            if (!distributionListDTOs.Any())
            {
                return new List<DistributionList>();
            }

            List<DistributionList> distributionLists = distributionListDTOs.Select(DTO =>
            {
                DistributionList distributionList = new DistributionList
                {
                    Id = DTO.Id,
                    UserId = DTO.UserId,
                    OrgUnitId = DTO.OrgUnitId,
                    LocalizationIdentifierId = DTO.LocalizationIdentifierId,
                    Name = LocalizationIdentifierMapper.Map(DTO.Name),
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
        public static List<DistributionListDTO> Map(List<DistributionList> distributionLists, string cultureName)
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
                    UserId = Model.UserId,
                    OrgUnitId = Model.OrgUnitId,
                    OrgUnitName = Model.OrgUnit.LocalizationIdentifier.Localizations.Where(s => s.Culture.ShortName == cultureName).FirstOrDefault().Text,
                    UserName = Model.UserId != null ? Model.User.LocalizationIdentifier.Localizations.Where(s => s.Culture.ShortName == cultureName).FirstOrDefault().Text : "",
                    LocalizationIdentifierId = Model.LocalizationIdentifierId,
                    Name = LocalizationIdentifierMapper.Map(Model.Name.Localizations),
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
        public static List<DistributionListDetails> Map(IList<DistributionListDetailsDTO> distributionListDetailsDTO)
        {
            if (distributionListDetailsDTO == null)
            {
                return new List<DistributionListDetails>();
            }
            List<DistributionListDetails> distributionListDetails = distributionListDetailsDTO.Select(Dld =>
            {
                DistributionListDetails distributionListDetail = new DistributionListDetails
                {
                    Id = Dld.Id,
                    UserId = Dld.UserId > 0 ? Dld.UserId : (int?)null,
                    OrgUnitId = Dld.OrgUnitId,
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
        public static List<DistributionListDetailsDTO> Map(IList<DistributionListDetails> distributionListDetails, string cultureName)
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
                    UserId = Dld.UserId ?? -1,
                    OrgUnitId = Dld.OrgUnitId,
                    DistributionListId = Dld.DistributionListId,
                    OrgUnitName = Dld.OrgUnit.LocalizationIdentifier.Localizations.Where(s => s.Culture.ShortName == cultureName).FirstOrDefault().Text,
                    UserName = Dld?.User?.LocalizationIdentifier?.Localizations.Where(s => s.Culture.ShortName == cultureName)?.FirstOrDefault()?.Text,
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