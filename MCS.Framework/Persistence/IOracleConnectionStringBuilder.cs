using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Persistence
{
    public interface IOracleConnectionStringBuilder
    {
        string UpdateDatabaseName(string connectionString, string databaseNameToUpdate);
    }
}
