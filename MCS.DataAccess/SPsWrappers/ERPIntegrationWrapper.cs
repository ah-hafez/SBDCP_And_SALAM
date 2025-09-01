using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class ERPIntegrationWrapper : BaseWrappers, IERPIntegrationWrapper
    {
        public ERPIntegrationWrapper(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator) { }

        private static string ERPConnectionString = SystemConfigurations.ERPConnectionString;
        public DataSet AddUserSync(out int totalCount)
        {
            try
            {
                totalCount = 0;
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = ERPConnectionString;
                conn.Open();

                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = conn;
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "ERP_USERS_ADD_VIEW";

                DataSet ds = new DataSet();
                OracleDataAdapter oraDa = new OracleDataAdapter(objCmd);
                oraDa.Fill(ds, "users");

                conn.Close();

                if (ds != null && ds.Tables.Count > 0)
                {
                    totalCount = ds.Tables["users"].Rows.Count;
                    return ds;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet DeleteUserSync(out int totalCount)
        {
            try
            {
                totalCount = 0;
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = ERPConnectionString;
                conn.Open();

                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = conn;
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "ERP_USERS_DELETE_VIEW";

                DataSet ds = new DataSet();
                OracleDataAdapter oraDa = new OracleDataAdapter(objCmd);
                oraDa.Fill(ds, "users");

                conn.Close();

                if (ds != null && ds.Tables.Count > 0)
                {
                    totalCount = ds.Tables["users"].Rows.Count;
                    return ds;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet MoveUserSync(out int totalCount)
        {
            try
            {
                totalCount = 0;
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = ERPConnectionString;
                conn.Open();

                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = conn;
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "ERP_USERS_MOVE_VIEW";

                DataSet ds = new DataSet();
                OracleDataAdapter oraDa = new OracleDataAdapter(objCmd);
                oraDa.Fill(ds, "users");

                conn.Close();

                if (ds != null && ds.Tables.Count > 0)
                {
                    totalCount = ds.Tables["users"].Rows.Count;
                    return ds;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet DelegationUserSync(out int totalCount)
        {
            try
            {
                totalCount = 0;
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = ERPConnectionString;
                conn.Open();

                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = conn;
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "ERP_USER_DELEGATION_VIEW";

                DataSet ds = new DataSet();
                OracleDataAdapter oraDa = new OracleDataAdapter(objCmd);
                oraDa.Fill(ds, "users");

                conn.Close();

                if (ds != null && ds.Tables.Count > 0)
                {
                    totalCount = ds.Tables["users"].Rows.Count;
                    return ds;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet AddEntitySync(out int totalCount)
        {
            try
            {
                totalCount = 0;
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = ERPConnectionString;
                conn.Open();

                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = conn;
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "ERP_ENTITY_ADD_VIEW";

                DataSet ds = new DataSet();
                OracleDataAdapter oraDa = new OracleDataAdapter(objCmd);
                oraDa.Fill(ds, "entity");

                conn.Close();

                if (ds != null && ds.Tables.Count > 0)
                {
                    totalCount = ds.Tables["entity"].Rows.Count;
                    return ds;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet MoveEntitySync(out int totalCount)
        {
            try
            {
                totalCount = 0;
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = ERPConnectionString;
                conn.Open();

                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = conn;
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "ERP_ENTITY_MOVE_VIEW";

                DataSet ds = new DataSet();
                OracleDataAdapter oraDa = new OracleDataAdapter(objCmd);
                oraDa.Fill(ds, "entity");

                conn.Close();

                if (ds != null && ds.Tables.Count > 0)
                {
                    totalCount = ds.Tables["entity"].Rows.Count;
                    return ds;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet UpdateEntityNameSync(out int totalCount)
        {
            try
            {
                totalCount = 0;
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = ERPConnectionString;
                conn.Open();

                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = conn;
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "ERP_ENTITY_UPDATE_NAME_VIEW";

                DataSet ds = new DataSet();
                OracleDataAdapter oraDa = new OracleDataAdapter(objCmd);
                oraDa.Fill(ds, "entity");

                conn.Close();

                if (ds != null && ds.Tables.Count > 0)
                {
                    totalCount = ds.Tables["entity"].Rows.Count;
                    return ds;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    public interface IERPIntegrationWrapper
    {
        DataSet AddUserSync(out int totalCount);
        DataSet DeleteUserSync(out int totalCount);
        DataSet MoveUserSync(out int totalCount);
        DataSet AddEntitySync(out int totalCount);
        DataSet MoveEntitySync(out int totalCount);
        DataSet UpdateEntityNameSync(out int totalCount);
        DataSet DelegationUserSync(out int totalCount);
    }
}
