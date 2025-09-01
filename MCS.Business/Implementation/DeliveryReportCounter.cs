using System;
using System.Collections.Generic;
using MCS.Common;
using MCS.Domain;

namespace MCS.Business
{
    public class DeliveryReportCounter
    {
        private static readonly object _deliveryReport = new object();
        private static DeliveryReportCounter _instance;

        private DeliveryReportCounter() { }

        public static DeliveryReportCounter GetInstance()
        {
            lock (_deliveryReport)
            {
                if (_instance == null)
                {
                    _instance = new DeliveryReportCounter();
                }
            }

            return _instance;
        }

        public int Next()
        {
            int result;

            ISettingBL settingBL = new SettingBL();
            List<Setting> settings = settingBL.GetSettingByKey(SettingsKeys.DeliveryReportCounter);
            Setting setting = settings.Find(a => a.Key == SettingsKeys.DeliveryReportCounter);
            result = Int32.Parse(setting.Value);

            setting.Value = (result + 1).ToString();

            settingBL.UpdateSetting(setting);

            return result;
        }
    }
}
