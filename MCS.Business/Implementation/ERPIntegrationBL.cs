using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MCS.Framework;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;
using System.Data;

namespace MCS.Business
{
    public class ERPIntegrationBL : BaseBL, IERPIntegrationBL
    {
        public void AddUserSync()
        {
            try
            {
                if (SystemConfigurations.ERPIntegrationEnabled)
                {
                    IERPIntegrationWrapper integrationWrapper = IoC.Resolve<ERPIntegrationWrapper>();
                    IOrgUnitBL OrgUnitBL = IoC.Resolve<IOrgUnitBL>();
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    DataSet addUserDS = integrationWrapper.AddUserSync(out int totalCount);

                    if (addUserDS != null && addUserDS.Tables.Count > 0)
                    {
                        //if (addUserDS.Tables[0] != null && addUserDS.Tables[0].Rows.Count > 0)
                        //{
                        //    if (IsUpdated(Convert.ToDateTime(addUserDS.Tables[0].Rows[0]["TimeStamp"].ToString()), ERPSettingsKeys.ERPAddUsersTimeStamp))
                        //        return;
                        //}

                        foreach (DataRow item in addUserDS.Tables[0].Rows)
                        {
                            AddUser(item);
                        }
                        if (addUserDS.Tables[0].Rows.Count > 0)
                            UpdateTimestamp(ERPSettingsKeys.ERPAddUsersTimeStamp);
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

        public void DeleteUserSync()
        {
            try
            {
                if (SystemConfigurations.ERPIntegrationEnabled)
                {
                    IERPIntegrationWrapper integrationWrapper = IoC.Resolve<ERPIntegrationWrapper>();
                    IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                    DataSet deleteUserDS = integrationWrapper.DeleteUserSync(out int totalCount);

                    if (deleteUserDS != null && deleteUserDS.Tables.Count > 0)
                    {
                        //if (deleteUserDS.Tables[0] != null && deleteUserDS.Tables[0].Rows.Count > 0)
                        //{
                        //    if (IsUpdated(Convert.ToDateTime(deleteUserDS.Tables[0].Rows[0]["TimeStamp"].ToString()), ERPSettingsKeys.ERPDeleteUsersTimeStamp))
                        //        return;
                        //}

                        foreach (DataRow item in deleteUserDS.Tables[0].Rows)
                        {
                            int externalId = Convert.ToInt32(item["UserExternalId"].ToString());
                            UserProfile user = userManagementRepository.FindBy(u => u.ExternalId == externalId);
                            if (user != null)
                            {
                                orgUnitBL.AdminDeleteUserERP(user.Id, Convert.ToInt32(item["EntityId"].ToString()), User.Id);
                            }
                        }
                        if (deleteUserDS.Tables[0].Rows.Count > 0)
                            UpdateTimestamp(ERPSettingsKeys.ERPDeleteUsersTimeStamp);
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

        public void MoveUserSync()
        {
            try
            {
                if (SystemConfigurations.ERPIntegrationEnabled)
                {
                    IERPIntegrationWrapper integrationWrapper = IoC.Resolve<ERPIntegrationWrapper>();
                    IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                    DataSet moveUserDS = integrationWrapper.MoveUserSync(out int totalCount);

                    if (moveUserDS != null && moveUserDS.Tables.Count > 0)
                    {
                        //if (moveUserDS.Tables[0] != null && moveUserDS.Tables[0].Rows.Count > 0)
                        //{
                        //    if (IsUpdated(Convert.ToDateTime(moveUserDS.Tables[0].Rows[0]["TimeStamp"].ToString()), ERPSettingsKeys.ERPMoveUsersTimeStamp))
                        //        return;
                        //}

                        foreach (DataRow item in moveUserDS.Tables[0].Rows)
                        {
                            int externalId = Convert.ToInt32(item["UserExternalId"].ToString());
                            UserProfile user = userManagementRepository.FindBy(u => u.ExternalId == externalId);
                            if (user != null)
                            {
                                orgUnitBL.AdminMoveUser(user.Id.ToString(), Convert.ToInt32(item["FromEntityId"].ToString()),
                                                            Convert.ToInt32(item["ToEntityId"].ToString()), User.Id, true);
                            }
                        }
                        if (moveUserDS.Tables[0].Rows.Count > 0)
                            UpdateTimestamp(ERPSettingsKeys.ERPMoveUsersTimeStamp);
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

        public void DelegationUserSync()
        {
            try
            {
                if (SystemConfigurations.ERPIntegrationEnabled)
                {
                    IERPIntegrationWrapper integrationWrapper = IoC.Resolve<ERPIntegrationWrapper>();
                    IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                    DataSet delegationUserDS = integrationWrapper.DelegationUserSync(out int totalCount);

                    if (delegationUserDS != null && delegationUserDS.Tables.Count > 0)
                    {
                        //if (delegationUserDS.Tables[0] != null && delegationUserDS.Tables[0].Rows.Count > 0)
                        //{
                        //    if (IsUpdated(Convert.ToDateTime(delegationUserDS.Tables[0].Rows[0]["TimeStamp"].ToString()), ERPSettingsKeys.ERPDelegationUsersTimeStamp))
                        //        return;
                        //}

                        foreach (DataRow item in delegationUserDS.Tables[0].Rows)
                        {
                            int externalId = Convert.ToInt32(item["SourceUserId"].ToString());
                            int toUserExternalId = Convert.ToInt32(item["ToUserId"].ToString());
                            UserProfile user = userManagementRepository.FindBy(u => u.ExternalId == externalId);
                            if (user != null)
                            {
                                UserProfile userTo = userManagementRepository.FindBy(u => u.ExternalId == toUserExternalId);
                                OrgUnit orgUnitTo = orgUnitBL.GetOrgUnitByExternalId(Convert.ToInt32(item["ToEntityId"].ToString()));

                                List<UserDelegation> userDelegations = new List<UserDelegation>()
                            {
                                new UserDelegation()
                                {
                                    Id = 0,
                                    FromDate = Convert.ToDateTime(item["FromDate"].ToString()),
                                    ToDate = Convert.ToDateTime(item["ToDate"].ToString()),
                                    FromDateH = DateTimeUtility.ConvertToUmAlQuraCalendar(Convert.ToDateTime(item["FromDate"].ToString())),
                                    ToDateH = DateTimeUtility.ConvertToUmAlQuraCalendar(Convert.ToDateTime(item["ToDate"].ToString())),
                                    UserProfileId = userTo.Id,
                                    OrgUnitId = orgUnitTo.Id,
                                    ConfidentialityId = null,
                                    PriorityId = null,
                                    TransactionTypeId = null,
                                    StatusId = DelegationStatus.InProcess.LookupIdentity(LookupCategory.DelegationStatus, string.Empty),
                                    RejectionReason = null,
                                    ReceiveCopy= false,
                                    ShowTransaction= false
                                }
                            };
                                userManagementBL.UpdateUserDelegations(user.Id, userDelegations, "ar");
                            }
                        }
                        if (delegationUserDS.Tables[0].Rows.Count > 0)
                            UpdateTimestamp(ERPSettingsKeys.ERPDelegationUsersTimeStamp);
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

        public void AddEntitySync()
        {
            try
            {
                if (SystemConfigurations.ERPIntegrationEnabled)
                {
                    IERPIntegrationWrapper integrationWrapper = IoC.Resolve<ERPIntegrationWrapper>();
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                    DataSet addEntityDS = integrationWrapper.AddEntitySync(out int totalCount);

                    if (addEntityDS != null && addEntityDS.Tables.Count > 0)
                    {
                        //if (addEntityDS.Tables[0] != null && addEntityDS.Tables[0].Rows.Count > 0)
                        //{
                        //    if (IsUpdated(Convert.ToDateTime(addEntityDS.Tables[0].Rows[0]["TimeStamp"].ToString()), ERPSettingsKeys.ERPAddEntityTimeStamp))
                        //        return;
                        //}

                        var parentRows = addEntityDS.Tables[0].AsEnumerable().Where(myRow => myRow["ParentId"] == DBNull.Value).ToList();
                        foreach (DataRow parentItem in parentRows)
                        {
                            OrgUnit orgUnit = orgUnitBL.GetOrgUnitByExternalId(Convert.ToInt32(parentItem["EntityExternalId"].ToString()));
                            int orgUnitId = orgUnit != null ? orgUnit.Id : 0;

                            OrgUnit orgUnitParent = new OrgUnit()
                            {
                                Id = orgUnitId,
                                ExternalId = Convert.ToInt32(parentItem["EntityExternalId"].ToString()),
                                IsActive = true,
                                Number = parentItem["EntityExternalId"].ToString(),
                                BarCode = "",
                                IsVirtualUnit = false,
                                IsDeleted = false,
                                TransactionsProcessingPeriod = Convert.ToInt32(SystemConfigurations.ERPProcessingPeriod),
                                LocalizationIdentifier = MapLocalizationIdentifier(parentItem["NameAr"].ToString(), parentItem["NameEn"].ToString()),
                                IsNew = true,
                                ManagerId = 0,
                                ParentId = (int?)null,
                            };

                            orgUnitBL.UpdateOrgUnitInfo(orgUnitParent);
                        }

                        var childRows = addEntityDS.Tables[0].AsEnumerable().Where(myRow => !string.IsNullOrWhiteSpace(myRow["ParentId"].ToString())).ToList();
                        foreach (DataRow childItem in childRows)
                        {
                            OrgUnit parentOrgUnit = (childItem["ParentId"] != null) ? orgUnitBL.GetOrgUnitByExternalId(Convert.ToInt32(childItem["ParentId"].ToString())) : null;
                            OrgUnit orgUnit = orgUnitBL.GetOrgUnitByExternalId(Convert.ToInt32(childItem["EntityExternalId"].ToString()));
                            int orgUnitId = orgUnit != null ? orgUnit.Id : 0;

                            OrgUnit orgUnitChild = new OrgUnit()
                            {
                                Id = orgUnitId,
                                ExternalId = Convert.ToInt32(childItem["EntityExternalId"].ToString()),
                                IsActive = true,
                                Number = childItem["EntityExternalId"].ToString(),
                                BarCode = "",
                                IsVirtualUnit = false,
                                IsDeleted = false,
                                TransactionsProcessingPeriod = Convert.ToInt32(SystemConfigurations.ERPProcessingPeriod),
                                LocalizationIdentifier = MapLocalizationIdentifier(childItem["NameAr"].ToString(), childItem["NameEn"].ToString()),
                                IsNew = true,
                                ManagerId = 0,
                                ParentId = (parentOrgUnit != null) ? parentOrgUnit.Id : (int?)null,
                            };

                            orgUnitBL.UpdateOrgUnitInfo(orgUnitChild);
                        }

                        AddUserSync();
                        if (addEntityDS.Tables[0].Rows.Count > 0)
                            UpdateTimestamp(ERPSettingsKeys.ERPAddEntityTimeStamp);
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

        public void MoveEntitySync()
        {
            try
            {
                if (SystemConfigurations.ERPIntegrationEnabled)
                {
                    IERPIntegrationWrapper integrationWrapper = IoC.Resolve<ERPIntegrationWrapper>();
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                    DataSet moveEntityDS = integrationWrapper.MoveEntitySync(out int totalCount);

                    if (moveEntityDS != null && moveEntityDS.Tables.Count > 0)
                    {
                        //if (moveEntityDS.Tables[0] != null && moveEntityDS.Tables[0].Rows.Count > 0)
                        //{
                        //    if (IsUpdated(Convert.ToDateTime(moveEntityDS.Tables[0].Rows[0]["TimeStamp"].ToString()), ERPSettingsKeys.ERPMoveEntityTimeStamp))
                        //        return;
                        //}

                        foreach (DataRow item in moveEntityDS.Tables[0].Rows)
                        {
                            OrgUnit entityFrom = orgUnitBL.GetOrgUnitByExternalId(Convert.ToInt32(item["EntityExternalId"].ToString()));
                            OrgUnit entityTo = orgUnitBL.GetOrgUnitByExternalId(Convert.ToInt32(item["EntityToExternalId"].ToString()));
                            if (entityFrom != null && entityTo != null && entityFrom.Id != entityTo.Id)
                            {
                                int conflictedEntityId = orgUnitBL.MoveEntity(entityFrom.Id, entityTo.Id, User.Id);
                            }
                        }

                        if (moveEntityDS.Tables[0].Rows.Count > 0)
                            UpdateTimestamp(ERPSettingsKeys.ERPMoveEntityTimeStamp);
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

        public void UpdateEntityNameSync()
        {
            try
            {
                if (SystemConfigurations.ERPIntegrationEnabled)
                {
                    IERPIntegrationWrapper integrationWrapper = IoC.Resolve<ERPIntegrationWrapper>();
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                    DataSet updateEntityDS = integrationWrapper.UpdateEntityNameSync(out int totalCount);

                    if (updateEntityDS != null && updateEntityDS.Tables.Count > 0)
                    {
                        //if (updateEntityDS.Tables[0] != null && updateEntityDS.Tables[0].Rows.Count > 0)
                        //{
                        //    if (IsUpdated(Convert.ToDateTime(updateEntityDS.Tables[0].Rows[0]["TimeStamp"].ToString()), ERPSettingsKeys.ERPUpdateEntityTimeStamp))
                        //        return;
                        //}

                        foreach (DataRow item in updateEntityDS.Tables[0].Rows)
                        {
                            OrgUnit orgUnit = orgUnitBL.GetOrgUnitByExternalId(Convert.ToInt32(item["EntityExternalId"].ToString()));

                            Localization currentlocalizationAr = orgUnit.LocalizationIdentifier.Localizations.FirstOrDefault(l => l.CultureId == 1);
                            if (currentlocalizationAr != null && !string.IsNullOrWhiteSpace(item["NameAr"].ToString()))
                            {
                                currentlocalizationAr.Text = item["NameAr"].ToString();
                            }

                            Localization currentlocalizationEn = orgUnit.LocalizationIdentifier.Localizations.FirstOrDefault(l => l.CultureId == 2);
                            if (currentlocalizationEn != null && !string.IsNullOrWhiteSpace(item["NameEn"].ToString()))
                            {
                                currentlocalizationEn.Text = item["NameEn"].ToString();
                            }

                            orgUnitBL.UpdateOrgUnitInfo(orgUnit);
                        }
                        if (updateEntityDS.Tables[0].Rows.Count > 0)
                            UpdateTimestamp(ERPSettingsKeys.ERPUpdateEntityTimeStamp);
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

        private void AddUser(DataRow item)
        {
            IOrgUnitBL OrgUnitBL = IoC.Resolve<IOrgUnitBL>();
            IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

            OrgUnit orgUnit = OrgUnitBL.GetOrgUnitByExternalId(Convert.ToInt32(item["MainEntityId"].ToString()));

            UserProfile userProfile = new UserProfile()
            {
                ExternalId = Convert.ToInt32(item["UserExternalId"].ToString()),
                IsActive = true,
                Email = string.IsNullOrEmpty(item["Email"].ToString()) ? "" : item["Email"].ToString(),
                PhoneNumber = item["PhoneNumber"].ToString(),
                TransactionProcessingPeriod = Convert.ToInt32(SystemConfigurations.ERPProcessingPeriod),
                UserName = string.IsNullOrEmpty(item["UserName"].ToString()) ? "" : item["UserName"].ToString(),
                TitleId = Convert.ToInt32(SystemConfigurations.ERPTitleId),
                Permissions = null,
                CategoryId = Convert.ToInt32(SystemConfigurations.ERPCategoryId),
                LocalizationIdentifier = MapLocalizationIdentifier(item["NameAr"].ToString(), item["NameEn"].ToString()),
                OrgUnits = new List<OrgUnit>() { orgUnit },
                UserNationalId = item["NationalId"].ToString(),
                MainOrgUnitId = orgUnit.Id,
                Gender = Convert.ToInt32(item["Gender"].ToString()) == 1 ? Gender.FeMale.LookupIdentity(LookupCategory.Gender, string.Empty) : Gender.Male.LookupIdentity(LookupCategory.Gender, string.Empty),
                //GroupId = Convert.ToBoolean(item["IsManager"]) ? Convert.ToInt32(SystemConfigurations.ERPAdminGroupId) : Convert.ToInt32(SystemConfigurations.ERPEditorGroupId),
                IsManager = Convert.ToBoolean(item["IsManager"]),
                Password = SystemConfigurations.ERPPassword
            };
            userManagementBL.AddUser(userProfile, "", "ar");
        }

        private bool IsUpdated(DateTime timeStamp, ERPSettingsKeys settingsKey)
        {
            ISettingBL settingBL = IoC.Resolve<ISettingBL>();

            List<Setting> settings = settingBL.GetSettingByModelId((int)SettingType.ERPSyncTimestamp);
            Setting setting = settings.Find(a => a.Key == settingsKey.ToString());
            if (setting != null && !string.IsNullOrWhiteSpace(setting.Value))
            {
                if (timeStamp > Convert.ToDateTime(setting.Value))
                {
                    return false;
                }
                return true;
            }

            return false;
        }

        private void UpdateTimestamp(ERPSettingsKeys settingsKey)
        {
            ISettingBL settingBL = IoC.Resolve<ISettingBL>();
            List<Setting> settings = settingBL.GetSettingByModelId((int)SettingType.ERPSyncTimestamp);
            Setting setting = settings.Find(a => a.Key == settingsKey.ToString());

            setting.Value = DateTime.Now.ToString();
            settingBL.UpdateSetting(setting);
        }

        private LocalizationIdentifier MapLocalizationIdentifier(string nameAr, string nameEn)
        {
            LocalizationIdentifier identifier = new LocalizationIdentifier();
            IList<Localization> localizations = new List<Localization>();

            Localization localizationAr = new Localization()
            {
                Id = 0,
                LocalizationIdentifier = identifier,
                Text = nameAr,
                CultureId = 1
            };

            Localization localizationEn = new Localization()
            {
                Id = 0,
                LocalizationIdentifier = identifier,
                Text = nameEn,
                CultureId = 2
            };

            localizations.Add(localizationAr);
            localizations.Add(localizationEn);

            identifier.Localizations = localizations;

            return identifier;
        }

        private enum ERPSettingsKeys
        {
            ERPAddUsersTimeStamp,
            ERPDeleteUsersTimeStamp,
            ERPMoveUsersTimeStamp,
            ERPDelegationUsersTimeStamp,
            ERPAddEntityTimeStamp,
            ERPMoveEntityTimeStamp,
            ERPUpdateEntityTimeStamp
        }
    }
}
