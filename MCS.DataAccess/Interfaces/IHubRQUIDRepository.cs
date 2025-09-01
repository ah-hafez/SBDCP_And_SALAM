using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IHubRQUIDRepository : IRepository<HubRQUID>
    {
        long GetByTransactionNumberByRQUID(string rQUID);
        HubRQUID GetByRQUID(string rQUID);
    }
}
