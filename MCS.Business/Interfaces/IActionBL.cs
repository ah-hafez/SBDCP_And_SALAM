using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Domain;

namespace MCS.Business
{
    public interface IActionBL
    {
        int AddAction(Action process);
        void UpdateAction(Action process);
        void DeleteAction(IList<int> ids, out IList<int> actionsCannotBeDeleted);
        IList<Action> GetAction(SearchCriteria searchCriteria, out int rowsCount, string cultureName);
        IList<Action> GetAllAction(string cultureName);
        Action GetActionById(int processId);
        void ChangeEntitiesNameBeforeMove(ChangeEntityName changeEntityName);
        List<UsersClearance> CheckUserClearance(List<int> usersIds, string cultureName);
    }
}
