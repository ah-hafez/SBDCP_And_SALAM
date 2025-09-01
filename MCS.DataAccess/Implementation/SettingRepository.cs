using System;
using System.Collections.Generic;
using MCS.Common.TransactionContext;
using MCS.Domain;
using System.Linq;
using System.Data.Entity;

namespace MCS.DataAccess
{
    public class SettingRepository : BaseRepository<Setting>, ISettingRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public SettingRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int AddSetting(Setting setting)
        {
            try
            {
                _oMCSDbContext.Settings.Add(setting);

                _oMCSDbContext.SaveChanges();

                return setting.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateSetting(Setting setting)
        {
            try
            {
                Setting settingOld = GetSettingByKey(setting.Key);

                if (settingOld != null)
                {
                    _oMCSDbContext.Entry(settingOld).CurrentValues.SetValues(setting);
                    _oMCSDbContext.Entry(settingOld).State = EntityState.Modified;
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Setting GetSettingByKey(string settingKey)
        {
            try
            {
                return this.FindBy(s => s.Key == settingKey);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<Setting> GetSettingByModelId(int modelId)
        {
            try
            {
                return _oMCSDbContext.Settings.Where(a => a.ModelId == modelId).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateSettings(List<Setting> settings)
        {
            try
            {
                foreach (var item in settings)
                {
                    var itemToUpdate = _oMCSDbContext.Settings.FirstOrDefault(a => a.Id == item.Id && !a.IsReadOnly);
                    if (itemToUpdate != null)
                    {
                        itemToUpdate.Value = item.Value;
                        itemToUpdate.BLOBValue = item.BLOBValue;
                        _oMCSDbContext.Entry(itemToUpdate).State = EntityState.Modified;
                        _oMCSDbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        #endregion Methods
    }
}
