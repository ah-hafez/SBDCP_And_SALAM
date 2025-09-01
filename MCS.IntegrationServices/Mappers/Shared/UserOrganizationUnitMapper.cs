using System.Collections.Generic;
using System.Linq;
using MCS.DTO;


namespace MCS.IntegrationServices.Mappers
{
    public static class UserOrgUnitMapper
    {
        public static List<UserOrgUnitDTO> Map(IList<UserOrgUnitVM> userOrgUnitVMs)
        {
            if (userOrgUnitVMs == null || !userOrgUnitVMs.Any())
            {
                return new List<UserOrgUnitDTO>();
            }
            List<UserOrgUnitDTO> userOrgUnitDTOs = userOrgUnitVMs
                .Select(b => new UserOrgUnitDTO
                {
                    Id = b.Id,
                    IsSelected = b.IsSelected,
                    LoclizationName = LocalizationMapper.Map(b.LoclizationName),
                    Name = b.Name
                }).ToList();
            return userOrgUnitDTOs;

        }
        public static List<UserOrgUnitVM> Map(IList<UserOrgUnitDTO> userOrgUnitDTOs)
        {
            if (userOrgUnitDTOs == null || !userOrgUnitDTOs.Any())
            {
                return new List<UserOrgUnitVM>();
            }
            List<UserOrgUnitVM> userOrgUnitVMs = userOrgUnitDTOs
                .Select(b => new UserOrgUnitVM
                {
                    Id = b.Id,
                    IsSelected = b.IsSelected,
                    LoclizationName = LocalizationMapper.Map(b.LoclizationName),
                    Name = b.Name,

                }).ToList();
            return userOrgUnitVMs;

        }
        public static UserOrgUnitVM Map(UserOrgUnitDTO userOrgUnitDTO)
        {
            if (userOrgUnitDTO != null)
            {
                UserOrgUnitVM userOrgUnitVM = new UserOrgUnitVM
                {
                    Id = userOrgUnitDTO.Id,
                    IsSelected = userOrgUnitDTO.IsSelected,
                    LoclizationName = LocalizationMapper.Map(userOrgUnitDTO.LoclizationName),
                    Name = userOrgUnitDTO.Name
                };
                return userOrgUnitVM;
            }
            return new UserOrgUnitVM();

        }
        public static UserOrgUnitDTO Map(UserOrgUnitVM userOrgUnitVM)
        {
            if (userOrgUnitVM != null)
            {
                UserOrgUnitDTO userOrgUnitDTO = new UserOrgUnitDTO
                {
                    Id = userOrgUnitVM.Id,
                    IsSelected = userOrgUnitVM.IsSelected,
                    LoclizationName = LocalizationMapper.Map(userOrgUnitVM.LoclizationName),
                    Name = userOrgUnitVM.Name
                };
                return userOrgUnitDTO;
            }
            return new UserOrgUnitDTO();

        }

    }
}