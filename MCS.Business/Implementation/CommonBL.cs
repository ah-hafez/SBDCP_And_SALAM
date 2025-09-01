using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class CommonBL : BaseBL, ICommonBL
    {
        public IList<Culture> GetCultures()
        {
            try
            {
                IList<Culture> cultures = CacheHelper.Get(CachedObjectsKey.Cultures, null) as IList<Culture>;

                if (cultures == null)
                {
                    ICultureRepository cultureRepository = IoC.Resolve<CultureRepository>();

                    cultures = cultureRepository.GetCultures();

                    CacheHelper.Insert(CachedObjectsKey.Cultures, cultures, null);
                }

                return cultures;
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

        public Culture GetCultureById(int id)
        {
            try
            {
                ICultureRepository cultureRepository = IoC.Resolve<CultureRepository>();

                return cultureRepository.Get(id);
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

        public IList<Theme> GetThemes()
        {
            try
            {
                //IList<Theme> theme = CacheHelper.Get(CachedObjectsKey.Theme, null) as IList<Theme>;

                //if (theme == null)
                //{
                //    IThemeRepository themeRepository = IoC.Resolve<ThemeRepository>();

                //    theme = themeRepository.GetTheme();

                //    CacheHelper.Insert(CachedObjectsKey.Theme, theme, null);
                //}
                IUserPreferenceRepository themeRepository = IoC.Resolve<UserPreferenceRepository>();
                return themeRepository.GetTheme();
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


        public bool AddUserOnline(int userId,int OrgUnitId, string connectionId)
        {
            try
            {
                IOnlineUserRepository onlineUserRepository = IoC.Resolve<OnlineUserRepository>();
                return onlineUserRepository.AddUserOnline(userId, OrgUnitId, connectionId);
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
        public bool UpdateUserOnline(int userId, int OrgUnitId)
        {
            try
            {
                IOnlineUserRepository onlineUserRepository = IoC.Resolve<OnlineUserRepository>();
                return onlineUserRepository.UpdateUserOnline(userId, OrgUnitId);
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
        public bool DeleteOnlineUser(string connectionId)
        {
            try
            {
                IOnlineUserRepository onlineUserRepository = IoC.Resolve<OnlineUserRepository>();
                return onlineUserRepository.DeleteOnlineUser(connectionId);
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

        public List<OnlineUser> GetOnlineUser()
        {
            try
            {
                IOnlineUserRepository onlineUserRepository = IoC.Resolve<OnlineUserRepository>();
                return onlineUserRepository.GetOnlineUser();
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
