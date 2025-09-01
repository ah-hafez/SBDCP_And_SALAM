using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using MCS.Framework.Encryption;

namespace MCS.Framework.Localization
{
    /// <summary>
    /// Basic low level Data Access Layer
    /// 
    /// </summary>

    public abstract class DataAccessBase : IDataAccess, IDisposable
    {
        private string _ErrorMessage = string.Empty;
        private string _ParameterPrefix = "@";
        private string _ConnectionString = string.Empty;
        private int _timeout = -1;
        //private const string STR_DefaultProviderName = "System.Data.SqlClient";
        /// <summary>
        /// The internally used dbProvider
        /// 
        /// </summary>
        public DbProviderFactory dbProvider;
        private int _ErrorNumber;
        private DbTransaction _Transaction;
        protected DbConnection _Connection;
        private bool _ExecuteWithSchema;

        /// <summary>
        /// An error message if a method fails
        /// 
        /// </summary>
        public virtual string ErrorMessage
        {
            get
            {
                return _ErrorMessage;
            }
            set
            {
                _ErrorMessage = value;
            }
        }

        /// <summary>
        /// Optional error number returned by failed SQL commands
        /// 
        /// </summary>
        public int ErrorNumber
        {
            get
            {
                return _ErrorNumber;
            }
            set
            {
                _ErrorNumber = value;
            }
        }

        /// <summary>
        /// The prefix used by the provider
        /// 
        /// </summary>
        public string ParameterPrefix
        {
            get
            {
                return _ParameterPrefix;
            }
            set
            {
                _ParameterPrefix = value;
            }
        }

        /// <summary>
        /// ConnectionString for the data access component
        /// 
        /// </summary>
        public virtual string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
            set
            {
                _ConnectionString = value;
            }
        }

        /// <summary>
        /// A SQL Transaction object that may be active. You can
        ///             also set this object explcitly
        /// 
        /// </summary>
        public virtual DbTransaction Transaction
        {
            get
            {
                return _Transaction;
            }
            set
            {
                _Transaction = value;
            }
        }

        /// <summary>
        /// The SQL Connection object used for connections
        /// 
        /// </summary>
        public virtual DbConnection Connection
        {
            get
            {
                return _Connection;
            }
            set
            {
                _Connection = value;
            }
        }

        /// <summary>
        /// The Sql Command execution Timeout in seconds.
        ///             Set to -1 for whatever the system default is.
        ///             Set to 0 to never timeout (not recommended).
        /// 
        /// </summary>
        public int Timeout
        {
            get
            {
                return _timeout;
            }
            set
            {
                _timeout = value;
            }
        }

        /// <summary>
        /// Determines whether extended schema information is returned for
        ///             queries from the server. Useful if schema needs to be returned
        ///             as part of DataSet XML creation
        /// 
        /// </summary>
        public virtual bool ExecuteWithSchema
        {
            get
            {
                return _ExecuteWithSchema;
            }
            set
            {
                _ExecuteWithSchema = value;
            }
        }

        /// <summary>
        /// Default constructor that should be called back to
        ///             by subclasses. Parameterless assumes default provider
        ///             and no connection string which must be explicitly set.
        /// 
        /// </summary>
        protected DataAccessBase()
        {
        }

        /// <summary>
        /// Most common constructor that expects a connection string or
        ///             connection string name from a .config file. If a connection
        ///             string is provided the default provider is used.
        /// 
        /// </summary>
        /// <param name="connectionString"/>
        protected DataAccessBase(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException(Resources.Resources.AConnectionStringMustBePassedToTheConstructor);

            var aes = new SimpleAES();

            //To be encrypted on production
            //var encryptStr = aes.EncryptToString(connectionString);
            //var decryptStr = aes.DecryptString(connectionString); 

            GetConnectionInfo(connectionString, null);
        }

