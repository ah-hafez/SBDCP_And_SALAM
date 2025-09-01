using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Persistence
{
    public interface IConnectionStringBuilder
    {
        string UpdateDatabaseName(string connectionString, string databaseNameToUpdate);
    }
}
