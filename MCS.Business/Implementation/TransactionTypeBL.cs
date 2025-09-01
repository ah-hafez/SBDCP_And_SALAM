using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Framework.Persistence;
using MCS.Framework.Security;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class TransactionTypeBL : BaseBL, ITransactionTypeBL
    {
        public int AddTransactionSourceType(Domain.TransactionType transactionType)
        {
            try
            {
                ITransactionTypeRepository transactionSourceTypeRepository = IoC.Resolve<TransactionTypeRepository>();
                return transactionSourceTypeRepository.AddTransactionType(transactionType);
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

        public void UpdateTransactionSourceType(Domain.TransactionType transactionType)
        {
            try
            {
                ITransactionTypeRepository transactionSourceTypeRepository = IoC.Resolve<TransactionTypeRepository>();
                transactionSourceTypeRepository.UpdateTransactionType(transactionType);
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

        public TransactionType GetTransactionSourceTypeById(int transactionTypeId)
        {
            try
            {
                ITransactionTypeRepository transactionTypeRepository = IoC.Resolve<ITransactionTypeRepository>();
                return transactionTypeRepository.Get(transactionTypeId);
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

        public void DeleteTransactionSourceTypes(IList<int> ids, out IList<int> transactionTypesCannotBeDeleted)
        {
            try
            {
                ITransactionTypeRepository transactionTypeRepository = IoC.Resolve<ITransactionTypeRepository>();
                transactionTypesCannotBeDeleted = new List<int>();
                IList<Transaction> transactions;

                foreach (int id in ids)
                {
                    transactions = TransactionBL.GetTransactions(t => t.TransactionType.Id == id);

                    if (transactions.Count > 0)
                    {
                        transactionTypesCannotBeDeleted.Add(id);

                        continue;
                    }
                    transactionTypeRepository.DeleteTransactionType(id);
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

        public IList<Domain.TransactionType> GetTransactionSourceTypes(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                ITransactionTypeRepository transactionTypeRepository = IoC.Resolve<ITransactionTypeRepository>();
                return transactionTypeRepository.GetTransactionTypes(searchCriteria, out rowsCount);
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

        //private IList<UserClaim> GetTransactionSourceTypeClaims(TransactionCategories transactionCategories)
        //{

        //    string transactionsTypePrefix = string.Empty;

        //    switch (transactionCategories)
        //    {
        //        case TransactionCategories.Outbound:
        //            transactionsTypePrefix = UserClaims.OutboundTransactionsTypes.Prefix;
        //            break;
        //        case TransactionCategories.Inbound:
        //            transactionsTypePrefix = UserClaims.InboundTransactionsTypes.Prefix;
        //            break;
        //        case TransactionCategories.InternalOutbound:
        //            transactionsTypePrefix = UserClaims.InternalOutboundTransactionsTypes.Prefix;
        //            break;
        //    }

        //    return User.Claims.Where(c => c.Name.StartsWith(transactionsTypePrefix)).ToList();
        //}

        public IList<Domain.TransactionType> GetTransactionTypesByUserId(TransactionCategories transactionCategories, string cultureName)
        {
            try
            {
                ITransactionTypeRepository transactionTypeRepository = IoC.Resolve<ITransactionTypeRepository>();
                var transactionTypes =  transactionTypeRepository.GetTransactionTypesByUserId(User.Id, transactionCategories, cultureName);

                transactionTypes = transactionTypes.Where(t => User.HasClaim(t.Permission.Code)).ToList();

                return transactionTypes;
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

        public IList<Domain.TransactionType> GetTransactionSourceTypes(TransactionCategories transactionCategories, string cultureName)
        {
            try
            {
                IList<Domain.TransactionType> transactionTypes = CacheHelper.Get(CachedObjectsKey.TransactionTypes, cultureName) as IList<Domain.TransactionType>;

                if (transactionTypes == null)
                {
                    ITransactionTypeRepository transactionSourceTypeRepository =
                        IoC.Resolve<TransactionTypeRepository>();

                    transactionTypes = transactionSourceTypeRepository.GetTransactionTypes(cultureName);

                    CacheHelper.Insert(CachedObjectsKey.TransactionTypes, transactionTypes, cultureName);
                }

                return transactionTypes;
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
