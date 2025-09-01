using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface ISpecificLevelRepository : IRepository<SpecificLevel>
    {

        int AddSpecificLevel(SpecificLevel specificLevel);
        void UpdateSpecificLevel(SpecificLevel specificLevel);
        void DeleteSpecificLevel(int id);
        SpecificLevel GetSpecificLevelById(int specificLevel);
        IList<SpecificLevel> GetSpecificLevels(SearchCriteria searchCriteria, out int rowsCount);
        IList<SpecificLevel> GetSpecificLevels(string cultureName);
        IList<SpecificLevel> GetSpecificLevels(TransactionCategories transactionCategories, string cultureName);
    }
}
