using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class ConfidentialityLevelMapper
    {
        public static ConfidentialityLevelDTO Map(ConfidentialityLevel confidentialityLevel)
        {
            if (confidentialityLevel == null)
            {
                return null;
            }
            return new ConfidentialityLevelDTO
            {
                Id = confidentialityLevel.Id,
                LocalizationDTOs = LocalizationIdentifierMapper.Map(confidentialityLevel.Localization)
            };
        }
        public static List<ConfidentialityLevelDTO> Map(List<ConfidentialityLevel> confidentialityLevels)
        {
            if (confidentialityLevels == null)
            {
                return null;
            }

            return confidentialityLevels.Select(c => Map(c)).ToList();
        }
    }
}