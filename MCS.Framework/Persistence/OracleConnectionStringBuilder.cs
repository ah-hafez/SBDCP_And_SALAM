using System.Data.SqlClient;

namespace MCS.Framework.Persistence
{
    public class OracleConnectionStringBuilder : IOracleConnectionStringBuilder
    {
        public string UpdateDatabaseName(string connectionString, string databaseNameToUpdate)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
            {
                UserID = databaseNameToUpdate,
                //Password = databaseNameToUpdate
            };
            return sqlConnectionStringBuilder.ConnectionString;
        }
    }
}
