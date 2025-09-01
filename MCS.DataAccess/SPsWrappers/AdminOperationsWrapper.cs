
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MCS.Framework.Web;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class AdminOperationsWrapper : BaseWrappers, IAdminOperationsWrapper
    {
        public AdminOperationsWrapper(IAmbienTTransactionContextLocator ambienTTransactionContextLocator) : base(ambienTTransactionContextLocator)
        {
        }
        public void AdminMoveUser(int userID, int orgunitID, int newOrgunitID, int loggedinUserID)
        {
            try
            {

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand(
                        @"BEGIN ADMINMOVEUSER (:P_USERPROFILEID, :P_ORGUNITID, :P_NEWORGUNITID, :P_LOGGEDINUSER); END;",
                        new OracleParameter(":P_USERPROFILEID", OracleDbType.Int32, userID, ParameterDirection.Input),
                        new OracleParameter(":P_ORGUNITID", OracleDbType.Int32, orgunitID, ParameterDirection.Input),
                        new OracleParameter(":P_NEWORGUNITID", OracleDbType.Int32, newOrgunitID, ParameterDirection.Input),
                        new OracleParameter(":P_LOGGEDINUSER", OracleDbType.Int32, loggedinUserID, ParameterDirection.Input)
                    );
                }
                else
                {

                    _oMCSDbContext.Database.ExecuteSqlCommand("AdminMoveUser @UserProfileId, @OrgUnitId, @NewOrgUnitId, @LoggedInUser",
                                               new SqlParameter("UserProfileId", userID),
                                               new SqlParameter("OrgUnitId", orgunitID),
                                               new SqlParameter("NewOrgUnitId", newOrgunitID),
                                               new SqlParameter("LoggedInUser", loggedinUserID));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void AdminMoveUser(string usersIDs, int orgunitID, int newOrgunitID, int loggedinUserID, bool isExternal = false)
        {
            try
            {

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand(
                        @"BEGIN ADMIN_MOVE_USERS 
                    (:v_UserProfileId, :v_OrgUnitId, :v_NewOrgUnitId, :v_LoggedInUser , :p_TrayOrgUnit, :p_TraySaved, :p_TrayMyTransactions ,:p_TrayDraftOutbound, :p_IsExternal); END;",
                    new OracleParameter(":p_UserProfileIds", OracleDbType.NVarchar2, usersIDs, ParameterDirection.Input),
                    new OracleParameter(":p_OrgUnitId", OracleDbType.Int32, orgunitID, ParameterDirection.Input),
                    new OracleParameter(":p_NewOrgUnitId", OracleDbType.Int32, newOrgunitID, ParameterDirection.Input),
                    new OracleParameter(":p_LoggedInUser", OracleDbType.Int32, loggedinUserID, ParameterDirection.Input),
                    new OracleParameter(":p_TrayOrgUnit", OracleDbType.Int32, (int)TrayType.OrgUnit, ParameterDirection.Input),
                    new OracleParameter(":p_TraySaved", OracleDbType.Int32, (int)TrayType.Saved, ParameterDirection.Input),
                    new OracleParameter(":p_TrayMyTransactions", OracleDbType.Int32, (int)TrayType.MyTransactions, ParameterDirection.Input),
                    new OracleParameter(":p_TrayDraftOutbound", OracleDbType.Int32, (int)TrayType.DraftOutbound, ParameterDirection.Input),
                    new OracleParameter(":p_IsExternal", OracleDbType.Boolean, isExternal, ParameterDirection.Input)
                    );
                }
                else
                {

                    _oMCSDbContext.Database.ExecuteSqlCommand("AdminMoveUser @UserProfileId, @OrgUnitId, @NewOrgUnitId, @LoggedInUser",
                                               new SqlParameter("UserProfileId", usersIDs),
                                               new SqlParameter("OrgUnitId", orgunitID),
                                               new SqlParameter("NewOrgUnitId", newOrgunitID),
                                               new SqlParameter("LoggedInUser", loggedinUserID));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void AdminDeleteUserERP(int userId, int externalOrgUnitId, int loggedinUserID)
        {
            try
            {

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand(
                        @"BEGIN ADMIN_DELETE_USER_ERP 
                    (:v_UserProfileId, :v_OrgUnitId, :v_LoggedInUser , :p_TrayOrgUnit, :p_TraySaved, :p_TrayMyTransactions ,:p_TrayDraftOutbound); END;",
                    new OracleParameter(":p_UserProfileId", OracleDbType.NVarchar2, userId, ParameterDirection.Input),
                    new OracleParameter(":p_ExternalOrgUnitId", OracleDbType.Int32, externalOrgUnitId, ParameterDirection.Input),
                    new OracleParameter(":p_LoggedInUser", OracleDbType.Int32, loggedinUserID, ParameterDirection.Input),
                    new OracleParameter(":p_TrayOrgUnit", OracleDbType.Int32, (int)TrayType.OrgUnit, ParameterDirection.Input),
                    new OracleParameter(":p_TraySaved", OracleDbType.Int32, (int)TrayType.Saved, ParameterDirection.Input),
                    new OracleParameter(":p_TrayMyTransactions", OracleDbType.Int32, (int)TrayType.MyTransactions, ParameterDirection.Input),
                    new OracleParameter(":p_TrayDraftOutbound", OracleDbType.Int32, (int)TrayType.DraftOutbound, ParameterDirection.Input)
                    );
                }
                else
                {

                    _oMCSDbContext.Database.ExecuteSqlCommand("ADMIN_DELETE_USER_ERP @UserProfileId, @OrgUnitId, @LoggedInUser",
                                               new SqlParameter("UserProfileId", userId),
                                               new SqlParameter("OrgUnitId", externalOrgUnitId),
                                               new SqlParameter("LoggedInUser", loggedinUserID));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private bool CheckIfHasSameName(List<Localization> localization01, List<Localization> localization02)
        {
            try
            {
                bool isMatched = false;
                if (localization01 == null || localization02 == null)
                {
                    isMatched = false;
                }

                if (localization01 == localization02) // check memory refrence 
                {
                    isMatched = true;
                }

                for (int i = 0; i < localization01.Count; i++)
                {
                    if (localization01[i].Text.CompareTo(localization02[i].Text) == 0)
                    {
                        isMatched = true;
                        break;
                    }
                }

                return isMatched;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private int CheckOrgUnitsNamesIfMatched(OrgUnit entityToMove, List<OrgUnit> newPerantAndChilds)
        {
            int entityId = -1;
            foreach (var item in newPerantAndChilds)
            {
                if (CheckIfHasSameName(entityToMove.LocalizationIdentifier.Localizations.ToList(), item.LocalizationIdentifier.Localizations.ToList()))
                {
                    entityId = item.Id;
                    break;
                }
            }
            return entityId;
        }

        public int AdminMoveEntity(int entityFromId, int entityToId, int logInUser, bool noExternal = false)
        {
            try
            {

                if (entityFromId == entityToId)
                {
                    throw new DataAccessException(StatusCode.CanNotMoveEntityToItself.ToString());
                }

                OrgUnit entityToMove = _oMCSDbContext.OrgUnits.FirstOrDefault(e => e.Id == entityFromId);
                OrgUnit DestinationEntity = _oMCSDbContext.OrgUnits.FirstOrDefault(e => e.Id == entityToId);

                if (noExternal && (entityToMove.ExternalId.HasValue || DestinationEntity.ExternalId.HasValue))
                {
                    throw new DataAccessException(StatusCode.CanNotMoveExternalEntity.ToString());
                }

                if (DestinationEntity.ParentId == entityToMove.Id)
                {
                    throw new DataAccessException(StatusCode.CanNotMoveParentEntityToChildEntity.ToString());
                }

                if (entityToMove.ParentId == DestinationEntity.Id)
                {
                    throw new DataAccessException(StatusCode.AlreadyChildOfThisEntity.ToString());
                }

                List<OrgUnit> newPerantAndChilds = _oMCSDbContext.OrgUnits.Where(e => (e.Lineage.StartsWith(entityToId.ToString()) && e.ParentId == DestinationEntity.Id) || e.Id == entityToId).ToList();
                int ConflictedEntityId = CheckOrgUnitsNamesIfMatched(entityToMove, newPerantAndChilds);
                if (ConflictedEntityId != -1)
                {
                    return ConflictedEntityId;
                }

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand(
                          @"BEGIN ADMINMOVEENTITY 
                    (:P_ORGUNITID, :P_NEWPARENTID, :P_LOGGEDINUSER); END;",
                      new OracleParameter(":P_ORGUNITID", OracleDbType.Int32, entityFromId, ParameterDirection.Input),
                      new OracleParameter(":P_NEWPARENTID", OracleDbType.Int32, entityToId, ParameterDirection.Input),
                      new OracleParameter(":P_LOGGEDINUSER", OracleDbType.Int32, logInUser, ParameterDirection.Input)
                      );
                }
                else
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("AdminMoveEntity @OrgUnitId, @NewParentID, @LoggedInUser",
                  new SqlParameter("OrgUnitId", entityFromId),
                  new SqlParameter("NewParentID", entityToId),
                  new SqlParameter("LoggedInUser", logInUser));
                }
                return ConflictedEntityId;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void AdminMoveTransactions(int entityFromId, int entityToId, int userFromId, int userToId, int logInUser)
        {
            try
            {

                if (entityFromId == entityToId & userToId == userFromId)
                {
                    throw new DataAccessException();
                }

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand(
                         @"BEGIN ADMINMOVETRANSACTIONS 
                    (:P_TOUSERID, :P_TOENTITYID, :P_FROMUSERID, :P_FROMENTITYID, :P_LOGGEDINUSER); END;",
                     new OracleParameter(":P_TOUSERID", OracleDbType.Int32, userToId, ParameterDirection.Input),
                     new OracleParameter(":P_TOENTITYID", OracleDbType.Int32, entityToId, ParameterDirection.Input),
                     new OracleParameter(":P_FROMUSERID", OracleDbType.Int32, userFromId, ParameterDirection.Input),
                     new OracleParameter(":P_FROMENTITYID", OracleDbType.Int32, entityFromId, ParameterDirection.Input),
                     new OracleParameter(":P_LOGGEDINUSER", OracleDbType.Int32, logInUser, ParameterDirection.Input)
                     );
                }
                else
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("AdminMoveTransactions @ToUserID, @ToEntityID, @FromUserID, @FromEntityID, @LoggedInUser", new SqlParameter("ToUserID", userToId),
                     new SqlParameter("ToEntityID", entityToId),
                     new SqlParameter("FromUserID", userFromId),
                     new SqlParameter("FromEntityID", entityFromId),
                     new SqlParameter("LoggedInUser", logInUser));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void AdminMoveTransactionById(int transId, int toUserId, int toEntityId, int loggedInUser)
        {
            try
            {

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand(
                         @"BEGIN ADMIN_MOVE_TRANSACTION_BYID
                    (:v_TransID, :v_ToUserID, :v_ToEntityID, :v_LoggedInUser, :p_TrayMyTransactions, :p_TrayOrgUnit); END;",
                     new OracleParameter(":p_TransID", OracleDbType.Int32, transId, ParameterDirection.Input),
                     new OracleParameter(":p_ToUserID", OracleDbType.Int32, toUserId, ParameterDirection.Input),
                     new OracleParameter(":p_ToEntityID", OracleDbType.Int32, toEntityId, ParameterDirection.Input),
                     new OracleParameter(":p_LoggedInUser", OracleDbType.Int32, loggedInUser, ParameterDirection.Input),
                     new OracleParameter(":p_TrayMyTransactions", OracleDbType.Int32, (int)TrayType.MyTransactions, ParameterDirection.Input),
                     new OracleParameter(":p_TrayOrgUnit", OracleDbType.Int32, (int)TrayType.OrgUnit, ParameterDirection.Input)
                      );
                }
                else
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("AdminMoveTransactionsByID @TransID, @ToUserID, @ToEntityID, @LoggedInUser",
                            new SqlParameter("TransID", transId),
                            new SqlParameter("ToUserID", toUserId),
                            new SqlParameter("ToEntityID", toEntityId),
                            new SqlParameter("loggedInUser", loggedInUser));
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int MergeDepartments(MergeDepartment mergeDepartment, bool noExternal = false)
        {
            try
            {
                if (mergeDepartment.MergedEntityId == mergeDepartment.BaseEntityId)
                {
                    throw new DataAccessException(StatusCode.CanNotMergeEntityToItself.ToString());
                }

                OrgUnit entityToBeMerged = _oMCSDbContext.OrgUnits.FirstOrDefault(e => e.Id == mergeDepartment.MergedEntityId);
                OrgUnit DestinationEntity = _oMCSDbContext.OrgUnits.FirstOrDefault(e => e.Id == mergeDepartment.BaseEntityId);

                if (noExternal && (entityToBeMerged.ExternalId.HasValue || DestinationEntity.ExternalId.HasValue))
                {
                    throw new DataAccessException(StatusCode.CanNotMergeExternalEntity.ToString());
                }

                if (DestinationEntity.ParentId == entityToBeMerged.Id)
                {
                    throw new DataAccessException(StatusCode.CanNotMergeParentEntityToChildEntity.ToString());
                }

                List<OrgUnit> newPerantAndChilds = _oMCSDbContext.OrgUnits.Where(e => (e.Lineage.StartsWith(mergeDepartment.BaseEntityId.ToString()) && e.ParentId == DestinationEntity.Id) || e.Id == mergeDepartment.BaseEntityId).ToList();
                int ConflictedEntityId = CheckOrgUnitsNamesIfMatched(entityToBeMerged, newPerantAndChilds);
                if (ConflictedEntityId != -1)
                {
                    return ConflictedEntityId;
                }

                if (CheckEntityIfHasExternalTransactionsAndCopies(mergeDepartment.MergedEntityId))
                {
                    throw new DataAccessException(StatusCode.OrgUnitHasTransactionsFromExternalParties.ToString());
                }

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand(
                         @"BEGIN MERGE_DEPARTMENTS
                    (:p_MergedEntityId, :p_BaseEntityId, :p_ManagerId, :p_UserId); END;",
                     new OracleParameter(":p_MergedEntityId", OracleDbType.Int32, mergeDepartment.MergedEntityId, ParameterDirection.Input),
                     new OracleParameter(":p_BaseEntityId", OracleDbType.Int32, mergeDepartment.BaseEntityId, ParameterDirection.Input),
                     new OracleParameter(":p_ManagerId", OracleDbType.Int32, mergeDepartment.ManagerId, ParameterDirection.Input),
                     new OracleParameter(":p_UserId", OracleDbType.Int32, UserContext.LoggedInUser.Id, ParameterDirection.Input)
                     );
                }
                else
                {
                    //_oMCSDbContext.Database.ExecuteSqlCommand("AdminMoveTransactionsByID @TransID, @ToUserID, @ToEntityID, @LoggedInUser",
                    //        new SqlParameter("TransID", transId),
                    //        new SqlParameter("ToUserID", toUserId),
                    //        new SqlParameter("ToEntityID", toEntityId),
                    //        new SqlParameter("loggedInUser", loggedInUser));
                }

                if (mergeDepartment.NewEntityNames[0].Text != null && mergeDepartment.NewEntityNames[1].Text != null)
                {
                    var baseEntityName = _oMCSDbContext.OrgUnits.FirstOrDefault(o => o.Id == mergeDepartment.BaseEntityId).LocalizationIdentifier.Localizations;

                    foreach (var item in baseEntityName)
                    {
                        _oMCSDbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    }

                    baseEntityName.FirstOrDefault(n => n.CultureId == mergeDepartment.NewEntityNames[0].CultureId).Text = mergeDepartment.NewEntityNames[0].Text;
                    baseEntityName.FirstOrDefault(n => n.CultureId == mergeDepartment.NewEntityNames[1].CultureId).Text = mergeDepartment.NewEntityNames[1].Text;

                    _oMCSDbContext.SaveChanges();

                }
                return ConflictedEntityId;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public bool CheckEntityIfHasExternalTransactionsAndCopies(int EntityId)
        {
            try
            {
                List<HubTransaction> hubTransactionList = _oMCSDbContext.HubTransactions.Where(
                    t => t.Status == HubTransactionStatus.Pending &&
                    t.DestinationId == EntityId &&
                    !t.IsDeleted).OrderByDescending(t => t.CreatedOn).ToList();

                return hubTransactionList.Count() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public interface IAdminOperationsWrapper
    {
        void AdminMoveUser(int userID, int orgunitID, int newOrgunitID, int loggedinUserID);
        void AdminMoveUser(string usersIDs, int orgunitID, int newOrgunitID, int loggedinUserID, bool isExternal = false);
        int AdminMoveEntity(int entityFromId, int entityToId, int logInUser, bool noExternal = false);
        void AdminMoveTransactions(int entityFromId, int entityToId, int userFromId, int userToId, int logInUser);
        void AdminMoveTransactionById(int transId, int toUserId, int toEntityId, int loggedInUser);
        int MergeDepartments(MergeDepartment mergeDepartment, bool noExternal = false);
        void AdminDeleteUserERP(int userId, int externalOrgUnitId, int loggedinUserID);
    }
}
