using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.DataAccess
{
    public interface ITrayRepository : IRepository<Tray>
    {
        void UpdateTray(Tray tray);
        void UpdateTrays(IList<Tray> trays);
        Tray GetTrayById(int id);
        IList<Tray> GetAllTrays(string cultureName);
        IList<Tray> GetTrays(SearchCriteriaCustom searchCriteria, out int rowsCount);
    }
}
