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

    public class ExternalPartyBL : BaseBL, IExternalPartyBL
    {
        public int AddExternalParty(ExternalParty externalParty)
        {
            try
            {
                IExternalPartyRepository partyRepository = IoC.Resolve<ExternalPartyRepository>();

                int externalPartyId = partyRepository.AddExternalParty(externalParty);

                return externalPartyId;
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

        //GetLastNumber
        public string GetLastNumber(int ParentId)
        {
            try
            {
                IExternalPartyRepository partyRepository = IoC.Resolve<ExternalPartyRepository>();
                string max = partyRepository.GetLastNumber(ParentId).ToString();
                int number = Convert.ToInt32(max);
                string parentidtnumber = ParentId.ToString(); 
                string parentIdString = ParentId.ToString();
                string prefix = "";
                
                if (max != null)
                {
                    if (parentIdString.Length < max.Length)
                    {
                        string parentNumber = max.Substring(0, parentIdString.Length);
                        if (parentNumber == ParentId.ToString())
                        {
                            string maxWithOutParentId = max.Substring(parentIdString.Length,( max.Length - parentIdString.Length));
                            number = Convert.ToInt32(max.Substring(parentIdString.Length, (max.Length - parentIdString.Length)));
                            prefix = max.Substring(parentIdString.Length  , (max.Length - parentIdString.Length)).Replace(number.ToString(), "");
                        }
                        else
                        {
                            string maxWithOutParentId = max.Substring(parentIdString.Length, (max.Length - parentIdString.Length));
                             number = Convert.ToInt32(max);
                            prefix = max.Substring(0, max.Length).Replace(number.ToString(), "");
                        }
                    }
                }
                number++;
                string stringNumber = number.ToString();
                if (stringNumber.Length <= 1 && prefix.Length < 1)
                {
                    stringNumber = "0";
                }
                else
                {
                    stringNumber = "";
                }
                string lastNumber = parentidtnumber + stringNumber + prefix + number;
                return lastNumber;
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
        public string GetLastNumberByCustomizeValue(string numberStartWithCustomizeValue)
        {
            try
            {
                IExternalPartyRepository partyRepository = IoC.Resolve<ExternalPartyRepository>();
                string max = partyRepository.GetLastNumberByCustomizeValue(numberStartWithCustomizeValue).ToString();
                int number = Convert.ToInt32(max);
                string parentidtnumber = numberStartWithCustomizeValue.ToString();
                string parentIdString = numberStartWithCustomizeValue.ToString();
                string prefix = "";

                if (max != null)
                {
                    if (parentIdString.Length < max.Length)
                    {
                        string parentNumber = max.Substring(0, parentIdString.Length);
                        if (parentNumber == numberStartWithCustomizeValue.ToString())
                        {
                            string maxWithOutParentId = max.Substring(parentIdString.Length, (max.Length - parentIdString.Length));
                            number = Convert.ToInt32(max.Substring(parentIdString.Length, (max.Length - parentIdString.Length)));
                            prefix = max.Substring(parentIdString.Length , (max.Length - parentIdString.Length)).Replace(number.ToString(), "");
                        }
                        else
                        {
                            string maxWithOutParentId = max.Substring(parentIdString.Length, (max.Length - parentIdString.Length));
                            number = Convert.ToInt32(max);
                            prefix = max.Substring(0, max.Length).Replace(number.ToString(), "");
                        }
                    }
                     
                }

                number++;
                string stringNumber = number.ToString();
                if (stringNumber.Length <= 1 && prefix.Length < 1)
                {
                    stringNumber = "0";
                }
                else
                {
                    stringNumber = "";
                }
                string lastNumber = parentidtnumber + stringNumber + prefix + number;
                return lastNumber;
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
        public void UpdateExternalParty(ExternalParty externalParty)
        {
            try
            {

                {
                    IExternalPartyRepository partyRepository = IoC.Resolve<IExternalPartyRepository>();
                    partyRepository.UpdateExternalParty(externalParty);
                    CacheHelper.RemoveBasedOnPrefix(CachedObjectsKey.ExternalParties);
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
        public void DeleteParties(IList<int> externalPartiesIds, out IList<int> partiesCannotBeDeleted)
        {
            try
            {
                IExternalPartyRepository externalPartyRepository = IoC.Resolve<ExternalPartyRepository>();
                partiesCannotBeDeleted = new List<int>();

                foreach (int externalPartyId in externalPartiesIds)
                {
                    IList<Transaction> transactions = TransactionBL.GetTransactions(t => t.ExternalPartyId == externalPartyId);
                    if (transactions.Count() > 0)
                    {
                        partiesCannotBeDeleted.Add(externalPartyId);
                        continue;
                    }

                    IList<ExternalParty> childs = externalPartyRepository.GetExternalPartiesByParentId(externalPartyId);
                    if (childs.Count > 0)
                    {
                        RemoveChilds(childs, externalPartyRepository, partiesCannotBeDeleted);
                    }
                    else
                    {
                        externalPartyRepository.Delete(externalPartyId);

                        CacheHelper.Remove(CachedObjectsKey.ExternalParties, "en");
                        CacheHelper.Remove(CachedObjectsKey.ExternalParties, "ar");
                    }
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
        public ExternalParty GetExternalPartyById(int externalPartyId)
        {
            try
            {
                IExternalPartyRepository partyRepository = IoC.Resolve<ExternalPartyRepository>();
                return partyRepository.GetExternalPartyById(externalPartyId);
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

        public ExternalParty GetExternalPartyInfoByNumber(string partyNumber)
        {
            try
            {
                IExternalPartyRepository partyRepository = IoC.Resolve<ExternalPartyRepository>();
                return partyRepository.GetExternalPartyInfoByNumber(partyNumber);
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

        
        public bool CheckPartyNumber(string Number, int partyId = -1)
        {
            try
            {
                IExternalPartyRepository partyRepository = IoC.Resolve<ExternalPartyRepository>();
                return partyRepository.CheckPartyNumber(Number, partyId);
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
        public IList<ExternalParty> GetExternalParties(int? parentId, string cultureName, bool getVirtual = false)
        {
            try
            {
                IList<ExternalParty> externalParties = null;

                if (parentId == null)
                {
                    string casheKey = getVirtual ? CachedObjectsKey.ExternalParties + "vir" : CachedObjectsKey.ExternalParties;
                    externalParties = CacheHelper.Get(casheKey, cultureName) as IList<ExternalParty>;

                }
                else
                {
                    string casheKey = getVirtual ? CachedObjectsKey.ExternalParties + parentId + "vir" : CachedObjectsKey.ExternalParties + parentId;
                    externalParties = CacheHelper.Get(casheKey, cultureName) as IList<ExternalParty>;
                }

               
                    IExternalPartyRepository partyRepository = IoC.Resolve<ExternalPartyRepository>();

                    externalParties = partyRepository.GetExternalParties(parentId, cultureName, getVirtual);

                    if (parentId == null)
                    {
                        string casheKey = getVirtual ? CachedObjectsKey.ExternalParties + "vir" : CachedObjectsKey.ExternalParties;
                        CacheHelper.Insert(casheKey, externalParties, cultureName);
                    }
                    else
                    {
                        string casheKey = getVirtual ? CachedObjectsKey.ExternalParties + parentId + "vir" : CachedObjectsKey.ExternalParties + parentId;
                        CacheHelper.Insert(casheKey, externalParties, cultureName);
                    }
               

                return externalParties;
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

        public IList<ExternalParty> GetAllExternalParties(int? parentId, string cultureName)
        {
            try
            {
                IList<ExternalParty> externalParties = null;

                if (parentId == null)
                {
                    externalParties = CacheHelper.Get(CachedObjectsKey.ExternalParties, cultureName) as IList<ExternalParty>;

                }
                else
                {
                    externalParties = CacheHelper.Get(CachedObjectsKey.ExternalParties + parentId, cultureName) as IList<ExternalParty>;
                }

                if (parentId == null)
                {
                    IExternalPartyRepository partyRepository = IoC.Resolve<ExternalPartyRepository>();

                    externalParties = partyRepository.GetAllExternalParties(parentId, cultureName);

                    if (parentId == null)
                    {
                        CacheHelper.Insert(CachedObjectsKey.ExternalParties, externalParties, cultureName);
                    }
                    else
                    {
                        CacheHelper.Insert(CachedObjectsKey.ExternalParties + parentId, externalParties, cultureName);
                    }
                }

                return externalParties;
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

        public IList<ExternalParty> GetExternalPartiesAutoComplete(string searchQuery, string cultureName, int resultSize)
        {
            try
            {
                IExternalPartyRepository partyRepository = IoC.Resolve<IExternalPartyRepository>();

                return partyRepository.GetExternalPartiesAutoComplete(searchQuery, cultureName, resultSize);

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
        public ExternalParty GetExternalPartiesByNumber(string number)
        {
            try
            {
                IExternalPartyRepository partyRepository = IoC.Resolve<IExternalPartyRepository>();

                return partyRepository.GetExternalPartiesByNumber(number);

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
        public IList<ExternalParty> GetExternalPartyNodes(int? nodeId, string cultureName)
        {
            try
            {
                IExternalPartyRepository partyRepository = IoC.Resolve<ExternalPartyRepository>();
                return partyRepository.GetExternalPartyNodes(nodeId, cultureName);
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
        public IList<ExternalParty> GetExternalPartiesByParentId(int? parentId, string cultureName)
        {
            try
            {
                IExternalPartyRepository partyRepository = IoC.Resolve<ExternalPartyRepository>();

                IList<ExternalParty> externalParties = partyRepository.GetExternalPartiesByParentId(parentId, cultureName);

                return externalParties;
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
        public IList<ExternalParty> GetExternalParties(SearchCriteria searchCriteria)
        {
            try
            {
                IExternalPartyRepository partyRepository = IoC.Resolve<ExternalPartyRepository>();

                IList<ExternalParty> externalParties = partyRepository.GetExternalParties(searchCriteria);

                return externalParties;
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
        public IList<ExternalParty> GetExternalPartiesByLetterType(int letterId, int? parentId, string cultureName)
        {
            try
            {
                IList<ExternalParty> externalParties = null;

                ILetterTypeBL letterTypeBL = new LetterTypeBL();

                LetterType letterType = letterTypeBL.GetLetterTypeById(letterId);

                IExternalPartyRepository partyRepository = IoC.Resolve<ExternalPartyRepository>();

                externalParties = partyRepository.GetExternalPartiesByLetterId(letterType.LetterListType, parentId, cultureName);

                CacheHelper.Insert(CachedObjectsKey.ExternalParties + letterId, externalParties, cultureName);

                return externalParties;

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
        public int AddExternalPartyManager(ExternalPartyManager externalPartyManager)
        {
            try
            {
                IExternalPartyRepository partyRepository = IoC.Resolve<ExternalPartyRepository>();

                int externalPartyManagerId = partyRepository.AddManager(externalPartyManager);

                return externalPartyManagerId;
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
        public void UpdateExternalPartyManager(ExternalPartyManager externalPartyManager)
        {
            try
            {
                {
                    IExternalPartyRepository partyRepository = IoC.Resolve<ExternalPartyRepository>();
                    partyRepository.UpdateManager(externalPartyManager);
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
        public void DeleteExternalPartyManagers(IList<int> managersIds, out IList<int> managersCannotBeDeleted)
        {
            try
            {
                managersCannotBeDeleted = new List<int>();
                IExternalPartyRepository externalPartyRepository = IoC.Resolve<ExternalPartyRepository>();
                foreach (int managerId in managersIds)
                {
                    IList<Transaction> transactions = TransactionBL.GetTransactions(t => t.ExternalPartyManagerId == managerId);
                    if (transactions.Count > 0)
                    {
                        managersCannotBeDeleted.Add(managerId);
                        continue;
                    }
                    externalPartyRepository.DeleteManager(managerId);
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
        public ExternalPartyManager GetExternalPartyManagerById(int externalPartyManagerId)
        {
            try
            {
                IExternalPartyRepository partyRepository = IoC.Resolve<ExternalPartyRepository>();

                return partyRepository.GetExternalPartyManagerById(externalPartyManagerId);
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
        public IList<ExternalPartyManager> GetExternalPartyManagers(int externalPartyId, SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IExternalPartyRepository partyRepository = IoC.Resolve<ExternalPartyRepository>();

                return partyRepository.GetExternalPartyManagers(externalPartyId, searchCriteria, out rowsCount);
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
        public IList<ExternalPartyManager> GetManagersByPartyId(int partyId, string cultureName)
        {
            try
            {
                IExternalPartyRepository partyRepository = IoC.Resolve<ExternalPartyRepository>();

                return partyRepository.GetAllExternalPartyManagers(partyId, cultureName);
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
        private void RemoveChilds(IList<ExternalParty> childs, IExternalPartyRepository externalPartyRepository, IList<int> partiesCannotBeDeleted)
        {
            foreach (ExternalParty child in childs)
            {
                IList<Transaction> transactions = TransactionBL.GetTransactions(t => t.ExternalPartyId == child.Id);

                if (transactions.Count() > 0)
                {
                    partiesCannotBeDeleted.Add(child.Id);
                    continue;
                }

                IList<ExternalParty> externalPartyChilds = externalPartyRepository.GetExternalPartiesByParentId(child.Id);
                if (externalPartyChilds.Count > 0)
                {
                    RemoveChilds(childs, externalPartyRepository, partiesCannotBeDeleted);
                }
                else
                {
                    externalPartyRepository.Delete(child.Id);
                }
            }
        }
    }
}

