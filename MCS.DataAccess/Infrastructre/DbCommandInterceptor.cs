using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DataAccess
{
    public class DbCommandInterceptor: IDbCommandInterceptor
    {
        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<Int32> interceptionContext)
        {
        }

        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
        }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            addQueryHint(command);
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
        }

        private static void addQueryHint(IDbCommand command)
        {
            if (command.CommandType != CommandType.Text || !(command is SqlCommand))
                return;

            if (command.CommandText.StartsWith("select", StringComparison.OrdinalIgnoreCase) && !command.CommandText.Contains("SET ARITHABORT ON"))
            {
                command.CommandText = "SET ARITHABORT ON; " + command.CommandText;
            }
        }
    }
}
