using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace MCS.Framework.Localization
{
    public interface IDataAccess : IDisposable
    {
        void GetConnectionInfo(string connectionString, string providerName = null);
        DbCommand CreateCommand(string sql, CommandType commandType, params object[] parameters);
        DbCommand CreateCommand(string sql, params object[] parameters);
        DbParameter CreateParameter(string parameterName, object value);
        DbParameter CreateParameter(string parameterName, object value, ParameterDirection parameterDirection = ParameterDirection.Input);
        DbParameter CreateParameter(string parameterName, object value, int size);
        DbParameter CreateParameter(string parameterName, object value, DbType type);
        DbParameter CreateParameter(string parameterName, object value, DbType type, int size);
        int ExecuteNonQuery(DbCommand Command);
        int ExecuteNonQuery(string sql, params object[] parameters);
        DbDataReader ExecuteReader(DbCommand command, params object[] parameters);
        DbDataReader ExecuteReader(string sql, params object[] parameters);
        DataTable ExecuteTable(string tablename, DbCommand command, params object[] parameters);
        DataTable ExecuteTable(string Tablename, string Sql, params object[] Parameters);
        DataSet ExecuteDataSet(string Tablename, DbCommand Command, params object[] Parameters);
        DataSet ExecuteDataSet(string tablename, string sql, params object[] parameters);
        DataSet ExecuteDataSet(DataSet dataSet, string tableName, DbCommand command, params object[] parameters);
        DataSet ExecuteDataSet(DataSet dataSet, string tablename, string sql, params object[] parameters);
        object ExecuteScalar(DbCommand command, params object[] parameters);
        object ExecuteScalar(string sql, params object[] parameters);
        bool RunSqlScript(string script, bool continueOnError = false, bool scriptIsFile = false);
        bool BeginTransaction();
        bool CommitTransaction();
        bool RollbackTransaction();
        string ErrorMessage { get; set; }
        DbTransaction Transaction { get; set; }
        void CloseConnection();
    }
}
