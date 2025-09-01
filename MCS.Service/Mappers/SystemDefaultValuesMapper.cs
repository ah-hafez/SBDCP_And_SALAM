using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class SystemDefaultValuesMapper
    {
        private static SystemDefaultValuesDTO MapSystemDefaultValue(SystemDefaultValues systemDefaultValues)
        {
            if (systemDefaultValues == null)
                return null;
            SystemDefaultValuesDTO systemDefaultValuesDTO = new SystemDefaultValuesDTO()
            {
                Id = systemDefaultValues.Id,
                CategoryId = systemDefaultValues.CategoryId,
                TypeId = systemDefaultValues.TypeId,
                DefaultValueId = systemDefaultValues.DefaultValueId
            };           

            return systemDefaultValuesDTO;
        }



        public static List<SystemDefaultValuesDTO> Map(IList<SystemDefaultValues> systemDefaultValues)
        {
            List<SystemDefaultValuesDTO> systemDefaultValuesDTOs = new List<SystemDefaultValuesDTO>();

            foreach (SystemDefaultValues systemDefaultValue in systemDefaultValues)
            {
                systemDefaultValuesDTOs.Add(SystemDefaultValuesMapper.MapSystemDefaultValue(systemDefaultValue));
            }

            return systemDefaultValuesDTOs;
        }

    }
}