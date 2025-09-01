using System.Collections.Generic;

namespace MCS.UI.Areas.Admin.Models.Shared
{
    public class SettingVM
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string BLOBValue { get; set; }
        public int? Type { get; set; }
        public string ResourceId { get; set; }
        public int ModelId { get; set; }
        public bool IsReadOnly { get; set; }
        public SettingConfigVM SettingConfigVM { get; set; }
    }
    public class SuperSettingVM
    {
        public string SubTitle { get; set; }
        public bool EnableSave { get; set; } = true;
        public List<SettingVM> SettingVMs { get; set; }
    }
}