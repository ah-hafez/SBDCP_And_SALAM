using System.Collections.Generic;
using MCS.Domain;

namespace MCS.Business
{
    public interface ISettingBL
    {
        void UpdateSetting(Setting setting);
        List<Setting> GetSettingByKey(string settingKey);
        List<Setting> GetSettingByModelId(int modelId);
        void UpdateSettings(List<Setting> settings);
    }
}
