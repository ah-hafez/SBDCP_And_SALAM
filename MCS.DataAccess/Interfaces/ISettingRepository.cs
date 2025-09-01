using System.Collections.Generic;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface ISettingRepository : IRepository<Setting>
    {
        int AddSetting(Setting setting);
        void UpdateSetting(Setting setting);
        Setting GetSettingByKey(string settingKey);
        List<Setting> GetSettingByModelId(int modelId);
        void UpdateSettings(List<Setting> settings);
    }
}
