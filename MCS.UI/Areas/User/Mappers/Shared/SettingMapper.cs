using System;
using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Shared;

namespace MCS.UI.Areas.User.Mappers.Shared
{
    public static class SettingMapper
    {
        public static List<SettingDTO> Map(IList<SettingVM> settingVMs)
        {
            if (settingVMs == null || !settingVMs.Any())
            {
                return new List<SettingDTO>();
            }
            List<SettingDTO> settingDTOs = settingVMs
                .Select(b => new SettingDTO
                {
                    Id = b.Id,
                    Key = b.Key,
                    Value = b.Value,
                    BLOBValue = b.BLOBValue != null ? Convert.FromBase64String(b.BLOBValue) : null,
                    Type = b.Type,
                    ModelId = b.ModelId,
                    ResourceId = b.ResourceId,
                    IsReadOnly = b.IsReadOnly
                }).ToList();
            return settingDTOs;

        }
        public static List<SettingVM> Map(IList<SettingDTO> settingDTOs)
        {
            if (settingDTOs == null || !settingDTOs.Any())
            {
                return new List<SettingVM>();
            }
            List<SettingVM> settingVMs = settingDTOs
                .Select(b => new SettingVM
                {
                    Id = b.Id,
                    Key = b.Key,
                    Value = b.Value,
                    BLOBValue = b.BLOBValue != null ? Convert.ToBase64String(b.BLOBValue) : string.Empty,
                    Type = b.Type,
                    ModelId = b.ModelId,
                    ResourceId = b.ResourceId,
                    IsReadOnly = b.IsReadOnly
                }).ToList();
            return settingVMs;

        }
        public static SettingVM Map(SettingDTO b)
        {
            if (b == null)
            {
                return new SettingVM();
            }

            SettingVM settingVM = new SettingVM()
            {
                Id = b.Id,
                Key = b.Key,
                Value = b.Value,
                BLOBValue = b.BLOBValue != null ? Convert.ToBase64String(b.BLOBValue) : string.Empty,
                Type = b.Type,
                ModelId = b.ModelId,
                ResourceId = b.ResourceId,
                IsReadOnly = b.IsReadOnly
            };

            return settingVM;
        }
    }
}