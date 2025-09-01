using System;
using System.Data.Common;

namespace MCS.Framework.Localization
{
    public class OracleDataAccess : DataAccessBase, IDataAccess, IDisposable
    {
        public OracleDataAccess()
        {
            this.dbProvider = DbProviderFactories.GetFactory("Oracle.ManagedDataAccess.Client");
        }

        public OracleDataAccess(string connectionString)
            : base(connectionString)
        {
            this.dbProvider = DbProviderFactories.GetFactory("Oracle.ManagedDataAccess.Client");
        }

        //public OracleDataAccess(string connectionString, string providerName)
        //    : base(connectionString, providerName)
        //{
        //}       
    }
}
