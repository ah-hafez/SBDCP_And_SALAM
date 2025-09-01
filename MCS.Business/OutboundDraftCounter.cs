using System;
using System.Collections.Generic;
using MCS.Common;
using MCS.Domain;

namespace MCS.Business
{
    public class OutboundDraftCounter
    {
        private static readonly object _transaction = new object();
        private static volatile OutboundDraftCounter _instance;

        private OutboundDraftCounter()
        {
        }

        public static OutboundDraftCounter Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_transaction)
                    {
                        if (_instance == null)
                        {
                            _instance = new OutboundDraftCounter();
                        }
                    }
                }

                return _instance;
            }
        }

        public int Next()
        {
            ISettingBL settingBL = new SettingBL();

            List<Setting> settings = settingBL.GetSettingByKey(SettingsKeys.OutboundDraftCounterKey);
            Setting setting = settings.Find(a => a.Key == SettingsKeys.OutboundDraftCounterKey);

            int result = 0;

            if (!string.IsNullOrEmpty(setting.Value))
            {
                result = Convert.ToInt32(setting.Value);
            }

            result += 1;

            setting.Value = (result).ToString();

            settingBL.UpdateSetting(setting);

            return result;
        }
    }
}
