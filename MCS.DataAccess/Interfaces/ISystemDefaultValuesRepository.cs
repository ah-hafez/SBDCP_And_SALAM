using System.Collections.Generic;
using MCS.Domain;


namespace MCS.DataAccess
{    public interface ISystemDefaultValuesRepository : IRepository<SystemDefaultValues>
    {
        IList<SystemDefaultValues> GetSystemDefaultValue();
    }
}


