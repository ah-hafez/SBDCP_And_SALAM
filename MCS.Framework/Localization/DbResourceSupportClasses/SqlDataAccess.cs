using System;
using System.Data.Common;

namespace MCS.Framework.Localization
{
    public class SqlDataAccess : DataAccessBase, IDataAccess, IDisposable
    {
        public SqlDataAccess()
        {
            this.dbProvider = DbProviderFactories.GetFactory("System.Data.SqlClient");
        }

        public SqlDataAccess(string connectionString)
            : base(connectionString)
        {
            this.dbProvider = DbProviderFactories.GetFactory("System.Data.SqlClient");
        }

        //public SqlDataAccess(string connectionString, string providerName)
        //    : base(connectionString, providerName)
        //{
        //}       
    }
}
