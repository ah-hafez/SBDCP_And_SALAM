using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Persistence
{
    public class SqlConnectionBuilder : IConnectionStringBuilder
    {
        public string UpdateDatabaseName(string connectionString, string databaseNameToUpdate)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

            sqlConnectionStringBuilder.InitialCatalog = databaseNameToUpdate;

            return sqlConnectionStringBuilder.ConnectionString;
        }
    }
}
