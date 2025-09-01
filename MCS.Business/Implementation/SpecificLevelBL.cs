using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class SpecificLevelBL : BaseBL, ISpecificLevelBL
    {
        public int AddSpecificLevel(SpecificLevel specificLevel)
        {
            try
            {
                ISpecificLevelRepository specificLevelRepository = IoC.Resolve<SpecificLevelRepository>();

                int specificLevelId = specificLevelRepository.AddSpecificLevel(specificLevel);

                return specificLevelId;
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

        public void UpdateSpecificLevel(SpecificLevel specificLevel)
        {
            try
            {
                ISpecificLevelRepository specificLevelRepository = IoC.Resolve<SpecificLevelRepository>();

                specificLevelRepository.UpdateSpecificLevel(specificLevel);
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

        public SpecificLevel GetSpecificLevelById(int specificLevelId)
        {
            try
            {
                ISpecificLevelRepository specificLevelRepository = IoC.Resolve<SpecificLevelRepository>();

                return specificLevelRepository.Get(specificLevelId);
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

        public void DeleteSpecificLevels(IList<int> ids, out IList<int> specificLevelsCannotBeDeleted)
        {
            try
            {
                ISpecificLevelRepository specificLevelRepository = IoC.Resolve<SpecificLevelRepository>();

                IList<Transaction> transactions;

                specificLevelsCannotBeDeleted = new List<int>();

                foreach (var id in ids)
                {
                    //transactions = TransactionBL.GetTransactions(t => t.SpecificLevel.Id == id);

                    //if (transactions.Count > 0)
                    //{
                    //    specificLevelsCannotBeDeleted.Add(id);

                    //    continue;
                    //}

                    specificLevelRepository.DeleteSpecificLevel(id);
                }
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

        public IList<SpecificLevel> GetSpecificLevels(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                ISpecificLevelRepository specificLevelRepository = IoC.Resolve<SpecificLevelRepository>();

                return specificLevelRepository.GetSpecificLevels(searchCriteria, out rowsCount);
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

        public IList<SpecificLevel> GetSpecificLevels(TransactionCategories transactionCategories, string cultureName)
        {
            try
            {
                IList<SpecificLevel> specificLevels = CacheHelper.Get(CachedObjectsKey.SpecificLevels, cultureName) as IList<SpecificLevel>;

                if (specificLevels == null || specificLevels.Count == 0)
                {
                    ISpecificLevelRepository specificLevelRepository = IoC.Resolve<SpecificLevelRepository>();

                    specificLevels = specificLevelRepository.GetSpecificLevels(cultureName);

                    CacheHelper.Insert(CachedObjectsKey.SpecificLevels, specificLevels, cultureName);
                }

                return specificLevels.Where(l => l.TransactionCategories.HasFlag(transactionCategories) || l.TransactionCategories.HasFlag(TransactionCategories.None)).ToList();
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
