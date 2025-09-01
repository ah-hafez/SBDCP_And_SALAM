using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MCS.Framework;
using MCS.Framework.ObjectExtensions;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Business
{
    public class OrgUnitBL : BaseBL, IOrgUnitBL
    {
        public IList<UserProfile> GetUsersByParentId(int OrgUnitId, string cultureName)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();
                return OrgUnitRepository.GetUsersByOrgUnitId(OrgUnitId, cultureName);
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

        public IList<int> GetOrgUnitsTransactions(IList<int> OrgUnitIds)
        {
            try
            {
                IList<int> OrgUnitsHasTransactions = new List<int>();

                foreach (int OrgUnitId in OrgUnitIds)
                {
                    IList<Transaction> transaction = TransactionBL.GetTransactions(t => t.OrgUnitId == OrgUnitId ||
                        t.EntityId == OrgUnitId);

                    if (transaction.Count > 0)
                    {
                        OrgUnitsHasTransactions.Add(OrgUnitId);
                    }
                }

                return OrgUnitsHasTransactions;
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
        public bool CheckOrgUnitUsedInTransaction(int orgUnitId, List<int> transactionCategoryIds)
        {
            try
            {
                IList<Transaction> transaction = TransactionBL.GetTransactions(t => t.OrgUnitId == orgUnitId &&
                transactionCategoryIds.Contains(t.TransactionCategoryId)).ToList();

                bool isExist = transaction != null && transaction.Count > 0;
                return isExist;
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
        public OrgUnit GetOrgUnitById(int OrgUnitId, string cultureName)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                return OrgUnitRepository.GetOrgUnitById(OrgUnitId, cultureName);
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
        public OrgUnit GetOrgUnitsGeneralCounter(string cultureName)
        {
            try
            {
                ICounterBL counterBL = new CounterBL();
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();
                int OrgUnitId = counterBL.GetGeneralCounter().OwnerEntityId;
                return OrgUnitRepository.Get(OrgUnitId);
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

        public OrgUnit GetOrgUnitById(int OrgUnitId)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                return OrgUnitRepository.Get(OrgUnitId);
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

        public OrgUnit GetOrgUnitByExternalId(int externalId)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                return OrgUnitRepository.FindBy(o => o.ExternalId == externalId);
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

        public bool CheckOrgUnitHasAssignmentPaper(int OrgUnitId)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                return OrgUnitRepository.CheckOrgUnitHasAssignmentPaper(OrgUnitId);
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

        public bool CheckOrgUnitIsAllowedToCreateGroup(int OrgUnitId)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                return OrgUnitRepository.CheckOrgUnitIsAllowedToCreateGroup(OrgUnitId);
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

        public IList<Domain.Action> GetOrgUnitActions(int OrgUnitId, string cultureName)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                return OrgUnitRepository.GetOrgUnitActions(OrgUnitId, cultureName);
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

        public IList<UserProfile> GetOrgUnitsManagers(string cultureName)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                return OrgUnitRepository.GetOrgUnitsManagers(cultureName);
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

        public UserProfile GetOrgUnitManager(int orgUnidId, string cultureName)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                return OrgUnitRepository.GetOrgUnitManager(orgUnidId, cultureName);
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

        public bool ValidateManagerCanAssign(int orgUnitId, int managerUserId, int transactionId, int transactionUserId, bool isManager)
        {
            try
            {

                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                return OrgUnitRepository.ValidateManagerCanAssign(orgUnitId, managerUserId, transactionId, transactionUserId, isManager);
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
        public IList<Domain.Action> GetOrgUnitActions(int OrgUnitId)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                return OrgUnitRepository.GetOrgUnitActions(OrgUnitId);
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

        public IList<AssignmentPaperBeneficiary> GetOrgUnitBeneficiaries(int OrgUnitId, string cultureName)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                return OrgUnitRepository.GetOrgUnitBeneficiaries(OrgUnitId, cultureName);
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

        public AssignmentPaper GetAssignmentPaperByOrgUnitId(int OrgUnitId, string cultureName)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                return OrgUnitRepository.GetAssignmentPaperByOrgUnitId(OrgUnitId, cultureName);
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

        public void UpdateAssignmentPaper(AssignmentPaper assignmentPaper)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                OrgUnitRepository.UpdateAssignmentPaper(assignmentPaper);
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

        public void BuildOrgUnitStructure(IList<OrgUnit> OrgUnits, string settingValue, out IList<int> OrgUnitUsedInTransactions)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();
                ICounterBL counterBL = new CounterBL();

                IList<int> OrgUnitsUsed = new List<int>();

                IList<OrgUnit> deletedOrgUnits = OrgUnits.Where(o => o.IsDeleted).ToList();

                deletedOrgUnits.ToList().ForEach(o =>
                {
                    IList<Transaction> transaction = TransactionBL.GetTransactions(t => t.OrgUnitId == o.Id ||
                        t.EntityId == o.Id);

                    if (transaction.Count > 0)
                    {
                        OrgUnitsUsed.Add(o.Id);
                    }
                });

                OrgUnitUsedInTransactions = OrgUnitsUsed;

                if (OrgUnitsUsed.Count == 0)
                {

                    {
                        OrgUnit rootOrgUnit = OrgUnits.Where(o => o.ParentId == null && o.IsActive && !o.IsDeleted).FirstOrDefault();

                        if (rootOrgUnit == null)
                        {
                            OrgUnits.ToList().ForEach(o =>
                            {
                                OrgUnit updatedCopy = o.ShallowCopy<OrgUnit>();

                                updatedCopy.AssignmentPaper = null;
                                updatedCopy.Links = null;
                                updatedCopy.Counter = null;

                                UpdateOrgUnit(updatedCopy);
                            });
                        }
                        else
                        {
                            OrgUnit updatedRootOrgUnit = rootOrgUnit.ShallowCopy<OrgUnit>();

                            updatedRootOrgUnit.AssignmentPaper = null;
                            updatedRootOrgUnit.Links = null;
                            updatedRootOrgUnit.Counter = null;

                            if (updatedRootOrgUnit.IsNew)
                            {
                                AddOrgUnit(updatedRootOrgUnit);
                            }
                            else
                            {
                                UpdateOrgUnit(updatedRootOrgUnit);
                            }

                            //TODO: Check this function
                            AddOrgUnitsChilds(OrgUnits, updatedRootOrgUnit);

                            //
                            //Now, we make sure that all the org units are added/updated to the database, so, we can manage them
                            //

                            OrgUnit existingOrgUnitRoot = GetOrgUnitById(rootOrgUnit.Id);

                            Counter generalCounter = counterBL.GetGeneralCounter();

                            if (generalCounter == null)
                            {
                                existingOrgUnitRoot.Counter = rootOrgUnit.Counter;
                            }
                            else
                            {
                                generalCounter.CounterDetails.ToList().ForEach(c =>
                                {
                                    generalCounter.CounterDetails.Remove(c);
                                });

                                rootOrgUnit.Counter.CounterDetails.ToList().ForEach(c =>
                                {
                                    IList<CounterDetail> counterDetails = counterBL.GetCounterDetailsByCounterId(generalCounter.Id).ToList();
                                    CounterDetail counterDetail = counterDetails.Where(cd => cd.TransactionCategories == c.TransactionCategories).FirstOrDefault();

                                    if (counterDetail != null)
                                    {
                                        counterDetail.InitialValue = c.InitialValue;
                                        generalCounter.CounterDetails.Add(counterDetail);
                                    }
                                    else
                                    {
                                        c.Counter = generalCounter;
                                        generalCounter.CounterDetails.Add(c);
                                    }
                                });

                                generalCounter.IsGeneral = rootOrgUnit.Counter.IsGeneral;
                                existingOrgUnitRoot.Counter = generalCounter;
                            }

                            UpdateOrgUnit(existingOrgUnitRoot);

                            OrgUnits.Where(o => !o.IsDeleted).ToList().ForEach(o =>
                            {
                                OrgUnit OrgUnit = GetOrgUnitById(o.Id);

                                //
                                //Manage org unit assignment papaer
                                //
                                OrgUnit = ManageOrgUnitAssignmentPaper(OrgUnit, o.AssignmentPaper);

                                //OrgUnit.Links = o.Links;
                                OrgUnit = ManageOrgUnitLinks(OrgUnit, o.Links);

                                //
                                //Manage org unit counter
                                //
                                OrgUnit = ManageOgrUnitCounter(existingOrgUnitRoot, OrgUnit, o);

                                if (o.ParentId != null)
                                {
                                    OrgUnit.Parent = OrgUnitRepository.Get(o.ParentId.Value);
                                }
                                else
                                {
                                    OrgUnit.Parent = null;
                                }

                                UpdateOrgUnit(OrgUnit);
                            });
                        }

                        ISettingBL settingBL = new SettingBL();

                        string settingKey = SettingsKeys.OrgUnitStructureKey;

                        List<Setting> settings = settingBL.GetSettingByKey(settingKey);
                        Setting setting = settings.Find(a => a.Key == settingKey);

                        if (setting != null)
                        {
                            setting.Value = settingValue;

                            settingBL.UpdateSetting(setting);
                        }


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

        public IList<OrgUnit> GetOrgUnitStructure()
        {
            try
            {
                IList<OrgUnit> OrgUnits;

                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                OrgUnits = OrgUnitRepository.GetOrgUnitStructure();

                foreach (OrgUnit OrgUnit in OrgUnits)
                {
                    if (OrgUnit.Counter != null)
                    {
                        foreach (CounterDetail counterDetail in OrgUnit.Counter.CounterDetails)
                        {
                            if (counterDetail.Count < counterDetail.InitialValue)
                            {
                                counterDetail.Count = 0;
                            }
                        }
                    }
                }
                return OrgUnits;
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

        public BarcodeDesign GetBarcodeDesignByOrgUnitId(int OrgUnitId, int typeId)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                return OrgUnitRepository.GetBarcodeDesignByOrgUnitId(OrgUnitId, typeId);
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

        public IList<OrgUnit> GetOrgUnits(string cultureName, int? OrgUnitId = null)
        {
            try
            {
                IList<OrgUnit> OrgUnits = CacheHelper.Get(CachedObjectsKey.OrgUnits, cultureName) as IList<OrgUnit>;

                if (OrgUnits == null)
                {
                    IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                    OrgUnits = OrgUnitRepository.GetOrgUnits(cultureName);

                    CacheHelper.Insert(CachedObjectsKey.OrgUnits, OrgUnits, cultureName);
                }

                return OrgUnits.Where(o => o.ParentId == OrgUnitId).ToList();
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
        public IList<OrgUnit> IAMGetOrgUnits(string cultureName)
        {
            try
            {
                IList<OrgUnit> OrgUnits = CacheHelper.Get(CachedObjectsKey.OrgUnits, cultureName) as IList<OrgUnit>;

                if (OrgUnits == null)
                {
                    IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                    OrgUnits = OrgUnitRepository.GetOrgUnits(cultureName);

                    CacheHelper.Insert(CachedObjectsKey.OrgUnits, OrgUnits, cultureName);
                }

                return OrgUnits.ToList();
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

        //This is the used version for the org unit hierarchy tree
        public IList<OrgUnit> GetOrgUnits(int? parentId, string cultureName, int LoggedInOrgUnitId, int? UserId = null, OrgUnitTreeMode? orgUnitTreeMode = OrgUnitTreeMode.User)
        {
            try
            {

                IList<OrgUnit> OrgUnits = null;

                string CacheKey = CachedObjectsKey.OrgUnits;

                CacheKey = CacheKey + "_ParentId_" + (parentId.HasValue ? parentId.Value.ToString() : "-1");

                if (UserId.HasValue)
                {
                    CacheKey = CacheKey + "_UserId_" + UserId.Value;
                }

                OrgUnits = CacheHelper.Get(CacheKey, cultureName) as IList<OrgUnit>;

                if (OrgUnits == null)
                {
                    IOrgUnitRepository OrgUnitRepository = IoC.Resolve<IOrgUnitRepository>();

                    OrgUnits = OrgUnitRepository.GetOrgUnits(parentId, cultureName, UserId);

                    if (parentId == null)
                    {
                        CacheHelper.Insert(CacheKey, OrgUnits, cultureName);
                    }
                    else
                    {
                        CacheHelper.Insert(CacheKey, OrgUnits, cultureName);
                    }
                }
                if (User != null)
                {
                    if (User.HasClaim(UserClaims.ModulesLevel.AllModules) || orgUnitTreeMode != OrgUnitTreeMode.Search)
                    {
                        return OrgUnits;
                    }

                }
                else
                {
                    return OrgUnits;
                }


                IList<OrgUnit> allFiltersUnits = new List<OrgUnit>();

                OrgUnit LoggedInOrgUnit = GetOrgUnit(LoggedInOrgUnitId, cultureName);
                if (User.HasClaim(UserClaims.ModulesLevel.ParentDepartment) && parentId != LoggedInOrgUnit.ParentId && LoggedInOrgUnit.Parent != null)
                {
                    allFiltersUnits.Add(LoggedInOrgUnit.Parent);
                    return allFiltersUnits;
                }

                if (parentId == LoggedInOrgUnit.ParentId)
                {
                    allFiltersUnits.Add(LoggedInOrgUnit);
                    return allFiltersUnits;
                }

                if (User.HasClaim(UserClaims.ModulesLevel.AllChildsModules) && LoggedInOrgUnit.Id == parentId)
                {
                    return OrgUnits;
                }

                return allFiltersUnits;
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

        public IList<OrgUnit> GetOrgUnitsAutoComplete(string searchQuery, string cultureName, int resultSize, int orgUnitId)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<IOrgUnitRepository>();
                bool isAllModules = User.HasClaim(UserClaims.ModulesLevel.AllModules);
                bool isChildModules = User.HasClaim(UserClaims.ModulesLevel.AllChildsModules);
                bool isParentModule = User.HasClaim(UserClaims.ModulesLevel.ParentDepartment);

                return OrgUnitRepository.GetOrgUnitsAutoComplete(searchQuery, cultureName, resultSize, orgUnitId, isAllModules, isParentModule, isChildModules);

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
        public OrgUnit GetOrgUnit(int orgUnitId, string cultureName)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<IOrgUnitRepository>();

                return OrgUnitRepository.GetOrgUnit(orgUnitId, cultureName);

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
        public OrgUnit GetParentOrgUnit(int orgUnitId, string cultureName)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<IOrgUnitRepository>();

                return OrgUnitRepository.GetParentOrgUnit(orgUnitId, cultureName);

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
        public OrgUnit GetInternalPartyInfoByNumber(string partyNumber, string cultureName)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<IOrgUnitRepository>();

                return OrgUnitRepository.GetInternalPartyInfoByNumber(partyNumber, cultureName);

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

        public List<OrgUnit> GetOrgUnits(List<int> orgUnitIds, string cultureName)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<IOrgUnitRepository>();

                return OrgUnitRepository.GetOrgUnits(orgUnitIds, cultureName);

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

        public string GetOrgUnitName(Expression<Func<OrgUnit, bool>> @where, string cultureName)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                return OrgUnitRepository.GetOrgUnitName(@where, cultureName);
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
        public OrgUnit ManageOrgUnitAssignmentPaper(OrgUnit OrgUnit, AssignmentPaper assignmentPaper)
        {
            try
            {
                if (OrgUnit.AssignmentPaper == null)
                {
                    if (assignmentPaper != null)
                    {
                        assignmentPaper.AssignmentPaperBeneficiaries.ToList().ForEach(b =>
                        {
                            b.OrgUnit = GetOrgUnitById(b.OrgUnitId);
                        });

                        OrgUnit.AssignmentPaper = assignmentPaper;
                    }
                }
                else
                {
                    IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                    AssignmentPaper existingAssignmentPaper = OrgUnitRepository.GetAssignmentPaperById(OrgUnit.AssignmentPaper.Id);

                    existingAssignmentPaper.IsCreateGroupAllowed = assignmentPaper.IsCreateGroupAllowed;

                    existingAssignmentPaper.AssignmentPaperActions.ToList().ForEach(a =>
                    {
                        existingAssignmentPaper.AssignmentPaperActions.Remove(a);
                    });

                    existingAssignmentPaper.AssignmentPaperBeneficiaries.ToList().ForEach(b =>
                    {
                        existingAssignmentPaper.AssignmentPaperBeneficiaries.Remove(b);
                    });

                    if (assignmentPaper != null)
                    {
                        assignmentPaper.AssignmentPaperBeneficiaries.ToList().ForEach(b =>
                        {
                            AssignmentPaperBeneficiary assignmentPaperBeneficiary = OrgUnitRepository.GetAssignmentPaperBeneficiaryById(b.Id);

                            if (assignmentPaperBeneficiary != null)
                            {
                                existingAssignmentPaper.AssignmentPaperBeneficiaries.Add(assignmentPaperBeneficiary);
                            }
                            else
                            {
                                b.OrgUnit = GetOrgUnitById(b.OrgUnitId);
                                existingAssignmentPaper.AssignmentPaperBeneficiaries.Add(b);
                            }
                        });

                        assignmentPaper.AssignmentPaperActions.ToList().ForEach(a =>
                        {
                            AssignmentPaperAction assignmentPaperAction = OrgUnitRepository.GetAssignmentPaperActionById(a.Id);

                            if (assignmentPaperAction != null)
                            {
                                existingAssignmentPaper.AssignmentPaperActions.Add(assignmentPaperAction);
                            }
                            else
                            {
                                existingAssignmentPaper.AssignmentPaperActions.Add(a);
                            }
                        });
                    }

                    OrgUnit.AssignmentPaper = existingAssignmentPaper;
                }

                return OrgUnit;
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

        public IList<OrgUnit> GetOrgUnitLinks(int OrgUnitId, string cultureName)
        {
            try
            {
                // OrgUnit OrgUnit = GetOrgUnitById(OrgUnitId);
                IList<OrgUnit> OrgUnits = GetOrgUnits(cultureName, null);
                return OrgUnits;
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
        public IList<Transaction> GetYearTransactionsCount(int year, int OrgUnit, bool isGeneralCounter)
        {
            try
            {
                List<Transaction> transactions = new List<Transaction>();

                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                if (isGeneralCounter)
                {
                    transactions.Add(OrgUnitRepository.GetYearTransactionsCount(t => t.YearH == year && t.TransactionCategoryId == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)));
                    transactions.Add(OrgUnitRepository.GetYearTransactionsCount(t => t.YearH == year && t.TransactionCategoryId == TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)));
                    transactions.Add(OrgUnitRepository.GetYearTransactionsCount(t => t.YearH == year && t.TransactionCategoryId == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)));
                }
                else
                {
                    transactions.Add(OrgUnitRepository.GetYearTransactionsCount(t => t.OrgUnitId == OrgUnit && t.YearH == year && t.TransactionCategoryId == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)));
                    transactions.Add(OrgUnitRepository.GetYearTransactionsCount(t => t.OrgUnitId == OrgUnit && t.YearH == year && t.TransactionCategoryId == TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)));
                    transactions.Add(OrgUnitRepository.GetYearTransactionsCount(t => t.OrgUnitId == OrgUnit && t.YearH == year && t.TransactionCategoryId == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)));
                }
                return transactions;
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
        public DateTime GetFirstTransactionDate()
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                return OrgUnitRepository.GetFirstTransactionDate();
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
        private int AddOrgUnit(OrgUnit OrgUnit)
        {
            ISettingBL settingBL = new SettingBL();

            List<Setting> settings = settingBL.GetSettingByKey(SettingsKeys.TenantOrgUnitsCount);
            Setting setting = settings.Find(a => a.Key == SettingsKeys.TenantOrgUnitsCount);

            if (!string.IsNullOrEmpty(setting.Value))
            {
                int OrgUnitsCount = GetAllOrgUnitsCount();

                if (OrgUnitsCount > Convert.ToInt32(setting.Value))
                {
                    throw new BusinessException(StatusCode.MaxOrgUnitsReached);
                }
            }

            IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

            int OrgUnitId = OrgUnitRepository.AddOrgUnit(OrgUnit);

            return OrgUnitId;
        }

        private void UpdateOrgUnit(OrgUnit OrgUnit)
        {
            IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

            OrgUnitRepository.UpdateOrgUnit(OrgUnit);
        }

        private void AddOrgUnitsChilds(IList<OrgUnit> OrgUnits, OrgUnit parent)
        {
            foreach (OrgUnit OrgUnit in OrgUnits)
            {
                OrgUnit updatedOrgUnit = OrgUnit.ShallowCopy<OrgUnit>();

                if (updatedOrgUnit.ParentId != null)
                {
                    if (updatedOrgUnit.ParentId == parent.Id)
                    {
                        updatedOrgUnit.Parent = GetOrgUnitById(parent.Id);

                        updatedOrgUnit.AssignmentPaper = null;
                        updatedOrgUnit.Links = null;
                        updatedOrgUnit.Counter = null;

                        if (updatedOrgUnit.IsNew)
                        {
                            AddOrgUnit(updatedOrgUnit);
                        }
                        else
                        {
                            UpdateOrgUnit(updatedOrgUnit);
                        }

                        AddOrgUnitsChilds(OrgUnits, updatedOrgUnit);
                    }
                }
            }
        }
        private OrgUnit ManageOrgUnitLinks(OrgUnit OrgUnit, IList<OrgUnitLink> links)
        {
            if (OrgUnit.Links == null)
            {
                OrgUnit.Links = new List<OrgUnitLink>();
            }

            IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

            IList<OrgUnitLink> OrgUnitLinks = new List<OrgUnitLink>();

            OrgUnitRepository.DeleteOrgUnitLinks(OrgUnit.Id);

            links.ToList().ForEach(l =>
            {
                OrgUnitLink OrgUnitLink = OrgUnitRepository.GetOrgUnitLink(l.FromEntity.Id, l.ToEntity.Id);

                if (OrgUnitLink != null)
                {
                    OrgUnitLinks.Add(OrgUnitLink);
                }
                else
                {
                    l.ToEntity = GetOrgUnitById(l.ToEntity.Id);
                    l.FromEntity = GetOrgUnitById(l.FromEntity.Id);

                    OrgUnitLinks.Add(l);
                }
            });

            OrgUnit.Links = OrgUnitLinks;

            return OrgUnit;
        }

        private OrgUnit ManageOgrUnitCounter(OrgUnit rootOrgUnit, OrgUnit existingOrgUnit, OrgUnit mappedOrgUnit)
        {
            existingOrgUnit.JoinToGeneralCounter = mappedOrgUnit.JoinToGeneralCounter;

            if (mappedOrgUnit.Id != rootOrgUnit.Id)
            {
                ICounterBL counterBL = new CounterBL();

                if (mappedOrgUnit.JoinToGeneralCounter)
                {
                    existingOrgUnit.Counter = null;
                    Counter counter = counterBL.GetCounterById(rootOrgUnit.Counter.Id);
                    existingOrgUnit.Counter = counter;
                }
                else
                {
                    if (existingOrgUnit.Counter == null)
                    {
                        foreach (CounterDetail counterDetail in mappedOrgUnit.Counter.CounterDetails)
                        {
                            counterDetail.Count = counterDetail.Count - 1;
                        }

                        existingOrgUnit.Counter = mappedOrgUnit.Counter;
                    }
                    else if (existingOrgUnit.Counter.Id == rootOrgUnit.Counter.Id)
                    {
                        existingOrgUnit.Counter = null;
                        existingOrgUnit.Counter = mappedOrgUnit.Counter;
                    }
                    else
                    {
                        Counter counter = counterBL.GetCounterById(existingOrgUnit.Counter.Id);

                        counter.CounterDetails.ToList().ForEach(c =>
                        {
                            counter.CounterDetails.Remove(c);
                        });

                        mappedOrgUnit.Counter.CounterDetails.ToList().ForEach(c =>
                        {
                            CounterDetail rootCounterDetail = counterBL.GetCounterDetailById(c.Id);

                            if (rootCounterDetail != null)
                            {
                                rootCounterDetail.InitialValue = c.InitialValue;
                                counter.CounterDetails.Add(rootCounterDetail);
                            }
                            else
                            {
                                c.Counter = counter;
                                counter.CounterDetails.Add(c);
                            }
                        });

                        counter.IsGeneral = mappedOrgUnit.Counter.IsGeneral;
                        existingOrgUnit.Counter = counter;
                    }
                }
            }

            return existingOrgUnit;
        }

        private void AddLinksNodes(IList<OrgUnit> OrgUnits, OrgUnit OrgUnit)
        {
            if (OrgUnits != null && OrgUnit != null)
            {
                foreach (OrgUnitLink OrgUnitLink in OrgUnit.Links)
                {
                    if (!OrgUnitLink.ToEntity.IsActive || OrgUnitLink.ToEntity.IsDeleted)
                    {
                        continue;
                    }

                    if (!OrgUnits.Contains(OrgUnitLink.ToEntity))
                    {
                        if (!OrgUnitLink.ToEntity.IsVirtualUnit || OrgUnitLink.ToEntity.Parent == null)
                        {
                            OrgUnits.Add(OrgUnitLink.ToEntity);
                        }

                        this.AddParentNode(OrgUnits, OrgUnitLink.ToEntity);
                    }
                }
            }
        }

        private void AddParentNode(IList<OrgUnit> OrgUnits, OrgUnit OrgUnit)
        {
            if (!OrgUnit.IsActive)
            {
                return;
            }

            if (!OrgUnits.Contains(OrgUnit))
            {
                if (!OrgUnit.IsVirtualUnit || OrgUnit.Parent == null)
                {
                    OrgUnits.Add(OrgUnit);
                }
            }

            if (OrgUnit.Parent != null)
            {
                this.AddParentNode(OrgUnits, OrgUnit.Parent);
            }
        }

        public int GetAllOrgUnitsCount()
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                return OrgUnitRepository.GetAllOrgUnitsCount();
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

        public void UpdateOrgUnitWithUsers(OrgUnit orgUnit, string cultureName)
        {
            IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

            OrgUnitRepository.UpdateOrgUnitWithUsers(orgUnit, cultureName);
        }

        public void UpdateOrgUnitWithCounter(OrgUnit orgUnit, string cultureName)
        {
            IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();
            OrgUnitRepository.UpdateOrgUnitWithCounter(orgUnit, cultureName);
        }
        public void UpdateOrgUnitToJoinGeneralCounter(int orgUnitId)
        {
            IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();
            OrgUnitRepository.UpdateOrgUnitToJoinGeneralCounter(orgUnitId);
        }
        public int UpdateOrgUnitInfo(OrgUnit orgUnit)
        {
            IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();
            int Id = OrgUnitRepository.UpdateOrgUnitInfo(orgUnit);
            CacheHelper.RemoveBasedOnPrefix(CachedObjectsKey.OrgUnits);
            return Id;
        }

        public void UpdateOrgUnitWithLink(OrgUnit orgUnit, string cultureName)
        {
            IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();
            OrgUnitRepository.UpdateOrgUnitWithLink(orgUnit, cultureName);
        }

        public void UpdateOrgUnitWithBarcodeDesign(OrgUnit orgUnit, string cultureName)
        {
            IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();
            OrgUnitRepository.UpdateOrgUnitWithBarcodeDesign(orgUnit, cultureName);
        }
        public IList<OrgUnit> GetOrgUnitsLight(string cultureName)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();
                var OrgUnits = OrgUnitRepository.GetOrgUnitsLight(cultureName);
                return OrgUnits;
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
        public IList<OrgUnit> GetOrgUnitsNew(string cultureName)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();
                var OrgUnits = OrgUnitRepository.GetOrgUnitsNew(cultureName);
                return OrgUnits;
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

        public List<int> GetAllOrgUnitsId(string cultureName)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();
                var OrgUnits = OrgUnitRepository.GetAllOrgUnitsId(cultureName);
                return OrgUnits;
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
        public IList<OrgUnit> GetOrgUnitsWithCounter(string cultureName)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();
                var OrgUnits = OrgUnitRepository.GetOrgUnitsWithCounter(cultureName);
                return OrgUnits;
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
        public IList<OrgUnit> GetOrgUnitsWithUser(string cultureName)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();
                var OrgUnits = OrgUnitRepository.GetOrgUnitsWithUser(cultureName);
                return OrgUnits;
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
        public IList<OrgUnit> GetOrgUnitsWithLinks(string cultureName)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();
                var OrgUnits = OrgUnitRepository.GetOrgUnitsWithLinks(cultureName);
                return OrgUnits;
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
        public bool DeleteOrgUnit(int orgUnitKey)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();
                var OrgUnits = OrgUnitRepository.DeleteOrgUnit(orgUnitKey);

                CacheHelper.RemoveBasedOnPrefix(CachedObjectsKey.OrgUnits);

                return OrgUnits;
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
        public IList<OrgUnit> GetOrgUnitStructureRoot(int? parentId)
        {
            try
            {
                IList<OrgUnit> OrgUnits;

                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                OrgUnits = OrgUnitRepository.GetOrgUnitStructureRoot(parentId);

                foreach (OrgUnit OrgUnit in OrgUnits)
                {
                    if (OrgUnit.Counter != null)
                    {
                        foreach (CounterDetail counterDetail in OrgUnit.Counter.CounterDetails)
                        {
                            if (counterDetail != null)
                            {
                                if (counterDetail.Count < counterDetail.InitialValue)
                                {
                                    counterDetail.Count = 0;
                                }
                            }
                        }
                    }
                }
                return OrgUnits;
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

        public IList<OrgUnit> GetAllUnitByLineage(string lineage, string cultureName)
        {
            try
            {
                string[] UnitIds = lineage.Split('/');

                IList<OrgUnit> OrgUnits = new List<OrgUnit>();

                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                for (int i = 0; i < UnitIds.Length - 1; i++)
                {
                    OrgUnit orgUnit = new OrgUnit();
                    orgUnit = GetOrgUnitById(Convert.ToInt32(UnitIds[i]), cultureName);
                    OrgUnits.Add(orgUnit);
                }
                return OrgUnits;
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

        public void AdminMoveUser(int userID, int orgunitID, int newOrgunitID, int loggedinUserID)
        {
            IAdminOperationsWrapper wrapper = IoC.Resolve<AdminOperationsWrapper>();
            wrapper.AdminMoveUser(userID, orgunitID, newOrgunitID, loggedinUserID);
            CacheHelper.RemoveBasedOnPrefix(CachedObjectsKey.OrgUnitStructure);
        }
        public void AdminMoveUser(string usersIDs, int orgunitID, int newOrgunitID, int loggedinUserID, bool isExternal = false)
        {
            IAdminOperationsWrapper wrapper = IoC.Resolve<AdminOperationsWrapper>();
            wrapper.AdminMoveUser(usersIDs, orgunitID, newOrgunitID, loggedinUserID, isExternal);
            CacheHelper.RemoveBasedOnPrefix(CachedObjectsKey.OrgUnitStructure);
        }
        public void AdminDeleteUserERP(int userId, int externalOrgunitID, int loggedinUserID)
        {
            IAdminOperationsWrapper wrapper = IoC.Resolve<AdminOperationsWrapper>();
            wrapper.AdminDeleteUserERP(userId, externalOrgunitID, loggedinUserID);
            CacheHelper.RemoveBasedOnPrefix(CachedObjectsKey.OrgUnitStructure);
        }

        public int MoveEntity(int entityFrom, int entityTo, int loginUser, bool noExternal = false)
        {
            try
            {
                IAdminOperationsWrapper wrapper = IoC.Resolve<AdminOperationsWrapper>();
                if (!SystemConfigurations.ERPIntegrationEnabled)
                    noExternal = false;

                int ConflictedEntityId = wrapper.AdminMoveEntity(entityFrom, entityTo, loginUser, noExternal);
                CacheHelper.RemoveBasedOnPrefix(CachedObjectsKey.OrgUnitStructure);
                CacheHelper.RemoveBasedOnPrefix(CachedObjectsKey.OrgUnits);
                return ConflictedEntityId;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException DAEx)
            {
                throw new BusinessException(DAEx.Message);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public int MergeDepartments(MergeDepartment mergeDepartment, bool noExternal = false)
        {
            try
            {
                int x = User.Id;
                IAdminOperationsWrapper wrapper = IoC.Resolve<AdminOperationsWrapper>();
                if (!SystemConfigurations.ERPIntegrationEnabled)
                    noExternal = false;

                int ConflictedEntityId = wrapper.MergeDepartments(mergeDepartment, noExternal);
                CacheHelper.RemoveBasedOnPrefix(CachedObjectsKey.OrgUnitStructure);
                CacheHelper.RemoveBasedOnPrefix(CachedObjectsKey.OrgUnits);
                return ConflictedEntityId;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException DAEx)
            {
                throw new BusinessException(DAEx.Message);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }


        public void AdminMoveTransactions(int entityFromId, int entityToId, int userFromId, int userToId, int logInUser)
        {
            try
            {
                IAdminOperationsWrapper wrapper = IoC.Resolve<AdminOperationsWrapper>();
                wrapper.AdminMoveTransactions(entityFromId, entityToId, userFromId, userToId, logInUser);
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

        public void AdminMoveTransactionById(int transId, int toUserId, int toEntityId, int loggedInUser)
        {
            IAdminOperationsWrapper wrapper = IoC.Resolve<AdminOperationsWrapper>();
            Transaction transaction = TransactionBL.GetTransactionById(transId);
            if (transaction.Assignments.FirstOrDefault().ToUserId == toUserId & transaction.Assignments.FirstOrDefault().ToEntityId == toEntityId)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            else
            {
                wrapper.AdminMoveTransactionById(transId, toUserId, toEntityId, loggedInUser);
            }

        }

        public bool CheckOrgUnitIsExternal(int entityId)
        {
            IOrgUnitRepository orgUnitRepository = IoC.Resolve<OrgUnitRepository>();
            OrgUnit orgUnit = orgUnitRepository.GetOrgUnitById(entityId);

            return orgUnit.ExternalId.HasValue;
        }

        public bool CheckOrgUnitNumber(string Number, int OrgUnitKey)
        {
            try
            {
                IOrgUnitRepository orgUnitRepository = IoC.Resolve<IOrgUnitRepository>();
                return orgUnitRepository.CheckOrgUnitNumber(Number, OrgUnitKey);
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

        public int? getIoDepartment(int orgunitID)
        {
            try
            {
                IOrgUnitRepository orgUnitRepository = IoC.Resolve<IOrgUnitRepository>();
                return orgUnitRepository.getIoDepartment(orgunitID);
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
        public int? getGeneralIoDepartment()
        {
            try
            {
                IOrgUnitRepository orgUnitRepository = IoC.Resolve<IOrgUnitRepository>();
                return orgUnitRepository.getGeneralIoDepartment();
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
        public int? getFollowUpDepartment(int orgunitID)
        {
            try
            {
                IOrgUnitRepository orgUnitRepository = IoC.Resolve<IOrgUnitRepository>();
                return orgUnitRepository.getFollowUpDepartment(orgunitID);
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
        public bool ReceiveElcOutBoundWithAcknowled(int orgunitID)
        {
            try
            {
                IOrgUnitRepository orgUnitRepository = IoC.Resolve<IOrgUnitRepository>();
                return orgUnitRepository.ReceiveElcOutBoundWithAcknowled(orgunitID);
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
        public bool CheckIfOrgunitSendSpecialCopy(int orgunitID)
        {
            try
            {
                IOrgUnitRepository orgUnitRepository = IoC.Resolve<IOrgUnitRepository>();
                return orgUnitRepository.CheckIfOrgunitSendSpecialCopy(orgunitID);
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
        public IList<UserProfile> GetAllUsers()
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                return OrgUnitRepository.GetAllUsers();
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
        public string GetOrgUnitSymbol(int OrgUnitId)
        {
            try
            {
                IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();

                return OrgUnitRepository.GetOrgUnitSymbol(OrgUnitId);
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


        public void UpdateOrgFromService(IList<OrgunitSap> orgunitSapDtos)
        {
            IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();
            OrgUnitRepository.UpdateOrgFromService(orgunitSapDtos);
            CacheHelper.RemoveBasedOnPrefix(CachedObjectsKey.OrgUnits);

        }
    }
}
