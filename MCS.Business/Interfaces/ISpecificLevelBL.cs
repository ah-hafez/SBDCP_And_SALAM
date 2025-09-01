using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;

namespace MCS.Business
{
    public interface ISpecificLevelBL
    {
        int AddSpecificLevel(SpecificLevel specificLevel);
        void UpdateSpecificLevel(SpecificLevel specificLevel);
        void DeleteSpecificLevels(IList<int> ids, out IList<int> specificLevelsCannotBeDeleted);
        SpecificLevel GetSpecificLevelById(int specificLevelId);
        IList<SpecificLevel> GetSpecificLevels(SearchCriteria searchCriteria, out int rowsCount);
        IList<SpecificLevel> GetSpecificLevels(TransactionCategories transactionCategories, string cultureName);
    }
}
