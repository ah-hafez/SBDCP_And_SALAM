using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Mappers
{
    public class ConfidentialityLevelMapper
    {
        public static ConfidentialityLevelVM Map(ConfidentialityLevelDTO confidentialityLevelDTO)
        {
            if (confidentialityLevelDTO == null)
            {
                return null;
            }
            return new ConfidentialityLevelVM
            {
                Id = confidentialityLevelDTO.Id,
                LocalizationVMs = LocalizationMapper.Map(confidentialityLevelDTO.LocalizationDTOs)
            };
        }
        public static List<ConfidentialityLevelVM> Map(List<ConfidentialityLevelDTO> confidentialityLevelDTOs)
        {
            if (confidentialityLevelDTOs == null)
            {
                return null;
            }

            return confidentialityLevelDTOs.Select(c => Map(c)).ToList();
        }
    }
}