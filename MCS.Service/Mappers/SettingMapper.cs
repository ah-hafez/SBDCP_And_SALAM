using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class SettingMapper
    {
        public static List<SettingDTO> Map(List<Setting> settings)
        {
            if (settings == null || !settings.Any())
            {
                return new List<SettingDTO>();
            }
            List<SettingDTO> settingDTOs = settings
                .Select(b => new SettingDTO
                {
                    Id = b.Id,
                    Key = b.Key,
                    Value = b.Value,
                    BLOBValue = b.BLOBValue,
                    Type = b.Type,
                    Description = b.Description,
                    ModelId = b.ModelId,
                    ResourceId = b.ResourceId,
                    IsReadOnly = b.IsReadOnly
                }).ToList();
            return settingDTOs;
        }

        public static SettingDTO Map(Setting b)
        {
            if (b == null )
            {
                return new SettingDTO();
            }

            SettingDTO settingDTO = new SettingDTO()
            {
                Id = b.Id,
                Key = b.Key,
                Value = b.Value,
                BLOBValue = b.BLOBValue,
                Type = b.Type,
                Description = b.Description,
                ModelId = b.ModelId,
                ResourceId = b.ResourceId,
                IsReadOnly = b.IsReadOnly
            };

            return settingDTO;
           
        }
        public static List<Setting> Map(List<SettingDTO> settingDTOs)
        {
            if (settingDTOs == null || !settingDTOs.Any())
            {
                return new List<Setting>();
            }
            List<Setting> settings = settingDTOs
                .Select(b => new Setting
                {
                    Id = b.Id,
                    Value = b.Value,
                    BLOBValue = b.BLOBValue
                }).ToList();
            return settings;
        }
    }
}