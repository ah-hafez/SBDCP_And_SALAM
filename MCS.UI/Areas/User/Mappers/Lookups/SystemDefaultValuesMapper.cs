using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Mappers.Lookups
{
    public static class SystemDefaultValuesMapper
    {
        public static List<SystemDefaultValuesVM> Map(IList<SystemDefaultValuesDTO> systemDefaultValuesDTOs)
        {
            if (systemDefaultValuesDTOs == null || !systemDefaultValuesDTOs.Any())
            {
                return new List<SystemDefaultValuesVM>();
            }
            List<SystemDefaultValuesVM> letterTypeVMs = systemDefaultValuesDTOs
                .Select(systemDefaultValuesDTO => new SystemDefaultValuesVM()
                {
                    Id = systemDefaultValuesDTO.Id,
                    CategoryId = systemDefaultValuesDTO.CategoryId,
                    TypeId = systemDefaultValuesDTO.TypeId,
                    DefaultValueId = systemDefaultValuesDTO.DefaultValueId
                }).ToList();
            return letterTypeVMs;
        }     
    }
}