        /// <summary>
        /// Constructor that expects a full connection string and provider
        ///             for creating a SQL instance. To be called by the same implementation
        ///             on a subclass.
        /// 
        /// </summary>
        /// <param name="connectionString"/><param name="providerName"/>
        //protected DataAccessBase(string connectionString, string providerName)
        //{
        //    this.dbProvider = DbProviderFactories.GetFactory(providerName);
        //    this.ConnectionString = connectionString;
        //}

        /// <summary>
        /// Figures out the dbProvider and Connection string from a
        ///             connectionString name in a config file or explicit
        ///             ConnectionString and provider.
        /// 
        /// </summary>
        /// <param name="connectionString">Config file connection name or full connection string</param><param name="providerName">optional provider name. If not passed with a connection string is considered Sql Server</param>
        public void GetConnectionInfo(string connectionString, string providerName = null)
        {
            ConnectionStringInfo connectionStringInfo = ConnectionStringInfo.GetConnectionStringInfo(connectionString, providerName);

            ConnectionString = connectionStringInfo.ConnectionString;
            dbProvider = connectionStringInfo.Provider;
        }

        /// <summary>
        /// Opens a Sql Connection based on the connection string.
        ///             Called internally but externally accessible. Sets the internal
        ///             _Connection property.
        /// 
        /// </summary>
        /// 
        /// <returns/>
        /// 
        /// <summary>
        /// Opens a Sql Connection based on the connection string.
        ///             Called internally but externally accessible. Sets the internal
        ///             _Connection property.
        /// 
        /// </summary>
        /// 
        /// <returns/>
        public virtual bool OpenConnection()
        {
            try
            {
                if (_Connection == null)
                {
                    if (ConnectionString.Contains("="))
                    {
                        _Connection = dbProvider.CreateConnection();
                        _Connection.ConnectionString = ConnectionString;
                    }
                    else
                    {
                        ConnectionStringInfo connectionStringInfo = ConnectionStringInfo.GetConnectionStringInfo(ConnectionString, null);
                        if (connectionStringInfo == null)
                        {
                            SetError(Resources.Resources.InvalidConnectionString);
                            return false;
                        }
                        else
                        {
                            dbProvider = connectionStringInfo.Provider;
                            ConnectionString = connectionStringInfo.ConnectionString;
                            _Connection = dbProvider.CreateConnection();
                            _Connection.ConnectionString = ConnectionString;
                        }
                    }
                }
                if (_Connection.State != ConnectionState.Open)
                    _Connection.Open();
            }
            catch (SqlException ex)
            {
                SetError(ex);
                return false;
            }
            catch (DbException ex)
            {
                SetError(ex);
                return false;
            }
            catch (Exception ex)
            {
                SetError(ex.GetBaseException().Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Creates a Command object and opens a connection
        /// 
        /// </summary>
        /// <param name="ConnectionString">Connection string or ConnnectionString configuration name</param><param name="sql">Sql string to create</param><param name="commandType">Type of command to create</param><param name="parameters">Parameter values that map to @0,@1 or DbParameter objects created with CreateParameter()</param>
        /// <returns/>
        public virtual DbCommand CreateCommand(string sql, CommandType commandType, params object[] parameters)
        {
            SetError();
            using (DbCommand command = dbProvider.CreateCommand())
            {
                command.CommandType = commandType;
                command.CommandText = sql;
                if (Timeout > -1)
                    command.CommandTimeout = Timeout;
                try
                {
                    if (Transaction != null)
                    {
                        command.Transaction = Transaction;
                        command.Connection = Transaction.Connection;
                    }
                    else
                    {
                        if (!OpenConnection())
                            return null;
                        command.Connection = _Connection;
                    }
                }
                catch (DbException ex)
                {
                    SetError(ex.Message, ex.ErrorCode);
                    return null;
                }
                catch (Exception ex)
                {
                    SetError(ex.GetBaseException().Message);
                    return null;
                }
                if (parameters != null)
                    AddParameters(command, parameters);
                return command;
            }
        }

        /// <summary>
        /// Creates a Command object and opens a connection
        /// 
        /// </summary>
        /// <param name="ConnectionString">Connection String or Connection String Entry from config file</param><param name="sql">Sql string to execute</param>
        /// <returns>
        /// Parameters. Either values mapping to @0,@1,@2 etc. or DbParameter objects created with CreateParameter()
        /// </returns>
        public virtual DbCommand CreateCommand(string sql, params object[] parameters)
        {
            return CreateCommand(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// Adds parameters to a DbCommand instance. Parses value and DbParameter parameters
        ///             properly into the command's Parameters collection.
        /// 
        /// </summary>
        /// <param name="command"/><param name="parameters"/>
        protected void AddParameters(DbCommand command, object[] parameters)
        {
            if (parameters == null)
                return;
            int num = 0;
            foreach (object obj in parameters)
            {
                if (obj is DbParameter && !command.Parameters.Contains(obj))
                {
                    command.Parameters.Add(obj);
                }
                else
                {
                    DbParameter parameter = CreateParameter(ParameterPrefix + num, obj);
                    command.Parameters.Add(parameter);
                    ++num;
                }
            }
        }

        /// <summary>
        /// Used to create named parameters to pass to commands or the various
        ///             methods of this class.
        /// 
        /// </summary>
        /// <param name="parameterName"/><param name="value"/><param name="dbType"/>
        /// <returns/>
        public virtual DbParameter CreateParameter(string parameterName, object value)
        {
            DbParameter parameter = dbProvider.CreateParameter();
            parameter.ParameterName = parameterName;
            if (value == null)
                value = DBNull.Value;
            parameter.Value = value;
            return parameter;
        }

        /// <summary>
        /// Used to create named parameters to pass to commands or the various
        ///             methods of this class.
        /// 
        /// </summary>
        /// <param name="parameterName"/><param name="value"/><param name="dbType"/>
        /// <returns/>
        public virtual DbParameter CreateParameter(string parameterName, object value, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            DbParameter parameter = CreateParameter(parameterName, value);
            parameter.Direction = parameterDirection;
            return parameter;
        }

        /// <summary>
        /// Used to create named parameters to pass to commands or the various
        ///             methods of this class.
        /// 
        /// </summary>
        /// <param name="parameterName"/><param name="value"/><param name="size"/>
        /// <returns/>
        public virtual DbParameter CreateParameter(string parameterName, object value, int size)
        {
            DbParameter parameter = CreateParameter(parameterName, value);
            parameter.Size = size;
            return parameter;
        }

        /// <summary>
        /// Used to create named parameters to pass to commands or the various
        ///             methods of this class.
        /// 
        /// </summary>
        /// <param name="parameterName"/><param name="value"/><param name="dbType"/>
        /// <returns/>
        public virtual DbParameter CreateParameter(string parameterName, object value, DbType type)
        {
            DbParameter parameter = CreateParameter(parameterName, value);
            parameter.DbType = type;
            return parameter;
        }

        /// <summary>
        /// Used to create named parameters to pass to commands or the various
        ///             methods of this class.
        /// 
        /// </summary>
        /// <param name="parameterName"/><param name="value"/><param name="type"/><param name="size"/>
        /// <returns/>
        public virtual DbParameter CreateParameter(string parameterName, object value, DbType type, int size)
        {
            DbParameter parameter = CreateParameter(parameterName, value);
            parameter.DbType = type;
            parameter.Size = size;
            return parameter;
        }

        /// <summary>
        /// Executes a non-query command and returns the affected records
        /// 
        /// </summary>
        /// <param name="Command">Command should be created with GetSqlCommand to have open connection</param><param name="Parameters"/>
        /// <returns/>
        public virtual int ExecuteNonQuery(DbCommand Command)
        {
            SetError();
            int num = 0;
            try
            {
                num = Command.ExecuteNonQuery();
                if (num == -1)
                    num = 0;
            }
            catch (DbException ex)
            {
                num = -1;
                SetError(ex);
            }
            catch (Exception ex)
            {
                num = -1;
                SetError(ex);
            }
            finally
            {
                CloseConnection();
            }
            return num;
        }

        /// <summary>
        /// Executes a command that doesn't return any data. The result
        ///             returns the number of records affected or -1 on error.
        /// 
        /// </summary>
        /// <param name="sql">SQL statement as a string</param><param name="parameters">Any number of SQL named parameters</param>
        /// <returns/>
        /// 
        /// <summary>
        /// Executes a command that doesn't return a data result. You can return
        ///             output parameters and you do receive an AffectedRecords counter.
        /// 
        /// </summary>
        public virtual int ExecuteNonQuery(string sql, params object[] parameters)
        {
            using (DbCommand command = CreateCommand(sql, parameters))
            {
                if (command == null)
                    return -1;
                else
                    return ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// Executes a SQL Command object and returns a SqlDataReader object
        /// 
        /// </summary>
        /// <param name="command">Command should be created with GetSqlCommand and open connection</param><param name="parameters"/>
        /// <returns/>
        /// 
        /// <returns>
        /// A SqlDataReader. Make sure to call Close() to close the underlying connection.
        /// </returns>
        public virtual DbDataReader ExecuteReader(DbCommand command, params object[] parameters)
        {
            SetError();
            if (command.Connection == null || command.Connection.State != ConnectionState.Open)
            {
                if (!OpenConnection())
                    return null;
                command.Connection = _Connection;
            }
            AddParameters(command, parameters);
            try
            {
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                SetError(ex.GetBaseException().Message);
                CloseConnection(command);
                return null;
            }
        }

        /// <summary>
        /// Executes a SQL command against the server and returns a DbDataReader
        /// 
        /// </summary>
        /// <param name="sql">Sql String</param><param name="parameters">Any SQL parameters </param>
        /// <returns/>
        public virtual DbDataReader ExecuteReader(string sql, params object[] parameters)
        {
            using (DbCommand command = CreateCommand(sql, parameters))
            {
                if (command == null)
                    return null;
                else
                    return ExecuteReader(command);
            }
        }

        /// <summary>
        /// Returns a DataTable from a Sql Command string passed in.
        /// 
        /// </summary>
        /// <param name="tablename"/><param name="command"/><param name="parameters"/>
        /// <returns/>
        public virtual DataTable ExecuteTable(string tablename, DbCommand command, params object[] parameters)
        {
            SetError();
            AddParameters(command, parameters);
            DbDataAdapter dataAdapter = dbProvider.CreateDataAdapter();
            dataAdapter.SelectCommand = command;
            DataTable dataTable = new DataTable(tablename);
            try
            {
                dataAdapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                SetError(ex.GetBaseException().Message);
                return null;
            }
            finally
            {
                CloseConnection(command);
            }
            return dataTable;
        }

        /// <summary>
        /// Returns a DataTable from a Sql Command string passed in.
        /// 
        /// </summary>
        /// <param name="Tablename"/><param name="ConnectionString"/><param name="Sql"/><param name="Parameters"/>
        /// <returns/>
        public virtual DataTable ExecuteTable(string Tablename, string Sql, params object[] Parameters)
        {
            SetError();
            using (DbCommand command = CreateCommand(Sql, Parameters))
            {
                if (command == null)
                    return null;
                else
                    return ExecuteTable(Tablename, command);
            }
        }

        /// <summary>
        /// Returns a DataSet/DataTable from a Sql Command string passed in.
        /// 
        /// </summary>
        /// <param name="Tablename">The name for the table generated or the base names</param><param name="Command"/><param name="Parameters"/>
        /// <returns/>
        public virtual DataSet ExecuteDataSet(string Tablename, DbCommand Command, params object[] Parameters)
        {
            return ExecuteDataSet(null, Tablename, Command, Parameters);
        }

        /// <summary>
        /// Executes a SQL command against the server and returns a DataSet of the result
        /// 
        /// </summary>
        /// <param name="command"/><param name="parameters"/>
        /// <returns/>
        public virtual DataSet ExecuteDataSet(string tablename, string sql, params object[] parameters)
        {
            return ExecuteDataSet(tablename, CreateCommand(sql), parameters);
        }

        /// <summary>
        /// Returns a DataSet from a Sql Command string passed in.
        /// 
        /// </summary>
        /// <param name="tableName"/><param name="command"/><param name="parameters"/>
        /// <returns/>
        public virtual DataSet ExecuteDataSet(DataSet dataSet, string tableName, DbCommand command, params object[] parameters)
        {
            SetError();
            if (dataSet == null)
                dataSet = new DataSet();
            DbDataAdapter dataAdapter = dbProvider.CreateDataAdapter();
            dataAdapter.SelectCommand = command;
            if (ExecuteWithSchema)
                dataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            AddParameters(command, parameters);
            DataTable dataTable = new DataTable(tableName);
            if (dataSet.Tables.Contains(tableName))
                dataSet.Tables.Remove(tableName);
            try
            {
                dataAdapter.Fill(dataSet, tableName);
            }
            catch (Exception ex)
            {
                SetError(ex.Message);
                return null;
            }
            finally
            {
                CloseConnection(command);
            }
            return dataSet;
        }

        /// <summary>
        /// Returns a DataTable from a Sql Command string passed in.
        /// 
        /// </summary>
        /// <param name="tablename"/><param name="Command"/><param name="parameters"/>
        /// <returns/>
        public virtual DataSet ExecuteDataSet(DataSet dataSet, string tablename, string sql, params object[] parameters)
        {
            using (DbCommand command = CreateCommand(sql, parameters))
            {

                if (command == null)
                    return null;
                else
                    return ExecuteDataSet(dataSet, tablename, command, new object[0]);
            }
        }

        /// <summary>
        /// Executes a command and returns a scalar value from it
        /// 
        /// </summary>
        /// <param name="SqlCommand">A SQL Command object</param>
        /// <returns>
        /// value or null on failure
        /// </returns>
        public virtual object ExecuteScalar(DbCommand command, params object[] parameters)
        {
            SetError();
            AddParameters(command, parameters);
            object obj = null;
            try
            {
                obj = command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                SetError(ex.GetBaseException());
            }
            finally
            {
                CloseConnection();
            }
            return obj;
        }

        /// <summary>
        /// Executes a Sql command and returns a single value from it.
        /// 
        /// </summary>
        /// <param name="Sql">Sql string to execute</param><param name="Parameters">Any named SQL parameters</param>
        /// <returns>
        /// Result value or null. Check ErrorMessage on Null if unexpected
        /// </returns>
        public virtual object ExecuteScalar(string sql, params object[] parameters)
        {
            SetError();
            using (DbCommand command = CreateCommand(sql, parameters))
            {
                if (command == null)
                    return null;
                else
                    return ExecuteScalar(command, null);
            }
        }

        /// <summary>
        /// Closes a connection
        /// 
        /// </summary>
        /// <param name="Command"/>
        public virtual void CloseConnection(DbCommand Command)
        {
            if (Transaction != null)
                return;
            if (Command.Connection != null && Command.Connection.State != ConnectionState.Closed)
                Command.Connection.Close();
            _Connection = null;
        }

        /// <summary>
        /// Closes an active connection. If a transaction is pending the
        ///             connection is held open.
        /// 
        /// </summary>
        public virtual void CloseConnection()
        {
            if (Transaction != null)
                return;
            if (_Connection != null && _Connection.State != ConnectionState.Closed)
                _Connection.Close();
            _Connection = null;
        }



        /// <summary>
        /// Executes a long SQL script that contains batches (GO commands). This code
        ///             breaks the script into individual commands and captures all execution errors.
        /// 
        ///             If ContinueOnError is false, operations are run inside of a transaction and
        ///             changes are rolled back. If true commands are accepted even if failures occur
        ///             and are not rolled back.
        /// 
        /// </summary>
        /// <param name="script"/><param name="continueOnError"/><param name="scriptIsFile"/>
        /// <returns/>
        public bool RunSqlScript(string script, bool continueOnError = false, bool scriptIsFile = false)
        {
            SetError();
            if (scriptIsFile)
            {
                try
                {
                    script = File.ReadAllText(script);
                }
                catch (Exception ex)
                {
                    SetError(ex.Message);
                    return false;
                }
            }
            string[] strArray = Regex.Split(script.Replace("\r\n", "\n").Replace("\r", "\n") + "\n", "GO\n");
            string str = "";
            if (!continueOnError)
                BeginTransaction();
            foreach (string sql in strArray)
            {
                if (!string.IsNullOrEmpty(sql.TrimEnd()) && ExecuteNonQuery(sql, new object[0]) == -1)
                {
                    str = ErrorMessage + sql;
                    if (!continueOnError)
                    {
                        RollbackTransaction();
                        return false;
                    }
                }
            }
            if (!continueOnError)
                CommitTransaction();
            if (string.IsNullOrEmpty(str))
                return true;
            ErrorMessage = str;
            return false;
        }



        /// <summary>
        /// Starts a new transaction on this connection/instance
        /// 
        /// </summary>
        /// 
        /// <returns/>
        public virtual bool BeginTransaction()
        {
            if (_Connection == null && !OpenConnection())
                return false;
            Transaction = _Connection.BeginTransaction();
            return Transaction != null;
        }

        /// <summary>
        /// Commits all changes to the database and ends the transaction
        /// 
        /// </summary>
        /// 
        /// <returns/>
        public virtual bool CommitTransaction()
        {
            if (Transaction == null)
            {
                SetError("No active Transaction to commit.");
                return false;
            }
            else
            {
                Transaction.Commit();
                Transaction = null;
                CloseConnection();
                return true;
            }
        }

        /// <summary>
        /// Rolls back a transaction
        /// 
        /// </summary>
        /// 
        /// <returns/>
        public virtual bool RollbackTransaction()
        {
            if (Transaction == null)
                return true;
            Transaction.Rollback();
            Transaction = null;
            CloseConnection();
            return true;
        }

        /// <summary>
        /// Sets the error message for the failure operations
        /// 
        /// </summary>
        /// <param name="Message"/>
        protected virtual void SetError(string Message, int errorNumber)
        {
            if (string.IsNullOrEmpty(Message))
            {
                ErrorMessage = string.Empty;
                ErrorNumber = 0;
            }
            else
            {
                ErrorMessage = Message;
                ErrorNumber = errorNumber;
            }
        }

        /// <summary>
        /// Sets the error message and error number.
        /// 
        /// </summary>
        /// <param name="message"/>
        protected virtual void SetError(string message)
        {
            SetError(message, 0);
        }

        protected virtual void SetError(DbException ex)
        {
            SetError(ex.Message, ex.ErrorCode);
        }

        protected virtual void SetError(SqlException ex)
        {
            SetError(ex.Message, ex.Number);
        }

        protected virtual void SetError(Exception ex)
        {
            if (ex is SqlException)
                SetError(ex as SqlException);
            else if (ex is DbException)
                SetError(ex as DbException);
            else
                SetError(ex.Message, 0);
        }

        /// <summary>
        /// Sets the error message for failure operations.
        /// 
        /// </summary>
        protected virtual void SetError()
        {
            SetError(null, 0);
        }

        public void Dispose()
        {
            if (_Connection == null)
                return;
            CloseConnection();
        }
    }
}


