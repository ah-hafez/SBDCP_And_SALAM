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
    public class LetterTypeBL : BaseBL, ILetterTypeBL
    {
        public int AddLetterType(LetterType letterType)
        {
            try
            {
                ILetterTypeRepository letterTypeRepository = IoC.Resolve<LetterTypeRepository>();

                int letterTypeId = letterTypeRepository.AddLetterType(letterType);

                return letterTypeId;
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

        public void UpdateLetterType(LetterType letterType)
        {
            try
            {
                ILetterTypeRepository letterTypeRepository = IoC.Resolve<LetterTypeRepository>();

                letterTypeRepository.UpdateLetterType(letterType);
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

        public LetterType GetLetterTypeById(int letterTypeId)
        {
            try
            {
                ILetterTypeRepository letterTypeRepository = IoC.Resolve<LetterTypeRepository>();

                return letterTypeRepository.Get(letterTypeId);
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

        public void DeleteLetterTypes(IList<int> ids, out IList<int> letterTypesCannotBeDeleted)
        {
            try
            {
                ILetterTypeRepository letterTypeRepository = IoC.Resolve<LetterTypeRepository>();

                IList<Transaction> transactions;

                letterTypesCannotBeDeleted = new List<int>();

                foreach (var id in ids)
                {
                    transactions = TransactionBL.GetTransactions(t => t.LetterType.Id == id);

                    if (transactions.Count > 0)
                    {
                        letterTypesCannotBeDeleted.Add(id);

                        continue;
                    }

                    letterTypeRepository.DeleteLetterType(id);
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

        public IList<LetterType> GetLetterTypes(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                ILetterTypeRepository letterTypeRepository = IoC.Resolve<LetterTypeRepository>();

                return letterTypeRepository.GetLetterTypes(searchCriteria, out rowsCount);
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

        public IList<LetterType> GetLetterTypes(TransactionCategories transactionCategories, string cultureName)
        {
            try
            {
                IList<LetterType> letterTypes = CacheHelper.Get(CachedObjectsKey.LetterTypes, cultureName) as IList<LetterType>;

                if (letterTypes == null)
                {
                    ILetterTypeRepository letterTypeRepository = IoC.Resolve<LetterTypeRepository>();

                    letterTypes = letterTypeRepository.GetLetterTypes(cultureName);

                    CacheHelper.Insert(CachedObjectsKey.LetterTypes, letterTypes, cultureName);
                }

                return letterTypes.Where(l => l.TransactionCategories.HasFlag(transactionCategories)).ToList();
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
