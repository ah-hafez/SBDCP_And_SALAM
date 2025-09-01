using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using Action = MCS.Domain.Action;

namespace MCS.Business
{
    public class ActionBL : BaseBL, IActionBL
    {
        public int AddAction(Action action)
        {
            try
            {
                IActionRepository oActionRepository = IoC.Resolve<IActionRepository>();
                var AddAction = oActionRepository.AddAction(action);
                CacheHelper.RemoveBasedOnPrefix(CachedObjectsKey.Actions);
                return AddAction;
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

        public void UpdateAction(Action oAction)
        {
            try
            {
                IActionRepository oActionRepository = IoC.Resolve<IActionRepository>();
                oActionRepository.UpdateAction(oAction);
                CacheHelper.RemoveBasedOnPrefix(CachedObjectsKey.Actions);

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

        public void DeleteAction(IList<int> ids, out IList<int> actionesCannotBeDeleted)
        {
            try
            {
                IActionRepository oActionRepository = IoC.Resolve<IActionRepository>();
                actionesCannotBeDeleted = new List<int>();
                foreach (var id in ids)
                {
                    if (oActionRepository.CheckIfActionUsed(id))
                    {
                        actionesCannotBeDeleted.Add(id);
                        continue;
                    }
                    oActionRepository.DeleteAction(id);
                }
                CacheHelper.RemoveBasedOnPrefix(CachedObjectsKey.Actions);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.ActionIsUsed);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Action> GetAllAction(string cultureName)
        {
            try
            {
                IList<Action> actions = CacheHelper.Get(CachedObjectsKey.Actions, cultureName) as IList<Action>;
                if (actions == null)
                {
                    IActionRepository oActionRepository = IoC.Resolve<IActionRepository>();
                    actions = oActionRepository.GetAllActions(cultureName);
                    CacheHelper.Insert(CachedObjectsKey.Actions, actions, cultureName);
                }
                return actions;
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

        public IList<Action> GetAction(SearchCriteria searchCriteria, out int rowsCount, string cultureName)
        {
            try
            {
                IActionRepository oActionRepository = IoC.Resolve<IActionRepository>();
                return oActionRepository.GetActions(searchCriteria, out rowsCount, cultureName);
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

        public Action GetActionById(int nProcessId)
        {
            try
            {
                IActionRepository oActionRepository = IoC.Resolve<IActionRepository>();
                return oActionRepository.Get(nProcessId);
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

        public void ChangeEntitiesNameBeforeMove(ChangeEntityName changeEntityName)
        {
            try
            {
                IActionRepository ActionRepository = IoC.Resolve<IActionRepository>();
                ActionRepository.ChangeEntitiesNameBeforeMove(changeEntityName);
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
        public List<UsersClearance> CheckUserClearance(List<int> usersIds, string cultureName)
        {
            try
            {
                IActionRepository ActionRepository = IoC.Resolve<IActionRepository>();
                return ActionRepository.CheckUserClearance(usersIds, cultureName);
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
