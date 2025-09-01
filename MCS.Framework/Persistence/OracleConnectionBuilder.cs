using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace YESSER.NCS.Framework.Persistence
{
    public class OracleConnectionBuilder : IOracleConnectionStringBuilder
    {
        public string UpdateDatabaseName(string connectionString, string databaseNameToUpdate)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            sqlConnectionStringBuilder.UserID = databaseNameToUpdate;
            return sqlConnectionStringBuilder.ConnectionString;
        }
    }
}
