using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class SettingBL : BaseBL, ISettingBL
    {
        public void UpdateSetting(Setting setting)
        {
            try
            {
                ISettingRepository settingRepository = IoC.Resolve<SettingRepository>();

                settingRepository.UpdateSetting(setting);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public List<Setting> GetSettingByKey(string settingKey)
        {
            try
            {
                List<Setting> settings = CacheHelper.Get(CachedObjectsKey.Settings, "ar") as List<Setting>;
                ISettingRepository settingRepository = IoC.Resolve<SettingRepository>();
                if (settings == null)

                {
                    settings = new List<Setting>();
                    Setting RepositoryItem = settingRepository.GetSettingByKey(settingKey);
                    settings.Add(RepositoryItem);
                    CacheHelper.Insert(CachedObjectsKey.Settings, settings, "ar");
                    CacheHelper.Insert(CachedObjectsKey.Settings, settings, "en");
                }
                else
                {
                    if (settings.Find(a => a.Key == settingKey) == null)
                    {
                        Setting RepositoryItem = settingRepository.GetSettingByKey(settingKey);
                        settings.Add(RepositoryItem);
                        CacheHelper.Insert(CachedObjectsKey.Settings, settings, "ar");
                        CacheHelper.Insert(CachedObjectsKey.Settings, settings, "en");
                    }
                }
                return settings;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public List<Setting> GetSettingByModelId(int modelId)
        {
            try
            {
                ISettingRepository settingRepository = IoC.Resolve<SettingRepository>();
                return settingRepository.GetSettingByModelId(modelId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void UpdateSettings(List<Setting> settings)
        {
            try
            {
                ISettingRepository settingRepository = IoC.Resolve<SettingRepository>();
                List<Setting> settingCashe = CacheHelper.Get(CachedObjectsKey.Settings, "ar") as List<Setting>;
                if (settingCashe != null)
                {
                    foreach (Setting item in settings)
                    {
                        var removeItem = settingCashe.Find(a => a.Id == item.Id);
                        if (removeItem != null)
                        {
                            settingCashe.Remove(removeItem);
                        }
                    };
                }
                settingRepository.UpdateSettings(settings);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
    }
}
